﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using UdemySignalR.API.Hubs;

namespace UdemySignalR.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {

        private readonly IHubContext<MyHub> _hubContext;

        public NotificationController(IHubContext<MyHub> hubContext)
        {
            _hubContext = hubContext;
        }

        [HttpGet("{teamCount}")]
        public async Task<IActionResult> SetTeamCount(int teamCount)
        {
            MyHub.TeamCount = teamCount;

            await _hubContext.Clients.All.SendAsync("Notify",$"Takımlar maksimum {teamCount} kişi olacaktır.");
            return Ok();
        }


    }
}
