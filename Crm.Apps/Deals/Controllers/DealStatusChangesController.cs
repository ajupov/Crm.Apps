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
    [Route("Api/Deals/Statuses/Changes")]
    public class DealStatusesChangesController : AllowingCheckControllerBase
    {
        private readonly IDealStatusesService _dealStatusesService;
        private readonly IDealStatusChangesService _dealStatusChangesService;

        public DealStatusesChangesController(
            IUserContext userContext,
            IDealStatusChangesService dealStatusChangesService,
            IDealStatusesService dealStatusesService)
            : base(userContext)
        {
            _dealStatusChangesService = dealStatusChangesService;
            _dealStatusesService = dealStatusesService;
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<List<DealStatusChange>>> GetPagedList(
            DealStatusChangeGetPagedListRequestParameter request,
            CancellationToken ct = default)
        {
            var status = await _dealStatusesService.GetAsync(request.StatusId, ct);
            var changes = await _dealStatusChangesService.GetPagedListAsync(request, ct);

            return ReturnIfAllowed(changes, Roles.Sales, status.AccountId);
        }
    }
}