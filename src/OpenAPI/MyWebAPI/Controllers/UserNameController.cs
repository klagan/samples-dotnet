namespace MyWebAPI.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [ApiController][ApiExplorerSettings(GroupName = "MyUserNameAPI")]
    [Route("[controller]")]
    public class UserNameController : ControllerBase
    {
        private static readonly string[] UserNames = new[]
        {
            "Hugh", "Pugh", "Barney", "McGrew", "Cuthbert", "Dibble", "Grub"
        };

        private readonly ILogger<UserNameController> _logger;

        public UserNameController(ILogger<UserNameController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Test description for 'GET' method
        /// </summary>
        /// <returns>Sample return description</returns>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            var rng = new Random();
            
            var result =  Enumerable.Range(1, 5).Select(index =>
                    UserNames[rng.Next(UserNames.Length)])
                .ToArray();

            return result;
        }
    }
}