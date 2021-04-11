namespace MyWebAPI.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Models;

    [ApiController][ApiExplorerSettings(GroupName = "MyWeatherAPI")]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Test description for 'GET' method
        /// </summary>
        /// <returns>Sample return description</returns>
        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = rng.Next(-20, 55),
                    Summary = Summaries[rng.Next(Summaries.Length)]
                })
                .ToArray();
        }

        /// <summary>
        /// A sample GET method
        /// </summary>
        /// <param name="sampleModel">a sample model value</param>
        /// <param name="someId">a sample ID property</param>
        /// <returns></returns>
        [HttpGet("SampleGet")]
        public string SampleGet([FromQuery] SampleModel sampleModel, [FromQuery] int someId)
        {
            return $"{sampleModel.Id} => {sampleModel.Description} => {someId}";
        }

        [HttpPost("SamplePost")]
        public string SamplePost([FromBody] SampleModel sampleModel, [FromQuery] int someId)
        {
            return $"{sampleModel.Id} => {sampleModel.Description} => {someId}";
        }
    }
}