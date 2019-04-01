using System.Threading;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Areas.Accounts.Controllers
{
    [ApiController]
    [Route("")]
    public class AccountsDefaultController : ControllerBase
    {
        [HttpGet("")]
        public ActionResult Status(CancellationToken ct = default)
        {
            return Ok();
        }
    }
}