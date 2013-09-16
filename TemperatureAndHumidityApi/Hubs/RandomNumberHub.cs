using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Web;

namespace TemperatureAndHumidityApi.Hubs
{
    [HubName("random")]
    public class RandomNumberHub : Hub
    {
        public override System.Threading.Tasks.Task OnConnected()
        {
            var timer = new Timer();
            timer.Interval = 2000;
            timer.Elapsed += (e, s) =>
                {
                    Clients.Caller.receiveRandom(new Random().Next(0, 100));
                };
            timer.Start();

            return base.OnConnected();
        }
    }
}