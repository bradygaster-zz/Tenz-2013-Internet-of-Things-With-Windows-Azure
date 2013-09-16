using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TemperatureAndHumidityApi.Hubs
{
    [HubName("sensor")]
    public class SensorDisplayHub : Hub
    {
        public void Initialize()
        {
            // nothing to do here
        }
    }
}