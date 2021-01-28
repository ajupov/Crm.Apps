using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Infrastructure.All.Jwt;
using Ajupov.Infrastructure.All.Mvc.Attributes;
using Crm.Apps.Leads.Models;
using Crm.Apps.Leads.Services;
using Crm.Apps.Leads.V1.Requests;
using Crm.Apps.Leads.V1.Responses;
using Crm.Common.All.BaseControllers;
using Crm.Common.All.Roles;
using Crm.Common.All.Roles.Attributes;
using Crm.Common.All.UserContext;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Leads.V1.Controllers
{
    [ApiController]
    [RequestContentTypeApplicationJson]
    [ResponseContentTypeApplicationJson]
    [RequireLeadsRole(JwtDefaults.AuthenticationScheme)]
    [Route("Leads/v1")]
    public class LeadsController : AllowingCheckControllerBase
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
            var lead = await _leadsService.GetAsync(id, false, ct);
            if (lead == null)
            {
                return NotFound(id);
            }

            return ReturnIfAllowed(lead, Roles.Leads, lead.AccountId);
        }

        [HttpPost("GetList")]
        public async Task<ActionResult<List<Lead>>> GetList([Required] List<Guid> ids, CancellationToken ct = default)
        {
            var leads = await _leadsService.GetListAsync(ids, ct);

            return ReturnIfAllowed(
                leads,
                Roles.Leads,
                leads.Select(x => x.AccountId));
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<LeadGetPagedListResponse>> GetPagedList(
            LeadGetPagedListRequest request,
            CancellationToken ct = default)
        {
            var response = await _leadsService.GetPagedListAsync(_userContext.AccountId, request, ct);

            return ReturnIfAllowed(
                response,
                Roles.Leads,
                response.Leads.Select(x => x.AccountId));
        }

        [HttpPut("Create")]
        public async Task<ActionResult<Guid>> Create(Lead lead, CancellationToken ct = default)
        {
            lead.AccountId = _userContext.AccountId;

            var id = await _leadsService.CreateAsync(_userContext.UserId, lead, ct);

            return Created(nameof(Get), id);
        }

        [HttpPatch("Update")]
        public async Task<ActionResult> Update(Lead lead, CancellationToken ct = default)
        {
            var oldLead = await _leadsService.GetAsync(lead.Id, true, ct);
            if (oldLead == null)
            {
                return NotFound(lead.Id);
            }

            return await ActionIfAllowed(
                () => _leadsService.UpdateAsync(_userContext.UserId, oldLead, lead, ct),
                Roles.Leads,
                oldLead.AccountId);
        }

        [HttpPatch("Delete")]
        public async Task<ActionResult> Delete([Required] List<Guid> ids, CancellationToken ct = default)
        {
            var leads = await _leadsService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _leadsService.DeleteAsync(_userContext.UserId, leads.Select(x => x.Id), ct),
                Roles.Leads,
                leads.Select(x => x.AccountId));
        }

        [HttpPatch("Restore")]
        public async Task<ActionResult> Restore([Required] List<Guid> ids, CancellationToken ct = default)
        {
            var leads = await _leadsService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _leadsService.RestoreAsync(_userContext.UserId, leads.Select(x => x.Id), ct),
                Roles.Leads,
                leads.Select(x => x.AccountId));
        }
    }
}
