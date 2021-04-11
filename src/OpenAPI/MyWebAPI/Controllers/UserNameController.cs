namespace MyWebAPI.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using MyUserName.Bll;
    
    [ApiController]
    [ApiExplorerSettings(GroupName = "MyUserNameAPI")]
    [Route("[controller]")]
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
            return Ok(_userNameCompute.Get());
        }
    }
}