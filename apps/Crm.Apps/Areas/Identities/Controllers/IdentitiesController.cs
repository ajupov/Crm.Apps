using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Identities.Models;
using Crm.Apps.Areas.Identities.Parameters;
using Crm.Apps.Areas.Identities.Services;
using Crm.Common.UserContext;
using Crm.Common.UserContext.Attributes;
using Crm.Utils.Enums;
using Crm.Utils.Guid;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Areas.Identities.Controllers
{
    [ApiController]
    [Route("Api/Identities")]
    public class IdentitiesController : ControllerBase
    {
        private readonly IUserContext _userContext;
        private readonly IIdentitiesService _identitiesService;

        public IdentitiesController(IUserContext userContext, IIdentitiesService identitiesService)
        {
            _userContext = userContext;
            _identitiesService = identitiesService;
        }

        [HttpGet("GetTypes")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.TechnicalSupport)]
        public ActionResult<List<IdentityType>> GetTypes()
        {
            return EnumsExtensions.GetValues<IdentityType>().ToList();
        }
        
        [HttpGet("Get")]
        [RequireAny(Permission.System, Permission.Development)]
        public async Task<ActionResult<Identity>> Get(Guid id, CancellationToken ct = default)
        {
            if (id.IsEmpty())
            {
                return BadRequest();
            }

            var account = await _identitiesService.GetAsync(id, ct).ConfigureAwait(false);
            if (account == null)
            {
                return NotFound();
            }

            return account;
        }

        [HttpPost("GetList")]
        [RequireAny(Permission.System, Permission.Development)]
        public async Task<ActionResult<List<Identity>>> GetList(List<Guid> ids, CancellationToken ct = default)
        {
            if (ids == null || ids.All(x => x.IsEmpty()))
            {
                return BadRequest();
            }

            return await _identitiesService.GetListAsync(ids, ct).ConfigureAwait(false);
        }

        [HttpPost("GetPagedList")]
        [RequireAny(Permission.System, Permission.Development)]
        public async Task<ActionResult<List<Identity>>> GetPagedList(IdentityGetPagedListParameter parameter,
            CancellationToken ct = default)
        {
            return await _identitiesService.GetPagedListAsync(parameter, ct).ConfigureAwait(false);
        }

        [HttpPost("Create")]
        [RequireAny(Permission.System, Permission.Development)]
        public async Task<ActionResult<Guid>> Create(Identity identity, CancellationToken ct = default)
        {
            var id = await _identitiesService.CreateAsync(_userContext.UserId, identity, ct).ConfigureAwait(false);

            return Created(nameof(Get), id);
        }

        [HttpPost("Update")]
        [RequireAny(Permission.System, Permission.Development)]
        public async Task<ActionResult> Update(Identity identity, CancellationToken ct = default)
        {
            if (identity.Id.IsEmpty())
            {
                return BadRequest();
            }

            var oldIdentity = await _identitiesService.GetAsync(identity.Id, ct).ConfigureAwait(false);
            if (oldIdentity == null)
            {
                return NotFound();
            }

            await _identitiesService.UpdateAsync(_userContext.UserId, oldIdentity, identity, ct).ConfigureAwait(false);

            return NoContent();
        }

        [HttpPost("Verify")]
        [RequireAny(Permission.System, Permission.Development)]
        public async Task<ActionResult> Verify(List<Guid> ids, CancellationToken ct = default)
        {
            if (ids == null || ids.All(x => x.IsEmpty()))
            {
                return BadRequest();
            }

            await _identitiesService.VerifyAsync(_userContext.UserId, ids, ct).ConfigureAwait(false);

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

            await _identitiesService.UnverifyAsync(_userContext.UserId, ids, ct).ConfigureAwait(false);

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

            await _identitiesService.SetAsPrimaryAsync(_userContext.UserId, ids, ct).ConfigureAwait(false);

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

            await _identitiesService.ResetAsPrimaryAsync(_userContext.UserId, ids, ct).ConfigureAwait(false);

            return NoContent();
        }
    }
}