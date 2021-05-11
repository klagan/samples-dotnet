using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyWeather.Bll;
using MyWeather.Models;

namespace MyWebAPI.V1.Controllers
{
    [ApiController]
    [ApiVersion( "1.0" )]
    [ApiExplorerSettings(GroupName = "v1")]
    // [Route("[controller]")]
    [Route( "api/v{version:apiVersion}/[controller]" )]
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
        /// Test description for 'GET' operation
        /// </summary>
        /// <returns>Sample return description</returns>
        /// <remarks>This operation retrieves a weather random forecast. Here is where I would write more detailed information</remarks>
        /// <code>
        /// int c = Math.Add(4, 5);
        /// if (c > 10)
        /// {
        ///     Console.WriteLine(c);
        /// }
        /// </code>
        /// <response code="200">Returns a list of weather forecasts</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<WeatherForecast>), StatusCodes.Status200OK)]
        public IActionResult Get()
        {
            return Ok(_weatherForecastCompute.Get());
        }

        /// <summary>
        /// A sample GET method
        /// This is an example of a GET operation for the WeatherForecast API
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
        /// A sample POST method. Ths is a sample POST operation for the WeatherForecast API
        /// </summary>
        /// <param name="sampleModel">a sample model value</param>
        /// <param name="someId">a sample ID property</param>
        /// <returns></returns>
        [HttpPost("SamplePost")]
        [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
        public IActionResult SamplePost([FromBody] SampleModel sampleModel, [FromQuery] int someId)
        {
            return Created(new System.Uri("/SamplePost", System.UriKind.Relative), $"{sampleModel.Id} => {sampleModel.Description} => {someId}");
        }
    }
}