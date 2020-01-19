using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Infrastructure.All.Jwt;
using Crm.Apps.Deals.Models;
using Crm.Apps.Deals.RequestParameters;
using Crm.Apps.Deals.Services;
using Crm.Common.All.BaseControllers;
using Crm.Common.All.Roles;
using Crm.Common.All.Roles.Attributes;
using Crm.Common.All.UserContext;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Deals.Controllers
{
    [ApiController]
    [RequireSalesRole(JwtDefaults.AuthenticationScheme)]
    [Route("Api/Deals/Attributes/Changes")]
    public class DealAttributeChangesController : AllowingCheckControllerBase
    {
        private readonly IDealAttributesService _dealAttributesService;
        private readonly IDealAttributeChangesService _dealAttributeChangesService;

        public DealAttributeChangesController(
            IUserContext userContext,
            IDealAttributesService dealAttributesService,
            IDealAttributeChangesService dealAttributeChangesService)
            : base(userContext)
        {
            _dealAttributeChangesService = dealAttributeChangesService;
            _dealAttributesService = dealAttributesService;
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<List<DealAttributeChange>>> GetPagedList(
            DealAttributeChangeGetPagedListRequestParameter request,
            CancellationToken ct = default)
        {
            var attribute = await _dealAttributesService.GetAsync(request.AttributeId, ct);
            var changes = await _dealAttributeChangesService.GetPagedListAsync(request, ct);

            return ReturnIfAllowed(changes, Roles.Sales, attribute.AccountId);
        }
    }
}