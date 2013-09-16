using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TemperatureAndHumidityApi.Hubs;

namespace TemperatureAndHumidityApi.Controllers
{
    public class SensorController : ApiController
    {
        public bool Get(double temperature, double humidity)
        {
            GlobalHost.ConnectionManager.GetHubContext("sensor")
                .Clients.All.readingReceived(temperature, humidity);

            return true;
        }
    }
}
