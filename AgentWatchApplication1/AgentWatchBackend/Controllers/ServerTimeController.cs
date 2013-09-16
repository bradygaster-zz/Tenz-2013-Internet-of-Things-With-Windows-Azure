using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AgentWatchBackend.Controllers
{
    public class ServerTimeController : ApiController
    {
        public string Get()
        {
            return DateTime.Now.ToString("T");
        }
    }
}
