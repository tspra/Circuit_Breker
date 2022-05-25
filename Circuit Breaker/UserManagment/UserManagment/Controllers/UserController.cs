using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserManagment.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
 

        public WeatherForecastController()
        {
          
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            throw new Exception("Error");
        }
    }
}
