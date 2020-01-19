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
    [Route("Api/Deals/Changes")]
    public class DealChangesController : AllowingCheckControllerBase
    {
        private readonly IDealsService _dealsService;
        private readonly IDealChangesService _dealChangesService;

        public DealChangesController(
            IUserContext userContext,
            IDealsService dealsService,
            IDealChangesService dealChangesService)
            : base(userContext)
        {
            _dealChangesService = dealChangesService;
            _dealsService = dealsService;
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<List<DealChange>>> GetPagedList(
            DealChangeGetPagedListRequestParameter request,
            CancellationToken ct = default)
        {
            var deal = await _dealsService.GetAsync(request.DealId, ct);
            var changes = await _dealChangesService.GetPagedListAsync(request, ct);

            return ReturnIfAllowed(changes, Roles.Sales, deal.AccountId);
        }
    }
}