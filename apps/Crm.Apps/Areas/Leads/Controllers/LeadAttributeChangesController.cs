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
    [Route("Api/Leads/Attributes/Changes")]
    public class LeadAttributeChangesController : UserContextController
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
            LeadAttributeChangeGetPagedListParameter parameter,
            CancellationToken ct = default)
        {
            var attribute = await _leadAttributesService.GetAsync(parameter.AttributeId, ct);
            var changes = await _leadAttributeChangesService.GetPagedListAsync(parameter, ct);

            return ReturnIfAllowed(changes, new[] {Role.AccountOwning, Role.LeadsManagement}, attribute.AccountId);
        }
    }
}