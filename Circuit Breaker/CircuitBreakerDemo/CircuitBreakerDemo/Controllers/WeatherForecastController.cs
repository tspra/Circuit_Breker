using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CircuitBreakerDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IHttpClientFactory httpClientFactory;

        public WeatherForecastController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }
        // GET api/values
        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> Get()
        {
           HttpResponseMessage response = null;

            var breaker = new CircuitBreaker.CircuitBreaker();

            try
            {

                breaker.ExecuteAction( () =>
                {
                    var client = httpClientFactory.CreateClient("errorApi");
                    response =  client.GetAsync("weatherforecast").Result;

                });
            }
            //catch (CircuitBreakerOpenException ex)
            //{
                
            //}
            catch (Exception ex)
            {
             }
            return JsonConvert.DeserializeObject<string[]>(await response?.Content.ReadAsStringAsync());
            //var client = httpClientFactory.CreateClient("errorApi");
            //var response = await client.GetAsync("weatherforecast");
            //return JsonConvert.DeserializeObject<string[]>(await response.Content.ReadAsStringAsync());
        }
    }
}
