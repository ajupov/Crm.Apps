using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Leads.Models;
using Crm.Apps.Leads.RequestParameters;
using Crm.Apps.Leads.Services;
using Crm.Common.All.UserContext;
using Crm.Common.All.UserContext.Attributes;
using Crm.Common.All.UserContext.BaseControllers;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Leads.Controllers
{
    [ApiController]
    [RequirePrivileged(Role.AccountOwning, Role.LeadsManagement)]
    [Route("Api/Leads/Attributes/Changes")]
    public class LeadAttributeChangesController : AllowingCheckControllerBase
    {
        private readonly ILeadAttributesService _leadAttributesService;
        private readonly ILeadAttributeChangesService _leadAttributeChangesService;

        public LeadAttributeChangesController(
            IUserContext userContext,
            ILeadAttributeChangesService leadAttributeChangesService,
            ILeadAttributesService leadAttributesService)
            : base(userContext)
        {
            _leadAttributeChangesService = leadAttributeChangesService;
            _leadAttributesService = leadAttributesService;
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<List<LeadAttributeChange>>> GetPagedList(
            LeadAttributeChangeGetPagedListRequestParameter request,
            CancellationToken ct = default)
        {
            var attribute = await _leadAttributesService.GetAsync(request.AttributeId, ct);
            var changes = await _leadAttributeChangesService.GetPagedListAsync(request, ct);

            return ReturnIfAllowed(changes, new[] {Role.AccountOwning, Role.LeadsManagement}, attribute.AccountId);
        }
    }
}