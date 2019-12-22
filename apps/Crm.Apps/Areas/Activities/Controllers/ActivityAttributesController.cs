﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Activities.Models;
using Crm.Apps.Areas.Activities.RequestParameters;
using Crm.Apps.Areas.Activities.Services;
using Crm.Apps.Utils;
using Crm.Common.Types;
using Crm.Common.UserContext;
using Crm.Common.UserContext.Attributes;
using Crm.Common.UserContext.BaseControllers;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Areas.Activities.Controllers
{
    [ApiController]
    [RequirePrivileged(Role.AccountOwning, Role.SalesManagement)]
    [Route("Api/Activities/Attributes")]
    public class ActivityAttributesController : UserContextController
    {
        private readonly IUserContext _userContext;
        private readonly IActivityAttributesService _activityAttributesService;

        public ActivityAttributesController(IUserContext userContext,
            IActivityAttributesService activityAttributesService)
            : base(userContext)
        {
            _userContext = userContext;
            _activityAttributesService = activityAttributesService;
        }

        [HttpGet("GetTypes")]
        public Dictionary<string, AttributeType> GetTypes()
        {
            return EnumsExtensions.GetAsDictionary<AttributeType>();
        }

        [HttpGet("Get")]
        public async Task<ActionResult<ActivityAttribute>> Get([Required] Guid id, CancellationToken ct = default)
        {
            var attribute = await _activityAttributesService.GetAsync(id, ct);
            if (attribute == null)
            {
                return NotFound(id);
            }

            return ReturnIfAllowed(attribute, new[] {Role.AccountOwning, Role.SalesManagement}, attribute.AccountId);
        }

        [HttpPost("GetList")]
        public async Task<ActionResult<ActivityAttribute[]>> GetList(
            [Required] IEnumerable<Guid> ids,
            CancellationToken ct = default)
        {
            var attributes = await _activityAttributesService.GetListAsync(ids, ct);

            return ReturnIfAllowed(
                attributes,
                new[] {Role.AccountOwning, Role.SalesManagement},
                attributes.Select(x => x.AccountId));
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<ActivityAttribute[]>> GetPagedList(
            ActivityAttributeGetPagedListRequestParameter request,
            CancellationToken ct = default)
        {
            var attributes = await _activityAttributesService.GetPagedListAsync(request, ct);

            return ReturnIfAllowed(
                attributes,
                new[] {Role.AccountOwning, Role.SalesManagement},
                attributes.Select(x => x.AccountId));
        }

        [HttpPost("Create")]
        public async Task<ActionResult<Guid>> Create(ActivityAttribute attribute, CancellationToken ct = default)
        {
            attribute.AccountId = _userContext.AccountId;

            var id = await _activityAttributesService.CreateAsync(_userContext.UserId, attribute, ct);

            return Created("Get", id);
        }

        [HttpPost("Update")]
        public async Task<ActionResult> Update(ActivityAttribute attribute, CancellationToken ct = default)
        {
            var oldAttribute = await _activityAttributesService.GetAsync(attribute.Id, ct);
            if (oldAttribute == null)
            {
                return NotFound(attribute.Id);
            }

            return await ActionIfAllowed(
                () => _activityAttributesService.UpdateAsync(_userContext.UserId, oldAttribute, attribute, ct),
                new[] {Role.AccountOwning, Role.SalesManagement},
                attribute.AccountId, oldAttribute.AccountId);
        }

        [HttpPost("Delete")]
        public async Task<ActionResult> Delete([Required] IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            var attributes = await _activityAttributesService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _activityAttributesService.DeleteAsync(_userContext.UserId, attributes.Select(x => x.Id), ct),
                new[] {Role.AccountOwning, Role.SalesManagement},
                attributes.Select(x => x.AccountId));
        }

        [HttpPost("Restore")]
        public async Task<ActionResult> Restore([Required] IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            var attributes = await _activityAttributesService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _activityAttributesService.RestoreAsync(_userContext.UserId, attributes.Select(x => x.Id), ct),
                new[] {Role.AccountOwning, Role.SalesManagement},
                attributes.Select(x => x.AccountId));
        }
    }
}