using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using System.Reflection;

namespace SimpleAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class WeatherForecastController : ControllerBase
    {
        private ApplicationPartManager _cPartManager;
        public WeatherForecastController(ApplicationPartManager partManager, ILogger<WeatherForecastController> logger)
        {
            _cPartManager = partManager;
            _logger = logger;
        }
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet(Name = "AddController")]
        public void AddController(string assName = "ExtController.dll")
        {
            // Dynamically load assembly 
            Assembly assembly = Assembly.LoadFrom($@"d:\temp\ass\{assName}");

            // Add controller to the application
            AssemblyPart _part = new AssemblyPart(assembly);
            _cPartManager.ApplicationParts.Add(_part);
            // Notify change
            MyActionDescriptorChangeProvider.Instance.HasChanged = true;
            MyActionDescriptorChangeProvider.Instance.TokenSource.Cancel();

        }
    }
}
