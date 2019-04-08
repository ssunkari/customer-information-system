using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public HealthController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        public ActionResult GetHealth(int id)
        {
            var status = new
            {
                Environment = _hostingEnvironment.EnvironmentName
            };

            return Ok(status);
        }
    }
}
