using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Infrastructure.All.Jwt;
using Crm.Apps.Leads.Models;
using Crm.Apps.Leads.RequestParameters;
using Crm.Apps.Leads.Services;
using Crm.Common.All.BaseControllers;
using Crm.Common.All.Roles;
using Crm.Common.All.Roles.Attributes;
using Crm.Common.All.UserContext;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Leads.Controllers
{
    [ApiController]
    [RequireLeadsRole(JwtDefaults.AuthenticationScheme)]
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

            return ReturnIfAllowed(changes, Roles.Leads, attribute.AccountId);
        }
    }
}