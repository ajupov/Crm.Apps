using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Infrastructure.All.Jwt;
using Crm.Apps.Deals.Services;
using Crm.Apps.Deals.v1.Models;
using Crm.Apps.Deals.v1.RequestParameters;
using Crm.Common.All.BaseControllers;
using Crm.Common.All.Roles;
using Crm.Common.All.Roles.Attributes;
using Crm.Common.All.UserContext;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Deals.v1.Controllers
{
    [ApiController]
    [RequireSalesRole(JwtDefaults.AuthenticationScheme)]
    [Route("v1/Deals/Types/Changes")]
    public class DealTypesChangesController : AllowingCheckControllerBase
    {
        private readonly IDealTypesService _dealTypesService;
        private readonly IDealTypeChangesService _dealTypeChangesService;

        public DealTypesChangesController(
            IUserContext userContext,
            IDealTypesService dealTypesService,
            IDealTypeChangesService dealTypeChangesService)
            : base(userContext)
        {
            _dealTypesService = dealTypesService;
            _dealTypeChangesService = dealTypeChangesService;
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<List<DealTypeChange>>> GetPagedList(
            DealTypeChangeGetPagedListRequestParameter request,
            CancellationToken ct = default)
        {
            var type = await _dealTypesService.GetAsync(request.TypeId, ct);
            var changes = await _dealTypeChangesService.GetPagedListAsync(request, ct);

            return ReturnIfAllowed(changes, Roles.Sales, type.AccountId);
        }
    }
}