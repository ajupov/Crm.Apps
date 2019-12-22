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
    [Route("Api/Activities")]
    public class ActivitiesController : ControllerBase
    {
        private readonly IUserContext _userContext;
        private readonly IActivitiesService _activitiesService;

        public ActivitiesController(IUserContext userContext, IActivitiesService activitiesService)
        {
            _userContext = userContext;
            _activitiesService = activitiesService;
        }

        [RequirePrivileged(Role.AccountOwning, Role.SalesManagement)]
        [HttpGet("GetTypes")]
        public Dictionary<string, ActivityType> GetTypes()
        {
            return EnumsExtensions.GetAsDictionary<ActivityType>();
        }

        [RequirePrivileged(Role.AccountOwning, Role.SalesManagement)]
        [HttpGet("Get")]
        public async Task<ActionResult<Activity>> Get([Required] Guid id, CancellationToken ct = default)
        {
            var activity = await _activitiesService.GetAsync(id, ct);
            if (activity == null)
            {
                return NotFound(id);
            }

            return ReturnIfAllowed(activity, new[] {activity.AccountId});
        }

        [RequirePrivileged(Role.AccountOwning, Role.SalesManagement)]
        [HttpPost("GetList")]
        public async Task<ActionResult<Activity[]>> GetList(IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            var activities = await _activitiesService.GetListAsync(ids, ct);

            return ReturnIfAllowed(activities, activities.Select(x => x.AccountId));
        }

        [RequirePrivileged(Role.AccountOwning, Role.SalesManagement)]
        [HttpPost("GetPagedList")]
        public async Task<ActionResult<Activity[]>> GetPagedList(
            ActivityGetPagedListRequest request,
            CancellationToken ct = default)
        {
            var activities = await _activitiesService.GetPagedListAsync(request, ct);

            return ReturnIfAllowed(activities, activities.Select(x => x.AccountId));
        }

        [RequirePrivileged(Role.AccountOwning, Role.SalesManagement)]
        [HttpPost("Create")]
        public async Task<ActionResult<Guid>> Create(ActivityCreateRequest request, CancellationToken ct = default)
        {
            if (!_userContext.HasAny(RequirePrivilegedAttribute.PrivilegedRoles))
            {
                request.AccountId = _userContext.AccountId;
            }

            var id = await _activitiesService.CreateAsync(_userContext.UserId, request, ct);

            return Created("Get", id);
        }

        [RequirePrivileged(Role.AccountOwning, Role.SalesManagement)]
        [HttpPost("Update")]
        public async Task<ActionResult> Update(ActivityUpdateRequest request, CancellationToken ct = default)
        {
            var activity = await _activitiesService.GetAsync(request.Id, ct);
            if (activity == null)
            {
                return NotFound(request.Id);
            }

            return await ActionIfAllowed(
                () => _activitiesService.UpdateAsync(_userContext.UserId, activity, request, ct),
                new[] {request.AccountId, activity.AccountId});
        }

        [RequirePrivileged(Role.AccountOwning, Role.SalesManagement)]
        [HttpPost("Delete")]
        public async Task<ActionResult> Delete([Required] IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            var activities = await _activitiesService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _activitiesService.DeleteAsync(_userContext.UserId, activities.Select(x => x.Id), ct),
                activities.Select(x => x.AccountId));
        }

        [RequirePrivileged(Role.AccountOwning, Role.SalesManagement)]
        [HttpPost("Restore")]
        public async Task<ActionResult> Restore([Required] IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            var activities = await _activitiesService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _activitiesService.RestoreAsync(_userContext.UserId, activities.Select(x => x.Id), ct),
                activities.Select(x => x.AccountId));
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