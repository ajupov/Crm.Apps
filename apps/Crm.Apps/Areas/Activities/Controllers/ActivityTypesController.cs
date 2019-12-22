using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Activities.Models;
using Crm.Apps.Areas.Activities.RequestParameters;
using Crm.Apps.Areas.Activities.Services;
using Crm.Common.UserContext;
using Crm.Common.UserContext.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Areas.Activities.Controllers
{
    [ApiController]
    [Route("Api/Activities/Types")]
    public class ActivityTypesController : ControllerBase
    {
        private readonly IUserContext _userContext;
        private readonly IActivityTypesService _activityTypesService;

        public ActivityTypesController(IUserContext userContext, IActivityTypesService activityTypesService)
        {
            _userContext = userContext;
            _activityTypesService = activityTypesService;
        }

        [RequirePrivileged(Role.AccountOwning, Role.SalesManagement)]
        [HttpGet("Get")]
        public async Task<ActionResult<ActivityType>> Get([Required] Guid id, CancellationToken ct = default)
        {
            var type = await _activityTypesService.GetAsync(id, ct);
            if (type == null)
            {
                return NotFound(id);
            }

            return ReturnIfAllowed(type, new[] {type.AccountId});
        }

        [RequirePrivileged(Role.AccountOwning, Role.SalesManagement)]
        [HttpPost("GetList")]
        public async Task<ActionResult<ActivityType[]>> GetList([Required] IEnumerable<Guid> ids,
            CancellationToken ct = default)
        {
            var types = await _activityTypesService.GetListAsync(ids, ct);

            return ReturnIfAllowed(types, types.Select(x => x.AccountId));
        }

        [RequirePrivileged(Role.AccountOwning, Role.SalesManagement)]
        [HttpPost("GetPagedList")]
        public async Task<ActionResult<ActivityType[]>> GetPagedList(
            ActivityTypeGetPagedListRequest request,
            CancellationToken ct = default)
        {
            var types = await _activityTypesService.GetPagedListAsync(request, ct);

            return ReturnIfAllowed(types, types.Select(x => x.AccountId));
        }

        [RequirePrivileged(Role.AccountOwning, Role.SalesManagement)]
        [HttpPost("Create")]
        public async Task<ActionResult<Guid>> Create(ActivityTypeCreateRequest request, CancellationToken ct = default)
        {
            if (!_userContext.HasAny(RequirePrivilegedAttribute.PrivilegedRoles))
            {
                request.AccountId = _userContext.AccountId;
            }

            var id = await _activityTypesService.CreateAsync(_userContext.UserId, request, ct);

            return Created("Get", id);
        }

        [RequirePrivileged(Role.AccountOwning, Role.SalesManagement)]
        [HttpPost("Update")]
        public async Task<ActionResult> Update(ActivityTypeUpdateRequest request, CancellationToken ct = default)
        {
            var type = await _activityTypesService.GetAsync(request.Id, ct);
            if (type == null)
            {
                return NotFound(request.Id);
            }

            return await ActionIfAllowed(() => _activityTypesService.UpdateAsync(_userContext.UserId, type,
                request, ct), new[] {type.AccountId, type.AccountId});
        }

        [RequirePrivileged(Role.AccountOwning, Role.SalesManagement)]
        [HttpPost("Delete")]
        public async Task<ActionResult> Delete([Required] IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            var attributes = await _activityTypesService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _activityTypesService.DeleteAsync(_userContext.UserId, attributes.Select(x => x.Id), ct),
                attributes.Select(x => x.AccountId));
        }

        [RequirePrivileged(Role.AccountOwning, Role.SalesManagement)]
        [HttpPost("Restore")]
        public async Task<ActionResult> Restore([Required] IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            var attributes = await _activityTypesService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _activityTypesService.RestoreAsync(_userContext.UserId, attributes.Select(x => x.Id), ct),
                attributes.Select(x => x.AccountId));
        }

        [NonAction]
        private ActionResult<TResult> ReturnIfAllowed<TResult>(TResult result, IEnumerable<Guid> accountIds)
        {
            if (_userContext.HasAny(RequirePrivilegedAttribute.PrivilegedRoles))
            {
                return result;
            }

            var accountIdsAsArray = accountIds.ToArray();

            if (_userContext.HasAny(Role.AccountOwning, Role.SalesManagement) &&
                _userContext.Belongs(accountIdsAsArray))
            {
                return result;
            }

            if (_userContext.HasAny(Role.AccountOwning, Role.SalesManagement) &&
                !_userContext.Belongs(accountIdsAsArray))
            {
                return Forbid();
            }

            throw new Exception();
        }

        [NonAction]
        private async Task<ActionResult> ActionIfAllowed(Func<Task> action, IEnumerable<Guid> accountIds)
        {
            if (_userContext.HasAny(RequirePrivilegedAttribute.PrivilegedRoles))
            {
                await action();

                return NoContent();
            }

            var accountIdsAsArray = accountIds.ToArray();

            if (_userContext.HasAny(Role.AccountOwning, Role.SalesManagement) &&
                _userContext.Belongs(accountIdsAsArray))
            {
                await action();

                return NoContent();
            }

            if (_userContext.HasAny(Role.AccountOwning, Role.SalesManagement) &&
                !_userContext.Belongs(accountIdsAsArray))
            {
                return Forbid();
            }

            throw new Exception();
        }
    }
}