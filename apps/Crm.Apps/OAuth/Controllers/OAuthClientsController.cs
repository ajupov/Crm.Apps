using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.OAuth.Models;
using Crm.Apps.OAuth.Parameters;
using Crm.Apps.OAuth.Services;
using Crm.Common.UserContext;
using Crm.Common.UserContext.Attributes;
using Crm.Infrastructure.ApiDocumentation.Attributes;
using Crm.Infrastructure.Mvc;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.OAuth.Controllers
{
    [ApiController]
    [IgnoreApiDocumentation]
    [Route("Api/OAuthClients")]
    public class OAuthClientsController : DefaultApiController
    {
        private readonly IUserContext _userContext;
        private readonly IOAuthClientsService _oauthClientsService;

        public OAuthClientsController(
            IUserContext userContext,
            IOAuthClientsService oauthClientsService)
        {
            _userContext = userContext;
            _oauthClientsService = oauthClientsService;
        }

        [HttpGet("Get")]
        [RequirePrivileged]
        public Task<ActionResult<OAuthClient>> Get(
            [Required] int id,
            CancellationToken ct = default)
        {
            return Get(_oauthClientsService.GetAsync(id, ct));
        }

        [HttpPost("GetList")]
        [RequirePrivileged]
        public Task<ActionResult<OAuthClient[]>> GetList(
            [Required] int[] ids,
            CancellationToken ct = default)
        {
            return Get(_oauthClientsService.GetListAsync(ids, ct));
        }

        [HttpPost("GetPagedList")]
        [RequirePrivileged]
        public Task<ActionResult<OAuthClient[]>> GetPagedList(
            OAuthClientGetPagedListParameter parameter,
            CancellationToken ct = default)
        {
            return Get(_oauthClientsService.GetPagedListAsync(parameter, ct));
        }

        [HttpPost("Create")]
        [RequirePrivileged]
        public Task<ActionResult<Guid>> Create(
            [Required] OAuthClient client,
            CancellationToken ct = default)
        {
            return Create(_oauthClientsService.CreateAsync(_userContext.UserId, client, ct), ct);
        }

        [HttpPost("Update")]
        [RequirePrivileged]
        public async Task<ActionResult> Update(
            [Required] OAuthClient oauthClient,
            CancellationToken ct = default)
        {
            var oldOAuthClient = await _oauthClientsService.GetAsync(oauthClient.Id, ct);

            return await Action(
                _oauthClientsService.UpdateAsync(_userContext.UserId, oldOAuthClient, oauthClient, ct), oldOAuthClient);
        }

        [HttpPost("Lock")]
        [RequirePrivileged]
        public Task<ActionResult> Lock(
            [Required] int[] ids,
            CancellationToken ct = default)
        {
            return Action(_oauthClientsService.LockAsync(_userContext.UserId, ids, ct));
        }

        [HttpPost("Unlock")]
        [RequirePrivileged]
        public Task<ActionResult> Unlock(
            [Required] int[] ids,
            CancellationToken ct = default)
        {
            return Action(_oauthClientsService.UnlockAsync(_userContext.UserId, ids, ct));
        }

        [HttpPost("Delete")]
        [RequirePrivileged]
        public Task<ActionResult> Delete(
            [Required] int[] ids,
            CancellationToken ct = default)
        {
            return Action(_oauthClientsService.DeleteAsync(_userContext.UserId, ids, ct));
        }

        [HttpPost("Restore")]
        [RequirePrivileged]
        public Task<ActionResult> Restore(
            [Required] int[] ids,
            CancellationToken ct = default)
        {
            return Action(_oauthClientsService.RestoreAsync(_userContext.UserId, ids, ct));
        }
    }
}