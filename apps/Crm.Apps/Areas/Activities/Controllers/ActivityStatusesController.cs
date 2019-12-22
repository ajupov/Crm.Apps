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
using Crm.Common.UserContext.BaseControllers;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Areas.Activities.Controllers
{
    [ApiController]
    [RequirePrivileged(Role.AccountOwning, Role.SalesManagement)]
    [Route("Api/Activities/Statuses")]
    public class ActivityStatusesController : UserContextController
    {
        private readonly IUserContext _userContext;
        private readonly IActivityStatusesService _activityStatusesService;

        public ActivityStatusesController(IUserContext userContext, IActivityStatusesService activityStatusesService) :
            base(userContext)
        {
            _userContext = userContext;
            _activityStatusesService = activityStatusesService;
        }

        [HttpGet("Get")]
        public async Task<ActionResult<ActivityStatus>> Get([Required] Guid id, CancellationToken ct = default)
        {
            var status = await _activityStatusesService.GetAsync(id, ct);
            if (status == null)
            {
                return NotFound(id);
            }

            return ReturnIfAllowed(status, new[] {Role.AccountOwning, Role.SalesManagement}, status.AccountId);
        }

        [HttpPost("GetList")]
        public async Task<ActionResult<ActivityStatus[]>> GetList(
            [Required] IEnumerable<Guid> ids,
            CancellationToken ct = default)
        {
            var statuses = await _activityStatusesService.GetListAsync(ids, ct);

            return ReturnIfAllowed(
                statuses,
                new[] {Role.AccountOwning, Role.SalesManagement},
                statuses.Select(x => x.AccountId));
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<ActivityStatus[]>> GetPagedList(
            ActivityStatusGetPagedListRequestParameter request,
            CancellationToken ct = default)
        {
            request.AccountId = _userContext.AccountId;

            var statuses = await _activityStatusesService.GetPagedListAsync(request, ct);

            return ReturnIfAllowed(
                statuses,
                new[] {Role.AccountOwning, Role.SalesManagement},
                statuses.Select(x => x.AccountId));
        }

        [HttpPost("Create")]
        public async Task<ActionResult<Guid>> Create(ActivityStatus status, CancellationToken ct = default)
        {
            status.AccountId = _userContext.AccountId;

            var id = await _activityStatusesService.CreateAsync(_userContext.UserId, status, ct);

            return Created("Get", id);
        }

        [HttpPost("Update")]
        public async Task<ActionResult> Update(ActivityStatus status, CancellationToken ct = default)
        {
            var oldStatus = await _activityStatusesService.GetAsync(status.Id, ct);
            if (oldStatus == null)
            {
                return NotFound(status.Id);
            }

            return await ActionIfAllowed(
                () => _activityStatusesService.UpdateAsync(_userContext.UserId, oldStatus, status, ct),
                new[] {Role.AccountOwning, Role.SalesManagement},
                status.AccountId, oldStatus.AccountId);
        }

        [HttpPost("Delete")]
        public async Task<ActionResult> Delete([Required] IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            var attributes = await _activityStatusesService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _activityStatusesService.DeleteAsync(_userContext.UserId, attributes.Select(x => x.Id), ct),
                new[] {Role.AccountOwning, Role.SalesManagement},
                attributes.Select(x => x.AccountId));
        }

        [HttpPost("Restore")]
        public async Task<ActionResult> Restore([Required] IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            var attributes = await _activityStatusesService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _activityStatusesService.RestoreAsync(_userContext.UserId, attributes.Select(x => x.Id), ct),
                new[] {Role.AccountOwning, Role.SalesManagement},
                attributes.Select(x => x.AccountId));
        }
    }
}