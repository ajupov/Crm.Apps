using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Leads.Models;
using Crm.Apps.Areas.Leads.RequestParameters;
using Crm.Apps.Areas.Leads.Services;
using Crm.Common.UserContext;
using Crm.Common.UserContext.Attributes;
using Crm.Common.UserContext.BaseControllers;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Areas.Leads.Controllers
{
    [ApiController]
    [RequirePrivileged(Role.AccountOwning, Role.LeadsManagement)]
    [Route("Api/Leads/Sources")]
    public class LeadSourcesController : UserContextController
    {
        private readonly IUserContext _userContext;
        private readonly ILeadSourcesService _leadSourcesService;

        public LeadSourcesController(IUserContext userContext, ILeadSourcesService leadSourcesService)
            : base(userContext)
        {
            _userContext = userContext;
            _leadSourcesService = leadSourcesService;
        }

        [HttpGet("Get")]
        public async Task<ActionResult<LeadSource>> Get([Required] Guid id, CancellationToken ct = default)
        {
            var source = await _leadSourcesService.GetAsync(id, ct);
            if (source == null)
            {
                return NotFound(id);
            }

            return ReturnIfAllowed(source, new[] {Role.AccountOwning, Role.LeadsManagement}, source.AccountId);
        }

        [HttpPost("GetList")]
        public async Task<ActionResult<List<LeadSource>>> GetList([Required] List<Guid> ids,
            CancellationToken ct = default)
        {
            var sources = await _leadSourcesService.GetListAsync(ids, ct);

            return ReturnIfAllowed(
                sources,
                new[] {Role.AccountOwning, Role.LeadsManagement},
                sources.Select(x => x.AccountId));
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<List<LeadSource>>> GetPagedList(
            LeadSourceGetPagedListRequestParameter request,
            CancellationToken ct = default)
        {
            request.AccountId = _userContext.AccountId;

            var sources = await _leadSourcesService.GetPagedListAsync(request, ct);

            return ReturnIfAllowed(
                sources,
                new[] {Role.AccountOwning, Role.LeadsManagement},
                sources.Select(x => x.AccountId));
        }

        [HttpPost("Create")]
        public async Task<ActionResult<Guid>> Create(LeadSource source, CancellationToken ct = default)
        {
            source.AccountId = _userContext.AccountId;

            var id = await _leadSourcesService.CreateAsync(_userContext.UserId, source, ct);

            return Created(nameof(Get), id);
        }

        [HttpPost("Update")]
        public async Task<ActionResult> Update(LeadSource source, CancellationToken ct = default)
        {
            var oldSource = await _leadSourcesService.GetAsync(source.Id, ct);
            if (oldSource == null)
            {
                return NotFound(source.Id);
            }

            return await ActionIfAllowed(
                () => _leadSourcesService.UpdateAsync(_userContext.UserId, oldSource, source, ct),
                new[] {Role.AccountOwning, Role.LeadsManagement},
                source.AccountId, oldSource.AccountId);
        }

        [HttpPost("Delete")]
        public async Task<ActionResult> Delete([Required] List<Guid> ids, CancellationToken ct = default)
        {
            var sources = await _leadSourcesService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _leadSourcesService.DeleteAsync(_userContext.UserId, sources.Select(x => x.Id), ct),
                new[] {Role.AccountOwning, Role.LeadsManagement},
                sources.Select(x => x.AccountId));
        }

        [HttpPost("Restore")]
        public async Task<ActionResult> Restore([Required] List<Guid> ids, CancellationToken ct = default)
        {
            var sources = await _leadSourcesService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _leadSourcesService.RestoreAsync(_userContext.UserId, sources.Select(x => x.Id), ct),
                new[] {Role.AccountOwning, Role.LeadsManagement},
                sources.Select(x => x.AccountId));
        }
    }
}