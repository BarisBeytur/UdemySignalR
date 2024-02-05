using Microsoft.AspNetCore.SignalR;

namespace Covid19Chart.API.Hubs
{
    public class CovidHub:Hub
    {
        public async Task GetCovidList()
        {
            await Clients.All.SendAsync("ReceiveCovidList","servisten covid19 verilerini al");
        }
    }
}
