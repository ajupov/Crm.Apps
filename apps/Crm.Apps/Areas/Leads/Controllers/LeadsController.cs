using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Leads.Models;
using Crm.Apps.Areas.Leads.Parameters;
using Crm.Apps.Areas.Leads.Services;
using Crm.Common.UserContext;
using Crm.Common.UserContext.Attributes;
using Crm.Common.UserContext.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Areas.Leads.Controllers
{
    [ApiController]
    [RequirePrivileged(Role.AccountOwning, Role.LeadsManagement)]
    [Route("Api/Leads")]
    public class LeadsController : UserContextController
    {
        private readonly IUserContext _userContext;
        private readonly ILeadsService _leadsService;

        public LeadsController(IUserContext userContext, ILeadsService leadsService)
            : base(userContext)
        {
            _userContext = userContext;
            _leadsService = leadsService;
        }

        [HttpGet("Get")]
        public async Task<ActionResult<Lead>> Get([Required] Guid id, CancellationToken ct = default)
        {
            var lead = await _leadsService.GetAsync(id, ct);
            if (lead == null)
            {
                return NotFound(id);
            }

            return ReturnIfAllowed(lead, new[] {Role.AccountOwning, Role.LeadsManagement}, lead.AccountId);
        }

        [HttpPost("GetList")]
        public async Task<ActionResult<List<Lead>>> GetList([Required] List<Guid> ids, CancellationToken ct = default)
        {
            var leads = await _leadsService.GetListAsync(ids, ct);

            return ReturnIfAllowed(
                leads,
                new[] {Role.AccountOwning, Role.LeadsManagement},
                leads.Select(x => x.AccountId));
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<List<Lead>>> GetPagedList(
            LeadGetPagedListParameter parameter,
            CancellationToken ct = default)
        {
            parameter.AccountId = parameter.AccountId;

            var leads = await _leadsService.GetPagedListAsync(parameter, ct);

            return ReturnIfAllowed(
                leads,
                new[] {Role.AccountOwning, Role.LeadsManagement},
                leads.Select(x => x.AccountId));
        }

        [HttpPost("Create")]
        public async Task<ActionResult<Guid>> Create(Lead lead, CancellationToken ct = default)
        {
            lead.AccountId = _userContext.AccountId;

            var id = await _leadsService.CreateAsync(_userContext.UserId, lead, ct);

            return Created(nameof(Get), id);
        }

        [HttpPost("Update")]
        public async Task<ActionResult> Update(Lead lead, CancellationToken ct = default)
        {
            var oldLead = await _leadsService.GetAsync(lead.Id, ct);
            if (oldLead == null)
            {
                return NotFound(lead.Id);
            }

            return await ActionIfAllowed(
                () => _leadsService.UpdateAsync(_userContext.UserId, oldLead, lead, ct),
                new[] {Role.AccountOwning, Role.LeadsManagement},
                lead.AccountId, oldLead.AccountId);
        }

        [HttpPost("Delete")]
        public async Task<ActionResult> Delete([Required] List<Guid> ids, CancellationToken ct = default)
        {
            var leads = await _leadsService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _leadsService.DeleteAsync(_userContext.UserId, leads.Select(x => x.Id), ct),
                new[] {Role.AccountOwning, Role.LeadsManagement},
                leads.Select(x => x.AccountId));
        }

        [HttpPost("Restore")]
        public async Task<ActionResult> Restore([Required] List<Guid> ids, CancellationToken ct = default)
        {
            var leads = await _leadsService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _leadsService.RestoreAsync(_userContext.UserId, leads.Select(x => x.Id), ct),
                new[] {Role.AccountOwning, Role.LeadsManagement},
                leads.Select(x => x.AccountId));
        }
    }
}