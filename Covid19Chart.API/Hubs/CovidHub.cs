﻿using Covid19Chart.API.Services;
using Microsoft.AspNetCore.SignalR;

namespace Covid19Chart.API.Hubs
{
    public class CovidHub:Hub
    {

        private readonly CovidService _covidService;

        public CovidHub(CovidService covidService)
        {
            _covidService = covidService;
        }

        public async Task GetCovidList()
        {
            await Clients.All.SendAsync("ReceiveCovidList",_covidService.GetCovidChartList());
        }
    }
}
