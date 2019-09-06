using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Infrastructure.Mvc
{
    public class DefaultApiController : ControllerBase
    {
        protected ActionResult<T> Get<T>(T model)
        {
            if (model == null)
            {
                return NotFound();
            }

            return model;
        }

        protected async Task<ActionResult> Action(Task task)
        {
            await task;

            return NoContent();
        }

        protected string IpAddress => Request.HttpContext.Connection.RemoteIpAddress.ToString();

        protected string UserAgent => Request.Headers["User-Agent"].ToString();
    }
}