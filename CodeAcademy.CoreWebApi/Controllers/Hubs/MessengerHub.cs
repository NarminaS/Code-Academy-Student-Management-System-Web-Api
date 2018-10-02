using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeAcademy.CoreWebApi.Controllers.Hubs
{
    public class MessengerHub:Hub
    {
        public async Task SendMessage(string userId, string message)
        {
            await Clients.Client(userId).SendAsync("Send", message);
        }
    }
}
