using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Users.Models;
using Crm.Apps.Areas.Users.RequestParameters;
using Crm.Apps.Areas.Users.Services;
using Crm.Apps.Infrastructure;
using Crm.Common.UserContext;
using Crm.Common.UserContext.Attributes;
using Crm.Common.UserContext.BaseControllers;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Areas.Users.Controllers
{
    [ApiController]
    [RequirePrivileged]
    [IgnoreApiDocumentation]
    [Route("Api/Users/Groups")]
    public class UserGroupsController : AllowingCheckControllerBase
    {
        private readonly IUserContext _userContext;
        private readonly IUserGroupsService _userGroupsService;

        public UserGroupsController(IUserContext userContext, IUserGroupsService userGroupsService)
            : base(userContext)
        {
            _userContext = userContext;
            _userGroupsService = userGroupsService;
        }

        [HttpGet("Get")]
        public async Task<ActionResult<UserGroup>> Get([Required] Guid id, CancellationToken ct = default)
        {
            var group = await _userGroupsService.GetAsync(id, ct);
            if (group == null)
            {
                return NotFound(id);
            }

            return ReturnIfAllowed(group, Role.AccountOwning, group.AccountId);
        }

        [HttpPost("GetList")]
        public async Task<ActionResult<List<UserGroup>>> GetList(
            [Required] List<Guid> ids,
            CancellationToken ct = default)
        {
            var groups = await _userGroupsService.GetListAsync(ids, ct);

            return ReturnIfAllowed(groups, Role.AccountOwning, groups.Select(x => x.AccountId));
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<List<UserGroup>>> GetPagedList(
            UserGroupGetPagedListRequestParameter request,
            CancellationToken ct = default)
        {
            request.AccountId = _userContext.AccountId;

            var groups = await _userGroupsService.GetPagedListAsync(request, ct);

            return ReturnIfAllowed(groups, Role.AccountOwning, groups.Select(x => x.AccountId));
        }

        [HttpPost("Create")]
        public async Task<ActionResult<Guid>> Create(UserGroup group, CancellationToken ct = default)
        {
            group.AccountId = _userContext.AccountId;

            var id = await _userGroupsService.CreateAsync(_userContext.UserId, group, ct);

            return Created(nameof(Get), id);
        }

        [HttpPost("Update")]
        public async Task<ActionResult> Update(UserGroup group, CancellationToken ct = default)
        {
            var oldGroup = await _userGroupsService.GetAsync(group.Id, ct);
            if (oldGroup == null)
            {
                return NotFound(group.Id);
            }

            return await ActionIfAllowed(
                () => _userGroupsService.UpdateAsync(_userContext.UserId, oldGroup, group, ct),
                Role.AccountOwning,
                group.AccountId, oldGroup.AccountId);
        }

        [HttpPost("Delete")]
        public async Task<ActionResult> Delete([Required] List<Guid> ids, CancellationToken ct = default)
        {
            var attributes = await _userGroupsService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _userGroupsService.DeleteAsync(_userContext.UserId, attributes.Select(x => x.Id), ct),
                Role.AccountOwning,
                attributes.Select(x => x.AccountId));
        }

        [HttpPost("Restore")]
        public async Task<ActionResult> Restore([Required] List<Guid> ids, CancellationToken ct = default)
        {
            var attributes = await _userGroupsService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _userGroupsService.RestoreAsync(_userContext.UserId, attributes.Select(x => x.Id), ct),
                Role.AccountOwning,
                attributes.Select(x => x.AccountId));
        }
    }
}