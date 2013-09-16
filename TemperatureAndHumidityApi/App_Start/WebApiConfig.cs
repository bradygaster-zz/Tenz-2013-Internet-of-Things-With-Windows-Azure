using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace TemperatureAndHumidityApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "Sensor",
                routeTemplate: "api/{controller}/{temperature}/{humidity}",
                defaults: new { temperature = 0, humidity = 0 }
            );

            // To disable tracing in your application, please comment out or remove the following line of code
            config.EnableSystemDiagnosticsTracing();

            // remove the xml formatter and leave JSON
            GlobalConfiguration.Configuration.Formatters.Remove(
                GlobalConfiguration.Configuration.Formatters.XmlFormatter
                );
        }
    }
}
