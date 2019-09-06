using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Activities.Models;
using Crm.Apps.Activities.RequestParameters;
using Crm.Apps.Activities.Services;
using Crm.Common.Types;
using Crm.Common.UserContext;
using Crm.Common.UserContext.Attributes;
using Crm.Utils.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Activities.Controllers
{
    [ApiController]
    [Route("Api/Activities/Attributes")]
    public class ActivityAttributesController : ControllerBase
    {
        private readonly IUserContext _userContext;
        private readonly IActivityAttributesService _activityAttributesService;

        public ActivityAttributesController(IUserContext userContext,
            IActivityAttributesService activityAttributesService)
        {
            _userContext = userContext;
            _activityAttributesService = activityAttributesService;
        }

        [RequirePrivileged(Permission.AccountOwning, Permission.SalesManagement)]
        [HttpGet("GetTypes")]
        public Dictionary<string, AttributeType> GetTypes()
        {
            return EnumsExtensions.GetAsDictionary<AttributeType>();
        }

        [RequirePrivileged(Permission.AccountOwning, Permission.SalesManagement)]
        [HttpGet("Get")]
        public async Task<ActionResult<ActivityAttribute>> Get([Required] Guid id, CancellationToken ct = default)
        {
            var attribute = await _activityAttributesService.GetAsync(id, ct);
            if (attribute == null)
            {
                return NotFound(id);
            }

            return ReturnIfAllowed(attribute, new[] {attribute.AccountId});
        }

        [RequirePrivileged(Permission.AccountOwning, Permission.SalesManagement)]
        [HttpPost("GetList")]
        public async Task<ActionResult<ActivityAttribute[]>> GetList(
            [Required] IEnumerable<Guid> ids,
            CancellationToken ct = default)
        {
            var attributes = await _activityAttributesService.GetListAsync(ids, ct);

            return ReturnIfAllowed(attributes, attributes.Select(x => x.AccountId));
        }

        [RequirePrivileged(Permission.AccountOwning, Permission.SalesManagement)]
        [HttpPost("GetPagedList")]
        public async Task<ActionResult<ActivityAttribute[]>> GetPagedList(
            ActivityAttributeGetPagedListRequest request,
            CancellationToken ct = default)
        {
            var attributes = await _activityAttributesService.GetPagedListAsync(request, ct);

            return ReturnIfAllowed(attributes, attributes.Select(x => x.AccountId));
        }

        [RequirePrivileged(Permission.AccountOwning, Permission.SalesManagement)]
        [HttpPost("Create")]
        public async Task<ActionResult<Guid>> Create(
            ActivityAttributeCreateRequest request,
            CancellationToken ct = default)
        {
            if (!_userContext.HasAny(RequirePrivilegedAttribute.PrivilegedPermissions))
            {
                request.AccountId = _userContext.AccountId;
            }

            var id = await _activityAttributesService.CreateAsync(_userContext.UserId, request, ct);

            return Created("Get", id);
        }

        [RequirePrivileged(Permission.AccountOwning, Permission.SalesManagement)]
        [HttpPost("Update")]
        public async Task<ActionResult> Update(ActivityAttributeUpdateRequest request, CancellationToken ct = default)
        {
            var attribute = await _activityAttributesService.GetAsync(request.Id, ct);
            if (attribute == null)
            {
                return NotFound(request.Id);
            }

            return await ActionIfAllowed(
                () => _activityAttributesService.UpdateAsync(_userContext.UserId, attribute, request, ct),
                new[] {request.AccountId, attribute.AccountId});
        }

        [RequirePrivileged(Permission.AccountOwning, Permission.SalesManagement)]
        [HttpPost("Delete")]
        public async Task<ActionResult> Delete([Required] IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            var attributes = await _activityAttributesService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _activityAttributesService.DeleteAsync(_userContext.UserId, attributes.Select(x => x.Id), ct),
                attributes.Select(x => x.AccountId));
        }

        [RequirePrivileged(Permission.AccountOwning, Permission.SalesManagement)]
        [HttpPost("Restore")]
        public async Task<ActionResult> Restore([Required] IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            var attributes = await _activityAttributesService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _activityAttributesService.RestoreAsync(_userContext.UserId, attributes.Select(x => x.Id), ct),
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