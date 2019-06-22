using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Users.Models;
using Crm.Apps.Users.Parameters;
using Crm.Apps.Users.Services;
using Crm.Common.UserContext;
using Crm.Common.UserContext.Attributes;
using Crm.Utils.Guid;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Users.Controllers
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
        public async Task<ActionResult<UserGroup>> Get(Guid id, CancellationToken ct = default)
        {
            if (id.IsEmpty())
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

        [HttpPost("GetList")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.TechnicalSupport,
            Permission.AccountOwning)]
        public async Task<ActionResult<List<UserGroup>>> GetList(List<Guid> ids, CancellationToken ct = default)
        {
            if (ids == null || ids.All(x => x.IsEmpty()))
            {
                return BadRequest();
            }

            var groups = await _userGroupsService.GetListAsync(ids, ct).ConfigureAwait(false);

            return ReturnIfAllowed(groups, groups.Select(x => x.AccountId));
        }

        [HttpPost("GetPagedList")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.TechnicalSupport,
            Permission.AccountOwning)]
        public async Task<ActionResult<List<UserGroup>>> GetPagedList(UserGroupGetPagedListParameter parameter,
            CancellationToken ct = default)
        {
            var groups = await _userGroupsService.GetPagedListAsync(parameter, ct).ConfigureAwait(false);

            return ReturnIfAllowed(groups, groups.Select(x => x.AccountId));
        }

        [HttpPost("Create")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.AccountOwning)]
        public async Task<ActionResult<Guid>> Create(UserGroup group, CancellationToken ct = default)
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
        public async Task<ActionResult> Update(UserGroup group, CancellationToken ct = default)
        {
            if (group.Id.IsEmpty())
            {
                return BadRequest();
            }

            var oldGroup = await _userGroupsService.GetAsync(group.Id, ct).ConfigureAwait(false);
            if (oldGroup == null)
            {
                return NotFound();
            }

            return await ActionIfAllowed(
                () => _userGroupsService.UpdateAsync(_userContext.UserId, oldGroup, @group, ct),
                new[] {group.AccountId, oldGroup.AccountId}).ConfigureAwait(false);
        }

        [HttpPost("Delete")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.TechnicalSupport,
            Permission.AccountOwning)]
        public async Task<ActionResult> Delete(List<Guid> ids, CancellationToken ct = default)
        {
            if (ids == null || ids.All(x => x.IsEmpty()))
            {
                return BadRequest();
            }

            var attributes = await _userGroupsService.GetListAsync(ids, ct).ConfigureAwait(false);

            return await ActionIfAllowed(
                () => _userGroupsService.DeleteAsync(_userContext.UserId, attributes.Select(x => x.Id), ct),
                attributes.Select(x => x.AccountId)).ConfigureAwait(false);
        }

        [HttpPost("Restore")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.TechnicalSupport,
            Permission.AccountOwning)]
        public async Task<ActionResult> Restore(List<Guid> ids, CancellationToken ct = default)
        {
            if (ids == null || ids.All(x => x.IsEmpty()))
            {
                return BadRequest();
            }

            var attributes = await _userGroupsService.GetListAsync(ids, ct).ConfigureAwait(false);

            return await ActionIfAllowed(
                () => _userGroupsService.RestoreAsync(_userContext.UserId, attributes.Select(x => x.Id), ct),
                attributes.Select(x => x.AccountId)).ConfigureAwait(false);
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