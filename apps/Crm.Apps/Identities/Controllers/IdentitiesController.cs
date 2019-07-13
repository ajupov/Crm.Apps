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
        private readonly IUserContext _userContext;
        private readonly IIdentitiesService _identitiesService;

        public IdentitiesController(IUserContext userContext, IIdentitiesService identitiesService)
        {
            _userContext = userContext;
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

            var account = await _identitiesService.GetAsync(id, ct);
            if (account == null)
            {
                return NotFound();
            }

            account.PasswordHash = string.Empty;
            
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

            return await _identitiesService.GetListAsync(ids, ct);
        }

        [HttpPost("GetPagedList")]
        [RequireAny(Permission.System, Permission.Development)]
        public async Task<ActionResult<List<Identity>>> GetPagedList(IdentityGetPagedListParameter parameter,
            CancellationToken ct = default)
        {
            return await _identitiesService.GetPagedListAsync(parameter, ct);
        }

        [HttpPost("Create")]
        [RequireAny(Permission.System, Permission.Development)]
        public async Task<ActionResult<Guid>> Create(Identity identity, CancellationToken ct = default)
        {
            var id = await _identitiesService.CreateAsync(_userContext.UserId, identity, ct);

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

            var oldIdentity = await _identitiesService.GetAsync(identity.Id, ct);
            if (oldIdentity == null)
            {
                return NotFound();
            }

            await _identitiesService.UpdateAsync(_userContext.UserId, oldIdentity, identity, ct);

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

            var identity = await _identitiesService.GetAsync(parameter.Id, ct);
            if (identity == null)
            {
                return NotFound();
            }

            await _identitiesService.SetPasswordAsync(_userContext.UserId, identity, parameter.Password, ct)
                ;

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

            var identity = await _identitiesService.GetAsync(id, ct);
            if (identity == null)
            {
                return NotFound();
            }

            return await _identitiesService.IsPasswordCorrectAsync(_userContext.UserId, identity, password, ct)
                ;
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