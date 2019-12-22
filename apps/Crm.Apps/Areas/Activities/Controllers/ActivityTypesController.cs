﻿using System;
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
    [Route("Api/Activities/Types")]
    public class ActivityTypesController : UserContextController
    {
        private readonly IUserContext _userContext;
        private readonly IActivityTypesService _activityTypesService;

        public ActivityTypesController(IUserContext userContext, IActivityTypesService activityTypesService)
            : base(userContext)
        {
            _userContext = userContext;
            _activityTypesService = activityTypesService;
        }

        [HttpGet("Get")]
        public async Task<ActionResult<ActivityType>> Get([Required] Guid id, CancellationToken ct = default)
        {
            var type = await _activityTypesService.GetAsync(id, ct);
            if (type == null)
            {
                return NotFound(id);
            }

            return ReturnIfAllowed(type, new[] {Role.AccountOwning, Role.SalesManagement}, type.AccountId);
        }

        [HttpPost("GetList")]
        public async Task<ActionResult<ActivityType[]>> GetList(
            [Required] IEnumerable<Guid> ids,
            CancellationToken ct = default)
        {
            var types = await _activityTypesService.GetListAsync(ids, ct);

            return ReturnIfAllowed(
                types,
                new[] {Role.AccountOwning, Role.SalesManagement},
                types.Select(x => x.AccountId));
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<ActivityType[]>> GetPagedList(
            ActivityTypeGetPagedListRequestParameter request,
            CancellationToken ct = default)
        {
            request.AccountId = _userContext.AccountId;

            var types = await _activityTypesService.GetPagedListAsync(request, ct);

            return ReturnIfAllowed(
                types,
                new[] {Role.AccountOwning, Role.SalesManagement},
                types.Select(x => x.AccountId));
        }

        [HttpPost("Create")]
        public async Task<ActionResult<Guid>> Create(ActivityType type, CancellationToken ct = default)
        {
            type.AccountId = _userContext.AccountId;

            var id = await _activityTypesService.CreateAsync(_userContext.UserId, type, ct);

            return Created("Get", id);
        }

        [HttpPost("Update")]
        public async Task<ActionResult> Update(ActivityType type, CancellationToken ct = default)
        {
            var oldType = await _activityTypesService.GetAsync(type.Id, ct);
            if (oldType == null)
            {
                return NotFound(type.Id);
            }

            return await ActionIfAllowed(
                () => _activityTypesService.UpdateAsync(_userContext.UserId, oldType, type, ct),
                new[] {Role.AccountOwning, Role.SalesManagement},
                oldType.AccountId, oldType.AccountId);
        }

        [HttpPost("Delete")]
        public async Task<ActionResult> Delete([Required] IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            var attributes = await _activityTypesService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _activityTypesService.DeleteAsync(_userContext.UserId, attributes.Select(x => x.Id), ct),
                new[] {Role.AccountOwning, Role.SalesManagement},
                attributes.Select(x => x.AccountId));
        }

        [HttpPost("Restore")]
        public async Task<ActionResult> Restore([Required] IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            var attributes = await _activityTypesService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _activityTypesService.RestoreAsync(_userContext.UserId, attributes.Select(x => x.Id), ct),
                new[] {Role.AccountOwning, Role.SalesManagement},
                attributes.Select(x => x.AccountId));
        }
    }
}