using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace LEDControllerSite.Controllers
{
    public class LEDController : ApiController
    {
        public async Task<string> Get(string color)
        {
            var ip = ConfigurationManager.AppSettings["MICROCONTROLLER_IP"];
            var port = ConfigurationManager.AppSettings["MICROCONTROLLER_PORT"];
            var microUrl = string.Format("http://{0}:{1}/{2}", ip, port, color);

            var client = new HttpClient();
            var response = await client.GetStringAsync(microUrl);

            return response;
        }
    }
}
