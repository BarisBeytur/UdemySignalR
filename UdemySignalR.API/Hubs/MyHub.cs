﻿using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using UdemySignalR.API.Models;

namespace UdemySignalR.API.Hubs
{
    public class MyHub : Hub
    {

        private readonly AppDbContext _context;

        public MyHub(AppDbContext context)
        {
            _context = context;
        }


        #region Declarations

        private static List<string> Names { get; set; } = new List<string>();
        private static int ClientCount { get; set; } = 0;
        public static int TeamCount { get; set; } = 7;

        #endregion

        public async Task SendProduct(Product p)
        {
            await Clients.All.SendAsync("ReceiveProduct",p);
        }

        public async Task SendName(string name)
        {

            if (Names.Count >= TeamCount)
            {
                await Clients.Caller.SendAsync("Error", $"Takım en fazla {TeamCount} kişi olabilir.");
            }
            else
            {
                Names.Add(name);
                await Clients.All.SendAsync("ReceiveName", name);
            }
        }

        public async Task GetNames()
        {
            await Clients.All.SendAsync("ReceiveNames", Names);
        }



        #region Groups

        public async Task AddToGroup(string teamName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, teamName);
        }

        public async Task RemoveToGroup(string teamName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, teamName);
        }

        public async Task SendNameByGroup(string Name, string teamName)
        {
            var team = _context.Teams.Where(x => x.Name == teamName).FirstOrDefault();

            if (team != null)
            {
                team.Users.Add(new User
                {
                    Name = Name,
                });
            }
            else
            {
                var newTeam = new Team() { Name = teamName, };

                _context.Teams.Add(newTeam);

                newTeam.Users.Add(new User { Name = Name });          
            }

            await _context.SaveChangesAsync();

            await Clients.Group(teamName).SendAsync("ReceiveMessageByGroup",Name,team.Id);
        }

        public async Task GetNamesByGroup()
        {

            Dictionary<int, List<User>> teamDictionary = new Dictionary<int, List<User>>();

            var teamA = _context.Users.Where(x => x.Team.Id == 1).ToList();
            var teamB = _context.Users.Where(x => x.Team.Id == 2).ToList();

            teamDictionary[1] = teamA;
            teamDictionary[2] = teamB;


            await Clients.All.SendAsync("ReceiveNamesByGroup", teamDictionary);
        }


        #endregion


        #region Server hub virtual methods

        public async override Task OnConnectedAsync()
        {
            ClientCount++;
            await Clients.All.SendAsync("ReceiveClientCount", ClientCount);
            await base.OnConnectedAsync();
        }

        public async override Task OnDisconnectedAsync(Exception? exception)
        {
            ClientCount--;
            await Clients.All.SendAsync("ReceiveClientCount", ClientCount);
            await base.OnDisconnectedAsync(exception);
        }

        #endregion

    }
}
