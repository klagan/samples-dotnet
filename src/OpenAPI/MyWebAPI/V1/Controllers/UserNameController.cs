using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyUserName.Bll;

namespace MyWebAPI.V1.Controllers
{
    [ApiController]
    [ApiVersion( "1.0" )]
    [ApiExplorerSettings(GroupName = "v1")]
    [Route( "api/v{version:apiVersion}/[controller]" )]
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
        /// Test description for 'GET' method
        /// </summary>
        /// <returns>Sample return description</returns>
        [HttpGet]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public IActionResult Get()
        {
            return Ok(_userNameCompute.Get());
        }
    }
}