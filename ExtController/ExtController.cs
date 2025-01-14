using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;

namespace ExtController
{
    [ApiController]
    [Route("[controller]")]

    public class ExtController : ControllerBase
    {

        [HttpGet(Name = "GetWeatherType")]
        public IEnumerable<WeatherType> GetWeatherType()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherType
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55)
            })
            .ToArray();
        }
    }
}
