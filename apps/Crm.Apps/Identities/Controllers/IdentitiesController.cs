using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Identities.Models;
using Crm.Apps.Identities.Parameters;
using Crm.Apps.Identities.Services;
using Crm.Common.UserContext;
using Crm.Common.UserContext.Attributes;
using Crm.Utils.Enums;
using Crm.Utils.Guid;
using Crm.Utils.String;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Identities.Controllers
{
    [ApiController]
    [Route("Api/Identities")]
    public class IdentitiesController : ControllerBase
    {
        private readonly IIdentitiesService _identitiesService;

        public IdentitiesController(IIdentitiesService identitiesService)
        {
            _identitiesService = identitiesService;
        }

        [HttpGet("GetTypes")]
        [RequireAny(Permission.System, Permission.Development)]
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
            var id = await _identitiesService.CreateAsync(identity, ct).ConfigureAwait(false);

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

            await _identitiesService.UpdateAsync(oldIdentity, identity, ct).ConfigureAwait(false);

            return NoContent();
        }

        [HttpPost("SetPassword")]
        [RequireAny(Permission.System, Permission.Development)]
        public async Task<ActionResult> SetPassword(SetPasswordParameter parameter, CancellationToken ct = default)
        {
            if (parameter.Id.IsEmpty())
            {
                return BadRequest();
            }

            var identity = await _identitiesService.GetAsync(parameter.Id, ct).ConfigureAwait(false);
            if (identity == null)
            {
                return NotFound();
            }

            await _identitiesService.SetPasswordAsync(identity, parameter.Password, ct).ConfigureAwait(false);

            return NoContent();
        }

        [HttpGet("IsPasswordCorrect")]
        [RequireAny(Permission.System, Permission.Development)]
        public async Task<ActionResult<bool>> IsPasswordCorrect(Guid id, string password,
            CancellationToken ct = default)
        {
            if (id.IsEmpty() || password.IsEmpty())
            {
                return BadRequest();
            }

            var identity = await _identitiesService.GetAsync(id, ct).ConfigureAwait(false);
            if (identity == null)
            {
                return NotFound();
            }

            return await _identitiesService.IsPasswordCorrectAsync(identity, password, ct).ConfigureAwait(false);
        }

        [HttpPost("Verify")]
        [RequireAny(Permission.System, Permission.Development)]
        public async Task<ActionResult> Verify(List<Guid> ids, CancellationToken ct = default)
        {
            if (ids == null || ids.All(x => x.IsEmpty()))
            {
                return BadRequest();
            }

            await _identitiesService.VerifyAsync(ids, ct).ConfigureAwait(false);

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

            await _identitiesService.UnverifyAsync(ids, ct).ConfigureAwait(false);

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

            await _identitiesService.SetAsPrimaryAsync(ids, ct).ConfigureAwait(false);

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

            await _identitiesService.ResetAsPrimaryAsync(ids, ct).ConfigureAwait(false);

            return NoContent();
        }
    }
}