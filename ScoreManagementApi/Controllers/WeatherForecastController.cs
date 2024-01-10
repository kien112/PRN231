using Microsoft.AspNetCore.Mvc;

namespace ScoreManagementApi.Controllers
***REMOVED***
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    ***REMOVED***
        private static readonly string[] Summaries = new[]
        ***REMOVED***
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
***REMOVED***;

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        ***REMOVED***
            _logger = logger;
***REMOVED***

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        ***REMOVED***
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            ***REMOVED***
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
    ***REMOVED***)
            .ToArray();
***REMOVED***
***REMOVED***
***REMOVED***