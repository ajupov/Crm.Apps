using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Base.Accounts.Controllers
{
    [ApiController]
    [Route("")]
    public class DefaultController : ControllerBase
    {
        [HttpGet("")]
        public ActionResult Status()
        {
            return Ok();
        }
    }
}