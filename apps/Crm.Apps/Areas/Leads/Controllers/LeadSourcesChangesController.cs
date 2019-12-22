using System.Collections.Generic;
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
    [Route("Api/Leads/Sources/Changes")]
    public class LeadSourcesChangesController : UserContextController
    {
        private readonly ILeadSourcesService _leadSourcesService;
        private readonly ILeadSourceChangesService _leadSourceChangesService;

        public LeadSourcesChangesController(
            IUserContext userContext,
            ILeadSourcesService leadSourcesService,
            ILeadSourceChangesService leadSourceChangesService)
            : base(userContext)
        {
            _leadSourcesService = leadSourcesService;
            _leadSourceChangesService = leadSourceChangesService;
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<List<LeadSourceChange>>> GetPagedList(
            LeadSourceChangeGetPagedListParameter parameter,
            CancellationToken ct = default)
        {
            var source = await _leadSourcesService.GetAsync(parameter.SourceId, ct);
            var changes = await _leadSourceChangesService.GetPagedListAsync(parameter, ct);

            return ReturnIfAllowed(changes, new[] {Role.AccountOwning, Role.LeadsManagement}, source.AccountId);
        }
    }
}