namespace MyWebAPI.V1.Controllers
{
    using System;
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using MyWeather.Bll;
    using MyWeather.Models;

    [ApiController]
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1")]
    // [Route("[controller]")]
    [Route("api/v{version:apiVersion}/[controller]")]
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
        ///     Test description for 'GET' operation
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
        // /// <param name="sampleModel">a sample model value</param>
        // /// <param name="someId">a sample ID property</param>
        /// <returns>a sample return value</returns>
        [HttpGet("SampleGet")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public IActionResult SampleGet([FromQuery] SampleModel sampleModel, [FromQuery] int someId)
        {
            return Ok($"{sampleModel.Id} => {sampleModel.Description} => {someId}");
        }

        /// <summary>
        ///     A sample POST method. Ths is a sample POST operation for the WeatherForecast API
        /// </summary>
        /// <param name="sampleModel">a sample model value</param>
        /// <param name="someId">a sample ID property</param>
        /// <returns></returns>
        [HttpPost("SamplePost")]
        [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
        public IActionResult SamplePost([FromBody] SampleModel sampleModel, [FromQuery] int someId)
        {
            return Created(new Uri("/SamplePost", UriKind.Relative),
                $"{sampleModel.Id} => {sampleModel.Description} => {someId}");
        }

        /// <summary>
        ///     this is the short summary of the operation.
        /// </summary>
        /// <remarks>this is a longer description of the API operation. <a href="https://www.google.com">google</a> <a href="#section/Authentication">go to authentication section (test)</a>
        /// testing anchor redoc: &lt;a href="#section/Authentication"&gt;TestPost&lt;/a&gt;
        /// testing anchor swagger: &lt;a href="?filter=/#Authentication"&gt;TestPost&lt;/a&gt;
        /// </remarks>
        /// <param name="sampleModel">this is the description for a sample model. testing anchor: <a href="?filter=TestPost">TestPost</a></param>
        /// <param name="myId" example="150273">
        ///     this is the description for
        ///     <b>
        ///         <i>myId</i>
        ///     </b>
        /// </param>
        /// <returns>this is the description of what the return value should be</returns>
        /// <response code="200">this is a description for the response code (200)</response>
        [HttpGet("SampleMethod")]
        [ProducesResponseType(typeof(SampleModel), StatusCodes.Status200OK)]
        public IActionResult SampleMethod([FromQuery] SampleModel sampleModel, [FromQuery] int myId)
        {
            return Ok(new SampleModel());
        }
    }
}