namespace MyWebAPI.V1.Controllers
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using MyUserName.Bll;
    using Models;

    [ApiController]
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class UserNameController : ControllerBase
    {
        private readonly ILogger<UserNameController> _logger;
        private readonly Compute _userNameCompute;

        public UserNameController(ILogger<UserNameController> logger, Compute userNameCompute)
        {
            _logger = logger;
            _userNameCompute = userNameCompute;
        }

        /// <summary>
        ///     Test description for 'GET' method
        /// </summary>
        /// <returns>Sample return description</returns>
        [HttpGet]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public IActionResult Get([FromHeader(Name = "X-MyCustomHeader")]
                                 string myCustomHeader)
        {
            return Ok(_userNameCompute.Get());
        }

        [HttpGet("TestCustomResponseCode")]
        [ProducesResponseType(typeof(string), MyCustomStatusCode.ResponseCode)]
        public IActionResult TestCustomResponseCode()
        {
            Response.Headers.Add("X-response-from-controller", "test123");
            return  new MyCustomStatusCode("kaml sends his love and kisses!");
        }
    }
}