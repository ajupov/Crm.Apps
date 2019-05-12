using Crm.Common.UserContext;
using Crm.Common.UserContext.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Areas.Users.Controllers
{
    [ApiController]
    [Route("Api/Users")]
    public class UsersDefaultController : ControllerBase
    {
        [HttpGet("")]
        [RequireAny(Permission.System, Permission.Development)]
        public ActionResult Status()
        {
            return Ok();
        }
    }
}