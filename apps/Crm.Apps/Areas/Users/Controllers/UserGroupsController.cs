using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Users.Models;
using Crm.Apps.Areas.Users.Services;
using Crm.Common.Types;
using Crm.Common.UserContext;
using Crm.Common.UserContext.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Areas.Users.Controllers
{
    [ApiController]
    [Route("Api/Users/Groups")]
    public class UserGroupsController : ControllerBase
    {
        private readonly IUserContext _userContext;
        private readonly IUserGroupsService _userGroupsService;

        public UserGroupsController(IUserContext userContext, IUserGroupsService userGroupsService)
        {
            _userContext = userContext;
            _userGroupsService = userGroupsService;
        }

        [HttpGet("Get")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.TechnicalSupport,
            Permission.AccountOwning)]
        public async Task<ActionResult<UserGroup>> Get([FromQuery] Guid id, CancellationToken ct = default)
        {
            if (id == Guid.Empty)
            {
                return BadRequest();
            }

            var group = await _userGroupsService.GetAsync(id, ct).ConfigureAwait(false);
            if (group == null)
            {
                return NotFound();
            }

            return ReturnIfAllowed(group, new[] {group.AccountId});
        }

        [HttpGet("GetList")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.TechnicalSupport,
            Permission.AccountOwning)]
        public async Task<ActionResult<List<UserGroup>>> GetList([FromQuery] List<Guid> ids,
            CancellationToken ct = default)
        {
            if (ids == null || ids.All(x => x == Guid.Empty))
            {
                return BadRequest();
            }

            var groups = await _userGroupsService.GetListAsync(ids, ct).ConfigureAwait(false);

            return ReturnIfAllowed(groups, groups.Select(x => x.AccountId));
        }

        [HttpGet("GetPagedList")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.TechnicalSupport,
            Permission.AccountOwning)]
        public async Task<ActionResult<List<UserGroup>>> GetPagedList(
            [FromQuery] Guid? accountId = default,
            [FromQuery] string name = default,
            [FromQuery] bool? isDeleted = default,
            [FromQuery] DateTime? minCreateDate = default,
            [FromQuery] DateTime? maxCreateDate = default,
            [FromQuery] int offset = default,
            [FromQuery] int limit = 10,
            [FromQuery] string sortBy = default,
            [FromQuery] string orderBy = default,
            CancellationToken ct = default)
        {
            var groups = await _userGroupsService.GetPagedListAsync(accountId, name, isDeleted, minCreateDate,
                    maxCreateDate, offset, limit, sortBy, orderBy, ct)
                .ConfigureAwait(false);

            return ReturnIfAllowed(groups, groups.Select(x => x.AccountId));
        }

        [HttpPost("Create")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.AccountOwning)]
        public async Task<ActionResult<Guid>> Create([FromBody] UserGroup group, CancellationToken ct = default)
        {
            if (group == null)
            {
                return BadRequest();
            }

            if (!_userContext.HasAny(Permission.System, Permission.Development, Permission.Administration,
                Permission.TechnicalSupport))
            {
                group.AccountId = _userContext.AccountId;
            }

            var id = await _userGroupsService.CreateAsync(_userContext.UserId, group, ct).ConfigureAwait(false);

            return Created(nameof(Get), id);
        }

        [HttpPost("Update")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.TechnicalSupport)]
        public async Task<ActionResult> Update([FromBody] UserGroup group, CancellationToken ct = default)
        {
            if (group.Id == Guid.Empty)
            {
                return BadRequest();
            }

            var oldGroup = await _userGroupsService.GetAsync(group.Id, ct).ConfigureAwait(false);
            if (oldGroup == null)
            {
                return NotFound();
            }

            return await ActionIfAllowed(() => _userGroupsService.UpdateAsync(_userContext.UserId, oldGroup, group, ct),
                new[] {oldGroup.AccountId});
        }

        [HttpPost("Delete")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.TechnicalSupport,
            Permission.AccountOwning)]
        public async Task<ActionResult> Delete([FromBody] List<Guid> ids, CancellationToken ct = default)
        {
            if (ids == null || ids.All(x => x == Guid.Empty))
            {
                return BadRequest();
            }

            var attributes = await _userGroupsService.GetListAsync(ids, ct).ConfigureAwait(false);

            return await ActionIfAllowed(
                () => _userGroupsService.DeleteAsync(_userContext.UserId, attributes.Select(x => x.Id), ct),
                attributes.Select(x => x.AccountId));
        }

        [HttpPost("Restore")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.TechnicalSupport,
            Permission.AccountOwning)]
        public async Task<ActionResult> Restore([FromBody] List<Guid> ids, CancellationToken ct = default)
        {
            if (ids == null || ids.All(x => x == Guid.Empty))
            {
                return BadRequest();
            }

            var attributes = await _userGroupsService.GetListAsync(ids, ct).ConfigureAwait(false);

            return await ActionIfAllowed(
                () => _userGroupsService.RestoreAsync(_userContext.UserId, attributes.Select(x => x.Id), ct),
                attributes.Select(x => x.AccountId));
        }

        [NonAction]
        private ActionResult<TResult> ReturnIfAllowed<TResult>(TResult result, IEnumerable<Guid> accountIds)
        {
            if (_userContext.HasAny(Permission.System, Permission.Development, Permission.Administration,
                Permission.TechnicalSupport))
            {
                return result;
            }

            var accountIdsAsArray = accountIds.ToArray();

            if (_userContext.HasAny(Permission.AccountOwning) && _userContext.Belongs(accountIdsAsArray))
            {
                return result;
            }

            if (_userContext.HasAny(Permission.AccountOwning) && !_userContext.Belongs(accountIdsAsArray))
            {
                return Forbid();
            }

            throw new Exception();
        }

        [NonAction]
        private async Task<ActionResult> ActionIfAllowed(Func<Task> action, IEnumerable<Guid> accountIds)
        {
            if (_userContext.HasAny(Permission.System, Permission.Development, Permission.Administration,
                Permission.TechnicalSupport))
            {
                await action().ConfigureAwait(false);

                return NoContent();
            }

            var accountIdsAsArray = accountIds.ToArray();

            if (_userContext.HasAny(Permission.AccountOwning) && _userContext.Belongs(accountIdsAsArray))
            {
                await action().ConfigureAwait(false);

                return NoContent();
            }

            if (_userContext.HasAny(Permission.AccountOwning) && !_userContext.Belongs(accountIdsAsArray))
            {
                return Forbid();
            }

            throw new Exception();
        }
    }
}