using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Leads.Models;
using Crm.Apps.Areas.Leads.Parameters;
using Crm.Apps.Areas.Leads.Services;
using Crm.Common.UserContext;
using Crm.Common.UserContext.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Areas.Leads.Controllers
{
    [ApiController]
    [Route("Api/Leads")]
    public class LeadsController : ControllerBase
    {
        private readonly IUserContext _userContext;
        private readonly ILeadsService _leadsService;

        public LeadsController(IUserContext userContext, ILeadsService leadsService)
        {
            _userContext = userContext;
            _leadsService = leadsService;
        }

        [HttpGet("Get")]
        [RequireAny(Role.System, Role.Development, Role.Administration, Role.TechnicalSupport,
            Role.AccountOwning, Role.LeadsManagement)]
        public async Task<ActionResult<Lead>> Get(Guid id, CancellationToken ct = default)
        {
            if (id.IsEmpty())
            {
                return BadRequest();
            }

            var lead = await _leadsService.GetAsync(id, ct);
            if (lead == null)
            {
                return NotFound();
            }

            return ReturnIfAllowed(lead, new[] {lead.AccountId});
        }

        [HttpPost("GetList")]
        [RequireAny(Role.System, Role.Development, Role.Administration, Role.TechnicalSupport,
            Role.AccountOwning, Role.LeadsManagement)]
        public async Task<ActionResult<List<Lead>>> GetList(List<Guid> ids, CancellationToken ct = default)
        {
            if (ids == null || ids.All(x => x.IsEmpty()))
            {
                return BadRequest();
            }

            var leads = await _leadsService.GetListAsync(ids, ct);

            return ReturnIfAllowed(leads, leads.Select(x => x.AccountId));
        }

        [HttpPost("GetPagedList")]
        [RequireAny(Role.System, Role.Development, Role.Administration, Role.TechnicalSupport,
            Role.AccountOwning, Role.LeadsManagement)]
        public async Task<ActionResult<List<Lead>>> GetPagedList(LeadGetPagedListParameter parameter,
            CancellationToken ct = default)
        {
            var leads = await _leadsService.GetPagedListAsync(parameter, ct);

            return ReturnIfAllowed(leads, leads.Select(x => x.AccountId));
        }

        [HttpPost("Create")]
        [RequireAny(Role.System, Role.Development, Role.Administration, Role.AccountOwning,
            Role.LeadsManagement)]
        public async Task<ActionResult<Guid>> Create(Lead lead, CancellationToken ct = default)
        {
            if (lead == null)
            {
                return BadRequest();
            }

            if (!_userContext.HasAny(Role.System, Role.Development, Role.Administration,
                Role.TechnicalSupport))
            {
                lead.AccountId = _userContext.AccountId;
            }

            var id = await _leadsService.CreateAsync(_userContext.UserId, lead, ct);

            return Created(nameof(Get), id);
        }

        [HttpPost("Update")]
        [RequireAny(Role.System, Role.Development, Role.Administration, Role.TechnicalSupport,
            Role.AccountOwning, Role.LeadsManagement)]
        public async Task<ActionResult> Update(Lead lead, CancellationToken ct = default)
        {
            if (lead.Id.IsEmpty())
            {
                return BadRequest();
            }

            var oldLead = await _leadsService.GetAsync(lead.Id, ct);
            if (oldLead == null)
            {
                return NotFound();
            }

            return await ActionIfAllowed(
                () => _leadsService.UpdateAsync(_userContext.UserId, oldLead, lead, ct),
                new[] {lead.AccountId, oldLead.AccountId});
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

            var leads = await _leadsService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _leadsService.DeleteAsync(_userContext.UserId, leads.Select(x => x.Id), ct),
                leads.Select(x => x.AccountId));
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

            var leads = await _leadsService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _leadsService.RestoreAsync(_userContext.UserId, leads.Select(x => x.Id), ct),
                leads.Select(x => x.AccountId));
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

            if (_userContext.HasAny(Role.AccountOwning, Role.LeadsManagement) &&
                _userContext.Belongs(accountIdsAsArray))
            {
                return result;
            }

            if (_userContext.HasAny(Role.AccountOwning, Role.LeadsManagement) &&
                !_userContext.Belongs(accountIdsAsArray))
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