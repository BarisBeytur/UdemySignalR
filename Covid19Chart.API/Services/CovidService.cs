using Covid19Chart.API.Hubs;
using Covid19Chart.API.Models;
using Microsoft.AspNetCore.SignalR;

namespace Covid19Chart.API.Services
{
    public class CovidService
    {
        private readonly AppDbContext _dbContext;
        private readonly IHubContext<CovidHub> _hubContext;

        public CovidService(AppDbContext dbContext, IHubContext<CovidHub> hubContext)
        {
            _dbContext = dbContext;
            _hubContext = hubContext;
        }

        public IQueryable<Covid> GetList()
        {
            return _dbContext.Covids.AsQueryable();
        }

        public async Task SaveCovid(Covid covid)
        {
            await _dbContext.Covids.AddAsync(covid);
            await _dbContext.SaveChangesAsync();
            await _hubContext.Clients.All.SendAsync("ReceiveCovidList","data");
        }


    }
}
