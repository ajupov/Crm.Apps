using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Identities.Models;
using Crm.Apps.Identities.Parameters;
using Crm.Apps.Identities.Services;
using Crm.Common.UserContext;
using Crm.Common.UserContext.Attributes;
using Crm.Infrastructure.ApiDocumentation.Attributes;
using Crm.Infrastructure.Mvc;
using Crm.Utils.Enums;
using Crm.Utils.Guid;
using Crm.Utils.String;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Identities.Controllers
{
    [ApiController]
    [IgnoreApiDocumentation]
    [Route("Api/Identities")]
    public class IdentitiesApiController : DefaultApiController
    {
        private readonly IUserContext _userContext;
        private readonly IIdentitiesService _identitiesService;

        public IdentitiesApiController(
            IUserContext userContext,
            IIdentitiesService identitiesService)
        {
            _userContext = userContext;
            _identitiesService = identitiesService;
        }

        [HttpGet("GetTypes")]
        [RequirePrivileged]
        public ActionResult<Dictionary<IdentityType, string>> GetTypes()
        {
            return Get(EnumsExtensions.GetAsDictionary<IdentityType>());
        }

        [HttpGet("Get")]
        [RequirePrivileged]
        public async Task<ActionResult<Identity>> Get(
            [Required] Guid id,
            CancellationToken ct = default)
        {
            var identity = await _identitiesService.GetAsync(id, ct);

            if (identity != null)
            {
                identity.PasswordHash = null;
            }

            return Get(identity);
        }

        [HttpPost("GetList")]
        [RequirePrivileged]
        public Task<ActionResult<Identity[]>> GetList(
            [Required] Guid[] ids,
            CancellationToken ct = default)
        {
            return Get(_identitiesService.GetListAsync(ids, ct));
        }

        [HttpPost("GetPagedList")]
        [RequirePrivileged]
        public Task<ActionResult<Identity[]>> GetPagedList(
            [Required] IdentityGetPagedListParameter parameter,
            CancellationToken ct = default)
        {
            return Get(_identitiesService.GetPagedListAsync(parameter, ct));
        }

        [HttpPost("Create")]
        [RequirePrivileged]
        public Task<ActionResult<Guid>> Create(
            [Required] Identity identity,
            CancellationToken ct = default)
        {
            return Create(_identitiesService.CreateAsync(_userContext.UserId, identity, ct));
        }

        [HttpPost("Update")]
        [RequirePrivileged]
        public async Task<ActionResult> Update(
            [Required] Identity identity,
            CancellationToken ct = default)
        {
            var oldIdentity = await _identitiesService.GetAsync(identity.Id, ct);

            return await Action(
                _identitiesService.UpdateAsync(_userContext.UserId, oldIdentity, identity, ct), oldIdentity);
        }

        [HttpPost("SetPassword")]
        [RequirePrivileged]
        public async Task<ActionResult> SetPassword(
            [Required] SetPasswordParameter parameter,
            CancellationToken ct = default)
        {
            var identity = await _identitiesService.GetAsync(parameter.Id, ct);
            
            return await Action(
                _identitiesService.SetPasswordAsync(_userContext.UserId, identity, parameter.Password, ct), identity);
        }

        [HttpGet("IsPasswordCorrect")]      
        [RequirePrivileged]
        public async Task<ActionResult<bool>> IsPasswordCorrect(
            IsPasswordCorrectParameter parameter,
            CancellationToken ct = default)
        {
            var identity = await _identitiesService.GetAsync(parameter.Id, ct);
            if (identity == null)
            {
                return NotFound();
            }

            return await _identitiesService.IsPasswordCorrectAsync(_userContext.UserId, identity, parameter.Password,
                ct);
        }

        [HttpPost("Verify")]
        [RequireAny(Permission.System, Permission.Development)]
        public async Task<ActionResult> Verify(List<Guid> ids, CancellationToken ct = default)
        {
            if (ids == null || ids.All(x => x.IsEmpty()))
            {
                return BadRequest();
            }

            await _identitiesService.VerifyAsync(_userContext.UserId, ids, ct);

            return NoContent();
        }

        [HttpPost("Unverify")]
        [RequireAny(Permission.System, Permission.Development)]
        public async Task<ActionResult> Unverify(List<Guid> ids, CancellationToken ct = default)
        {
            if (ids == null || ids.All(x => x.IsEmpty()))
            {
                return BadRequest();
            }

            await _identitiesService.UnverifyAsync(_userContext.UserId, ids, ct);

            return NoContent();
        }

        [HttpPost("SetAsPrimary")]
        [RequireAny(Permission.System, Permission.Development)]
        public async Task<ActionResult> SetAsPrimary(List<Guid> ids, CancellationToken ct = default)
        {
            if (ids == null || ids.All(x => x.IsEmpty()))
            {
                return BadRequest();
            }

            await _identitiesService.SetAsPrimaryAsync(_userContext.UserId, ids, ct);

            return NoContent();
        }

        [HttpPost("ResetAsPrimary")]
        [RequireAny(Permission.System, Permission.Development)]
        public async Task<ActionResult> ResetAsPrimary(List<Guid> ids, CancellationToken ct = default)
        {
            if (ids == null || ids.All(x => x.IsEmpty()))
            {
                return BadRequest();
            }

            await _identitiesService.ResetAsPrimaryAsync(_userContext.UserId, ids, ct);

            return NoContent();
        }
    }
}