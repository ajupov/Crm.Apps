﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Accounts.Models;
using Crm.Apps.Areas.Accounts.Services;
using Crm.Common.UserContext;
using Crm.Common.UserContext.Attributes;
using Crm.Utils.Guid;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Areas.Accounts.Controllers
{
    [ApiController]
    [Route("Api/Accounts")]
    public class AccountsController : ControllerBase
    {
        private readonly IUserContext _userContext;
        private readonly IAccountsService _accountsService;

        public AccountsController(IUserContext userContext, IAccountsService accountsService)
        {
            _userContext = userContext;
            _accountsService = accountsService;
        }

        [HttpGet("Get")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.TechnicalSupport)]
        public async Task<ActionResult<Account>> Get([FromQuery] Guid id, CancellationToken ct = default)
        {
            if (id.IsEmpty())
            {
                return BadRequest();
            }

            var account = await _accountsService.GetAsync(id, ct).ConfigureAwait(false);
            if (account == null)
            {
                return NotFound();
            }

            return account;
        }

        [HttpGet("GetList")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.TechnicalSupport)]
        public async Task<ActionResult<List<Account>>> GetList([FromQuery] List<Guid> ids,
            CancellationToken ct = default)
        {
            if (ids == null || ids.All(x => x.IsEmpty()))
            {
                return BadRequest();
            }

            return await _accountsService.GetListAsync(ids, ct).ConfigureAwait(false);
        }

        [HttpGet("GetPagedList")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.TechnicalSupport)]
        public async Task<ActionResult<List<Account>>> GetPagedList(
            [FromQuery] bool? isLocked = default,
            [FromQuery] bool? isDeleted = default,
            [FromQuery] DateTime? minCreateDate = default,
            [FromQuery] DateTime? maxCreateDate = default,
            [FromQuery] int offset = default,
            [FromQuery] int limit = 10,
            [FromQuery] string sortBy = default,
            [FromQuery] string orderBy = default,
            CancellationToken ct = default)
        {
            return await _accountsService.GetPagedListAsync(isLocked, isDeleted, minCreateDate, maxCreateDate, offset,
                limit, sortBy, orderBy, ct).ConfigureAwait(false);
        }

        [HttpPost("Create")]
        [RequireAny(Permission.System, Permission.Development)]
        public async Task<ActionResult<Guid>> Create(CancellationToken ct = default)
        {
            var id = await _accountsService.CreateAsync(_userContext.UserId, ct).ConfigureAwait(false);

            return Created(nameof(Get), id);
        }

        [HttpPost("Update")]
        [RequireAny(Permission.System, Permission.Development)]
        public async Task<ActionResult> Update([FromBody] Account account, CancellationToken ct = default)
        {
            if (account.Id.IsEmpty())
            {
                return BadRequest();
            }

            var oldAccount = await _accountsService.GetAsync(account.Id, ct).ConfigureAwait(false);
            if (oldAccount == null)
            {
                return NotFound();
            }

            await _accountsService.UpdateAsync(_userContext.UserId, oldAccount, account, ct).ConfigureAwait(false);

            return NoContent();
        }

        [HttpPost("Lock")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.TechnicalSupport)]
        public async Task<ActionResult> Lock([FromBody] List<Guid> ids, CancellationToken ct = default)
        {
            if (ids == null || ids.All(x => x.IsEmpty()))
            {
                return BadRequest();
            }

            await _accountsService.LockAsync(_userContext.UserId, ids, ct).ConfigureAwait(false);

            return NoContent();
        }

        [HttpPost("Unlock")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.TechnicalSupport)]
        public async Task<ActionResult> Unlock([FromBody] List<Guid> ids, CancellationToken ct = default)
        {
            if (ids == null || ids.All(x => x.IsEmpty()))
            {
                return BadRequest();
            }

            await _accountsService.UnlockAsync(_userContext.UserId, ids, ct).ConfigureAwait(false);

            return NoContent();
        }

        [HttpPost("Delete")]
        [RequireAny(Permission.System, Permission.Development)]
        public async Task<ActionResult> Delete([FromBody] List<Guid> ids, CancellationToken ct = default)
        {
            if (ids == null || ids.All(x => x.IsEmpty()))
            {
                return BadRequest();
            }

            await _accountsService.DeleteAsync(_userContext.UserId, ids, ct).ConfigureAwait(false);

            return NoContent();
        }

        [HttpPost("Restore")]
        [RequireAny(Permission.System, Permission.Development)]
        public async Task<ActionResult> Restore([FromBody] List<Guid> ids, CancellationToken ct = default)
        {
            if (ids == null || ids.All(x => x.IsEmpty()))
            {
                return BadRequest();
            }

            await _accountsService.RestoreAsync(_userContext.UserId, ids, ct).ConfigureAwait(false);

            return NoContent();
        }
    }
}