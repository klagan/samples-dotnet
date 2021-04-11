namespace MyWebAPI.Controllers
{
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using MyWeather.Bll;
    using MyWeather.Models;


    [ApiController]
    [ApiExplorerSettings(GroupName = "MyWeatherAPI")]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly Compute _weatherForecastCompute;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, Compute weatherForecastCompute)
        {
            _logger = logger;
            _weatherForecastCompute = weatherForecastCompute;
        }

        /// <summary>
        /// Test description for 'GET' method
        /// </summary>
        /// <returns>Sample return description</returns>
        /// <response code="200">Returns a list of weather forecasts</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<WeatherForecast>), StatusCodes.Status200OK)]
        public IActionResult Get()
        {
            return Ok(_weatherForecastCompute.Get());
        }

        /// <summary>
        /// A sample GET method
        /// </summary>
        /// <param name="sampleModel">a sample model value</param>
        /// <param name="someId">a sample ID property</param>
        /// <returns>a sample return value</returns>
        [HttpGet("SampleGet")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public IActionResult SampleGet([FromQuery] SampleModel sampleModel, [FromQuery] int someId)
        {
            return Ok($"{sampleModel.Id} => {sampleModel.Description} => {someId}");
        }

        /// <summary>
        /// A sample POST method
        /// </summary>
        /// <param name="sampleModel">a sample model value</param>
        /// <param name="someId">a sample ID property</param>
        /// <returns></returns>
        [HttpPost("SamplePost")]
        public IActionResult SamplePost([FromBody] SampleModel sampleModel, [FromQuery] int someId)
        {
            return Ok($"{sampleModel.Id} => {sampleModel.Description} => {someId}");
        }
    }
}