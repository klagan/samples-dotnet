using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyUserName.Bll;

namespace MyWebAPI.V2.Controllers
{
    [ApiController]
    [ApiVersion( "2.0" )]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = "v2")]
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
        public IActionResult Get()
        {
            var result = _userNameCompute.Get()
                .Select(x => x = $"{x}-v2");
            
            return Ok(result);
        }

        [HttpGet("OnlyOnV2")]
        [ApiVersion( "2.0" )]
        public IActionResult OnlyOnV2()
        {
            return Ok("i'm on version 2.0");
        }
    }
}