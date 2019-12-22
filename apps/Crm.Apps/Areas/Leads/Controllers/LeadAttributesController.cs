using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Leads.Models;
using Crm.Apps.Areas.Leads.Parameters;
using Crm.Apps.Areas.Leads.Services;
using Crm.Common.Types;
using Crm.Common.UserContext;
using Crm.Common.UserContext.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Areas.Leads.Controllers
{
    [ApiController]
    [Route("Api/Leads/Attributes")]
    public class LeadAttributesController : ControllerBase
    {
        private readonly IUserContext _userContext;
        private readonly ILeadAttributesService _leadAttributesService;

        public LeadAttributesController(IUserContext userContext, ILeadAttributesService leadAttributesService)
        {
            _userContext = userContext;
            _leadAttributesService = leadAttributesService;
        }

        [HttpGet("GetTypes")]
        [RequireAny(Role.System, Role.Development, Role.Administration, Role.TechnicalSupport)]
        public ActionResult<List<AttributeType>> GetTypes()
        {
            return EnumsExtensions.GetValues<AttributeType>().ToList();
        }

        [HttpGet("Get")]
        [RequireAny(Role.System, Role.Development, Role.Administration, Role.TechnicalSupport,
            Role.AccountOwning, Role.LeadsManagement)]
        public async Task<ActionResult<LeadAttribute>> Get(Guid id, CancellationToken ct = default)
        {
            if (id.IsEmpty())
            {
                return BadRequest();
            }

            var attribute = await _leadAttributesService.GetAsync(id, ct);
            if (attribute == null)
            {
                return NotFound();
            }

            return ReturnIfAllowed(attribute, new[] {attribute.AccountId});
        }

        [HttpPost("GetList")]
        [RequireAny(Role.System, Role.Development, Role.Administration, Role.TechnicalSupport,
            Role.AccountOwning, Role.LeadsManagement)]
        public async Task<ActionResult<List<LeadAttribute>>> GetList(List<Guid> ids, CancellationToken ct = default)
        {
            if (ids == null || ids.All(x => x.IsEmpty()))
            {
                return BadRequest();
            }

            var attributes = await _leadAttributesService.GetListAsync(ids, ct);

            return ReturnIfAllowed(attributes, attributes.Select(x => x.AccountId));
        }

        [HttpPost("GetPagedList")]
        [RequireAny(Role.System, Role.Development, Role.Administration, Role.TechnicalSupport,
            Role.AccountOwning, Role.LeadsManagement)]
        public async Task<ActionResult<List<LeadAttribute>>> GetPagedList(
            LeadAttributeGetPagedListParameter parameter,
            CancellationToken ct = default)
        {
            var attributes = await _leadAttributesService.GetPagedListAsync(parameter, ct);

            return ReturnIfAllowed(attributes, attributes.Select(x => x.AccountId));
        }

        [HttpPost("Create")]
        [RequireAny(Role.System, Role.Development, Role.Administration, Role.AccountOwning,
            Role.LeadsManagement)]
        public async Task<ActionResult<Guid>> Create(LeadAttribute attribute, CancellationToken ct = default)
        {
            if (attribute == null)
            {
                return BadRequest();
            }

            if (!_userContext.HasAny(Role.System, Role.Development, Role.Administration,
                Role.TechnicalSupport))
            {
                attribute.AccountId = _userContext.AccountId;
            }

            var id = await _leadAttributesService.CreateAsync(_userContext.UserId, attribute, ct)
                ;

            return Created(nameof(Get), id);
        }

        [HttpPost("Update")]
        [RequireAny(Role.System, Role.Development, Role.Administration, Role.TechnicalSupport,
            Role.LeadsManagement)]
        public async Task<ActionResult> Update(LeadAttribute attribute, CancellationToken ct = default)
        {
            if (attribute.Id.IsEmpty())
            {
                return BadRequest();
            }

            var oldAttribute = await _leadAttributesService.GetAsync(attribute.Id, ct);
            if (oldAttribute == null)
            {
                return NotFound();
            }

            return await ActionIfAllowed(
                () => _leadAttributesService.UpdateAsync(_userContext.UserId, oldAttribute, attribute, ct),
                new[] {attribute.AccountId, oldAttribute.AccountId});
        }

        [HttpPost("Delete")]
        [RequireAny(Role.System, Role.Development, Role.Administration, Role.TechnicalSupport,
            Role.AccountOwning, Role.LeadsManagement)]
        public async Task<ActionResult> Delete(List<Guid> ids, CancellationToken ct = default)
        {
            if (ids == null || ids.All(x => x.IsEmpty()))
            {
                return BadRequest();
            }

            var attributes = await _leadAttributesService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _leadAttributesService.DeleteAsync(_userContext.UserId, attributes.Select(x => x.Id), ct),
                attributes.Select(x => x.AccountId));
        }

        [HttpPost("Restore")]
        [RequireAny(Role.System, Role.Development, Role.Administration, Role.TechnicalSupport,
            Role.AccountOwning, Role.LeadsManagement)]
        public async Task<ActionResult> Restore(List<Guid> ids, CancellationToken ct = default)
        {
            if (ids == null || ids.All(x => x.IsEmpty()))
            {
                return BadRequest();
            }

            var attributes = await _leadAttributesService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _leadAttributesService.RestoreAsync(_userContext.UserId, attributes.Select(x => x.Id), ct),
                attributes.Select(x => x.AccountId));
        }

        [NonAction]
        private ActionResult<TResult> ReturnIfAllowed<TResult>(TResult result, IEnumerable<Guid> accountIds)
        {
            if (_userContext.HasAny(Role.System, Role.Development, Role.Administration,
                Role.TechnicalSupport))
            {
                return result;
            }

            var accountIdsAsArray = accountIds.ToArray();

            if (_userContext.HasAny(Role.AccountOwning) && _userContext.Belongs(accountIdsAsArray))
            {
                return result;
            }

            if (_userContext.HasAny(Role.AccountOwning) && !_userContext.Belongs(accountIdsAsArray))
            {
                return Forbid();
            }

            throw new Exception();
        }

        [NonAction]
        private async Task<ActionResult> ActionIfAllowed(Func<Task> action, IEnumerable<Guid> accountIds)
        {
            if (_userContext.HasAny(Role.System, Role.Development, Role.Administration,
                Role.TechnicalSupport))
            {
                await action();

                return NoContent();
            }

            var accountIdsAsArray = accountIds.ToArray();

            if (_userContext.HasAny(Role.AccountOwning, Role.LeadsManagement) &&
                _userContext.Belongs(accountIdsAsArray))
            {
                await action();

                return NoContent();
            }

            if (_userContext.HasAny(Role.AccountOwning, Role.LeadsManagement) &&
                !_userContext.Belongs(accountIdsAsArray))
            {
                return Forbid();
            }

            throw new Exception();
        }
    }
}