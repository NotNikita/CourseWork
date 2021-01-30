using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Comics.Web.Hubs
{
    public class UpdateHub : Hub
    {
        public async Task JoinGroup(int group)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, group.ToString());
        }
    }
}
