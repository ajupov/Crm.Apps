using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Infrastructure.Mvc
{
    public class DefaultApiController : ControllerBase
    {
        protected ActionResult<T> Get<T>(
            T model)
        {
            if (model == null)
            {
                return NotFound();
            }

            return model;
        }

        protected async Task<ActionResult<T>> Get<T>(
            Task<T> getModelTask)
        {
            var model = await getModelTask;

            return model == null ? (ActionResult<T>) NotFound() : model;
        }

        protected async Task<ActionResult> Action(
            Task task)
        {
            await task;

            return NoContent();
        }

        protected async Task<ActionResult> Action<T>(
            Task task,
            T model = default(T))
        {
            if (model == null)
            {
                return NotFound();
            }

            return await Action(task);
        }

        protected async Task<ActionResult<Guid>> Create(
            Task<Guid> task)
        {
            var id = await task;

            return Created("Get", id);
        }
        
        protected string IpAddress => Request.HttpContext.Connection.RemoteIpAddress.ToString();

        protected string UserAgent => Request.Headers["User-Agent"].ToString();
    }
}