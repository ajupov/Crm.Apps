using System.Reflection;
using Ajupov.Infrastructure.All.Mvc;
using Crm.Common.All.UserContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Crm.Apps.Home.Controllers
{
    [Route("")]
    public class HomeController : DefaultMvcController
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet("Test")]
        [Authorize(Roles = "AccountOwning")]
        public ActionResult Test()
        {
            return Ok("123");
        }

        [HttpGet("")]
        public ActionResult Index()
        {
            var assembly = Assembly.GetEntryAssembly();
            var attribute = assembly?.GetCustomAttribute<AssemblyFileVersionAttribute>();

            var name = assembly?.GetName().Name;
            var version = attribute?.Version;
            var message = $"{name} {version}";

            _logger.LogInformation("Index request. message: {0}", message);

            return Content(message);
        }
    }
}