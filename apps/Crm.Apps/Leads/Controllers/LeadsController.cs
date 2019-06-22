using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Leads.Models;
using Crm.Apps.Leads.Parameters;
using Crm.Apps.Leads.Services;
using Crm.Common.UserContext;
using Crm.Common.UserContext.Attributes;
using Crm.Utils.Guid;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Leads.Controllers
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
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.TechnicalSupport,
            Permission.AccountOwning, Permission.LeadsManagement)]
        public async Task<ActionResult<Lead>> Get(Guid id, CancellationToken ct = default)
        {
            if (id.IsEmpty())
            {
                return BadRequest();
            }

            var lead = await _leadsService.GetAsync(id, ct).ConfigureAwait(false);
            if (lead == null)
            {
                return NotFound();
            }

            return ReturnIfAllowed(lead, new[] {lead.AccountId});
        }

        [HttpPost("GetList")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.TechnicalSupport,
            Permission.AccountOwning, Permission.LeadsManagement)]
        public async Task<ActionResult<List<Lead>>> GetList(List<Guid> ids, CancellationToken ct = default)
        {
            if (ids == null || ids.All(x => x.IsEmpty()))
            {
                return BadRequest();
            }

            var leads = await _leadsService.GetListAsync(ids, ct).ConfigureAwait(false);

            return ReturnIfAllowed(leads, leads.Select(x => x.AccountId));
        }

        [HttpPost("GetPagedList")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.TechnicalSupport,
            Permission.AccountOwning, Permission.LeadsManagement)]
        public async Task<ActionResult<List<Lead>>> GetPagedList(LeadGetPagedListParameter parameter,
            CancellationToken ct = default)
        {
            var leads = await _leadsService.GetPagedListAsync(parameter, ct).ConfigureAwait(false);

            return ReturnIfAllowed(leads, leads.Select(x => x.AccountId));
        }

        [HttpPost("Create")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.AccountOwning,
            Permission.LeadsManagement)]
        public async Task<ActionResult<Guid>> Create(Lead lead, CancellationToken ct = default)
        {
            if (lead == null)
            {
                return BadRequest();
            }

            if (!_userContext.HasAny(Permission.System, Permission.Development, Permission.Administration,
                Permission.TechnicalSupport))
            {
                lead.AccountId = _userContext.AccountId;
            }

            var id = await _leadsService.CreateAsync(_userContext.UserId, lead, ct).ConfigureAwait(false);

            return Created(nameof(Get), id);
        }

        [HttpPost("Update")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.TechnicalSupport,
            Permission.AccountOwning, Permission.LeadsManagement)]
        public async Task<ActionResult> Update(Lead lead, CancellationToken ct = default)
        {
            if (lead.Id.IsEmpty())
            {
                return BadRequest();
            }

            var oldLead = await _leadsService.GetAsync(lead.Id, ct).ConfigureAwait(false);
            if (oldLead == null)
            {
                return NotFound();
            }

            return await ActionIfAllowed(
                () => _leadsService.UpdateAsync(_userContext.UserId, oldLead, lead, ct),
                new[] {lead.AccountId, oldLead.AccountId}).ConfigureAwait(false);
        }

        [HttpPost("Delete")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.TechnicalSupport,
            Permission.AccountOwning, Permission.LeadsManagement)]
        public async Task<ActionResult> Delete(List<Guid> ids, CancellationToken ct = default)
        {
            if (ids == null || ids.All(x => x.IsEmpty()))
            {
                return BadRequest();
            }

            var leads = await _leadsService.GetListAsync(ids, ct).ConfigureAwait(false);

            return await ActionIfAllowed(
                () => _leadsService.DeleteAsync(_userContext.UserId, leads.Select(x => x.Id), ct),
                leads.Select(x => x.AccountId)).ConfigureAwait(false);
        }

        [HttpPost("Restore")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.TechnicalSupport,
            Permission.AccountOwning, Permission.LeadsManagement)]
        public async Task<ActionResult> Restore(List<Guid> ids, CancellationToken ct = default)
        {
            if (ids == null || ids.All(x => x.IsEmpty()))
            {
                return BadRequest();
            }

            var leads = await _leadsService.GetListAsync(ids, ct).ConfigureAwait(false);

            return await ActionIfAllowed(
                () => _leadsService.RestoreAsync(_userContext.UserId, leads.Select(x => x.Id), ct),
                leads.Select(x => x.AccountId)).ConfigureAwait(false);
        }

        [NonAction]
        private ActionResult<TResult> ReturnIfAllowed<TResult>(TResult result, IEnumerable<Guid> accountIds)
        {
            if (_userContext.HasAny(Permission.System, Permission.Development, Permission.Administration,
                Permission.TechnicalSupport))
            {
                return result;
            }

            var accountIdsAsArray = accountIds.ToArray();

            if (_userContext.HasAny(Permission.AccountOwning, Permission.LeadsManagement) &&
                _userContext.Belongs(accountIdsAsArray))
            {
                return result;
            }

            if (_userContext.HasAny(Permission.AccountOwning, Permission.LeadsManagement) &&
                !_userContext.Belongs(accountIdsAsArray))
            {
                return Forbid();
            }

            throw new Exception();
        }

        [NonAction]
        private async Task<ActionResult> ActionIfAllowed(Func<Task> action, IEnumerable<Guid> accountIds)
        {
            if (_userContext.HasAny(Permission.System, Permission.Development, Permission.Administration,
                Permission.TechnicalSupport))
            {
                await action().ConfigureAwait(false);

                return NoContent();
            }

            var accountIdsAsArray = accountIds.ToArray();

            if (_userContext.HasAny(Permission.AccountOwning, Permission.LeadsManagement) &&
                _userContext.Belongs(accountIdsAsArray))
            {
                await action().ConfigureAwait(false);

                return NoContent();
            }

            if (_userContext.HasAny(Permission.AccountOwning, Permission.LeadsManagement) &&
                !_userContext.Belongs(accountIdsAsArray))
            {
                return Forbid();
            }

            throw new Exception();
        }
    }
}