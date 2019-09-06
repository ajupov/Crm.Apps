using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Activities.Models;
using Crm.Apps.Activities.RequestParameters;
using Crm.Apps.Activities.Services;
using Crm.Common.UserContext;
using Crm.Common.UserContext.Attributes;
using Crm.Utils.Guid;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Activities.Controllers
{
    [ApiController]
    [Route("Api/Activities/Statuses")]
    public class ActivityStatusesController : ControllerBase
    {
        private readonly IUserContext _userContext;
        private readonly IActivityStatusesService _activityStatusesService;

        public ActivityStatusesController(IUserContext userContext, IActivityStatusesService activityStatusesService)
        {
            _userContext = userContext;
            _activityStatusesService = activityStatusesService;
        }

        [RequirePrivileged(Permission.AccountOwning, Permission.SalesManagement)]
        [HttpGet("Get")]
        public async Task<ActionResult<ActivityStatus>> Get([Required] Guid id, CancellationToken ct = default)
        {
            var status = await _activityStatusesService.GetAsync(id, ct);
            if (status == null)
            {
                return NotFound(id);
            }

            return ReturnIfAllowed(status, new[] {status.AccountId});
        }

        [RequirePrivileged(Permission.AccountOwning, Permission.SalesManagement)]
        [HttpPost("GetList")]
        public async Task<ActionResult<ActivityStatus[]>> GetList(
            [Required] IEnumerable<Guid> ids,
            CancellationToken ct = default)
        {
            var statuses = await _activityStatusesService.GetListAsync(ids, ct);

            return ReturnIfAllowed(statuses, statuses.Select(x => x.AccountId));
        }

        [RequirePrivileged(Permission.AccountOwning, Permission.SalesManagement)]
        [HttpPost("GetPagedList")]
        public async Task<ActionResult<ActivityStatus[]>> GetPagedList(
            ActivityStatusGetPagedListRequest request,
            CancellationToken ct = default)
        {
            var statuses = await _activityStatusesService.GetPagedListAsync(request, ct);

            return ReturnIfAllowed(statuses, statuses.Select(x => x.AccountId));
        }

        [RequirePrivileged(Permission.AccountOwning, Permission.SalesManagement)]
        [HttpPost("Create")]
        public async Task<ActionResult<Guid>> Create(
            ActivityStatusCreateRequest request,
            CancellationToken ct = default)
        {
            if (!_userContext.HasAny(RequirePrivilegedAttribute.PrivilegedPermissions))
            {
                request.AccountId = _userContext.AccountId;
            }

            var id = await _activityStatusesService.CreateAsync(_userContext.UserId, request, ct);

            return Created("Get", id);
        }

        [RequirePrivileged(Permission.AccountOwning, Permission.SalesManagement)]
        [HttpPost("Update")]
        public async Task<ActionResult> Update(ActivityStatusUpdateRequest request, CancellationToken ct = default)
        {
            var status = await _activityStatusesService.GetAsync(request.Id, ct);
            if (status == null)
            {
                return NotFound(request.Id);
            }

            return await ActionIfAllowed(() => _activityStatusesService.UpdateAsync(_userContext.UserId, status,
                request, ct), new[] {request.AccountId, status.AccountId});
        }

        [RequirePrivileged(Permission.AccountOwning, Permission.SalesManagement)]
        [HttpPost("Delete")]
        public async Task<ActionResult> Delete([Required] IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            var attributes = await _activityStatusesService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _activityStatusesService.DeleteAsync(_userContext.UserId, attributes.Select(x => x.Id), ct),
                attributes.Select(x => x.AccountId));
        }

        [RequirePrivileged(Permission.AccountOwning, Permission.SalesManagement)]
        [HttpPost("Restore")]
        public async Task<ActionResult> Restore([Required] IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            var attributes = await _activityStatusesService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _activityStatusesService.RestoreAsync(_userContext.UserId, attributes.Select(x => x.Id), ct),
                attributes.Select(x => x.AccountId));
        }

        [NonAction]
        private ActionResult<TResult> ReturnIfAllowed<TResult>(TResult result, IEnumerable<Guid> accountIds)
        {
            if (_userContext.HasAny(RequirePrivilegedAttribute.PrivilegedPermissions))
            {
                return result;
            }

            var accountIdsAsArray = accountIds.ToArray();

            if (_userContext.HasAny(Permission.AccountOwning, Permission.SalesManagement) &&
                _userContext.Belongs(accountIdsAsArray))
            {
                return result;
            }

            if (_userContext.HasAny(Permission.AccountOwning, Permission.SalesManagement) &&
                !_userContext.Belongs(accountIdsAsArray))
            {
                return Forbid();
            }

            throw new Exception();
        }

        [NonAction]
        private async Task<ActionResult> ActionIfAllowed(Func<Task> action, IEnumerable<Guid> accountIds)
        {
            if (_userContext.HasAny(RequirePrivilegedAttribute.PrivilegedPermissions))
            {
                await action();

                return NoContent();
            }

            var accountIdsAsArray = accountIds.ToArray();

            if (_userContext.HasAny(Permission.AccountOwning, Permission.SalesManagement) &&
                _userContext.Belongs(accountIdsAsArray))
            {
                await action();

                return NoContent();
            }

            if (_userContext.HasAny(Permission.AccountOwning, Permission.SalesManagement) &&
                !_userContext.Belongs(accountIdsAsArray))
            {
                return Forbid();
            }

            throw new Exception();
        }
    }
}