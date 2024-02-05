using Microsoft.AspNetCore.SignalR;
using UdemySignalR.API.Models;

namespace UdemySignalR.API.Hubs
{
    public class ProductHub : Hub<IProductHub>
    {
        public async Task SendProduct(Product p)
        {
            // Strongly Typed Hubs : interfacede belirtilen ad direkt olarak kullanilir. sendasync() kullanilmasina gerek kalmaz.
            await Clients.All.ReceiveProduct(p);
        }
    }
}
