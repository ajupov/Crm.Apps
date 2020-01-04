﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Utils.All.Enums;
using Crm.Apps.Activities.Models;
using Crm.Apps.Activities.RequestParameters;
using Crm.Apps.Activities.Services;
using Crm.Common.All.UserContext;
using Crm.Common.All.UserContext.Attributes;
using Crm.Common.All.UserContext.BaseControllers;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Activities.Controllers
{
    [ApiController]
    [RequirePrivileged(Role.AccountOwning, Role.SalesManagement)]
    [Route("Api/Activities")]
    public class ActivitiesController : AllowingCheckControllerBase
    {
        private readonly IUserContext _userContext;
        private readonly IActivitiesService _activitiesService;

        public ActivitiesController(IUserContext userContext, IActivitiesService activitiesService)
            : base(userContext)
        {
            _userContext = userContext;
            _activitiesService = activitiesService;
        }

        [HttpGet("GetTypes")]
        public Dictionary<string, ActivityType> GetTypes()
        {
            return EnumsExtensions.GetAsDictionary<ActivityType>();
        }

        [HttpGet("Get")]
        public async Task<ActionResult<Activity>> Get([Required] Guid id, CancellationToken ct = default)
        {
            var activity = await _activitiesService.GetAsync(id, ct);
            if (activity == null)
            {
                return NotFound(id);
            }

            return ReturnIfAllowed(activity, new[] {Role.AccountOwning, Role.SalesManagement}, activity.AccountId);
        }

        [HttpPost("GetList")]
        public async Task<ActionResult<Activity[]>> GetList(
            [Required] IEnumerable<Guid> ids,
            CancellationToken ct = default)
        {
            var activities = await _activitiesService.GetListAsync(ids, ct);

            return ReturnIfAllowed(
                activities,
                new[] {Role.AccountOwning, Role.SalesManagement},
                activities.Select(x => x.AccountId));
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<Activity[]>> GetPagedList(
            ActivityGetPagedListRequestParameter request,
            CancellationToken ct = default)
        {
            request.AccountId = _userContext.AccountId;

            var activities = await _activitiesService.GetPagedListAsync(request, ct);

            return ReturnIfAllowed(
                activities,
                new[] {Role.AccountOwning, Role.SalesManagement},
                activities.Select(x => x.AccountId));
        }

        [HttpPost("Create")]
        public async Task<ActionResult<Guid>> Create(Activity activity, CancellationToken ct = default)
        {
            activity.AccountId = _userContext.AccountId;

            var id = await _activitiesService.CreateAsync(_userContext.UserId, activity, ct);

            return Created("Get", id);
        }

        [HttpPost("Update")]
        public async Task<ActionResult> Update(Activity activity, CancellationToken ct = default)
        {
            var oldActivity = await _activitiesService.GetAsync(activity.Id, ct);
            if (oldActivity == null)
            {
                return NotFound(activity.Id);
            }

            return await ActionIfAllowed(
                () => _activitiesService.UpdateAsync(_userContext.UserId, oldActivity, activity, ct),
                new[] {Role.AccountOwning, Role.SalesManagement},
                activity.AccountId, oldActivity.AccountId);
        }

        [HttpPost("Delete")]
        public async Task<ActionResult> Delete([Required] IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            var activities = await _activitiesService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _activitiesService.DeleteAsync(_userContext.UserId, activities.Select(x => x.Id), ct),
                new[] {Role.AccountOwning, Role.SalesManagement},
                activities.Select(x => x.AccountId));
        }

        [HttpPost("Restore")]
        public async Task<ActionResult> Restore([Required] IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            var activities = await _activitiesService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _activitiesService.RestoreAsync(_userContext.UserId, activities.Select(x => x.Id), ct),
                new[] {Role.AccountOwning, Role.SalesManagement},
                activities.Select(x => x.AccountId));
        }
    }
}