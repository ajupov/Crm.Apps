using System.Threading;
using System.Threading.Tasks;
using Ajupov.Infrastructure.All.Api.Attributes;
using Ajupov.Infrastructure.All.Jwt;
using Crm.Apps.Deals.Services;
using Crm.Apps.Deals.V1.Requests;
using Crm.Apps.Deals.V1.Responses;
using Crm.Common.All.BaseControllers;
using Crm.Common.All.Roles;
using Crm.Common.All.Roles.Attributes;
using Crm.Common.All.UserContext;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Deals.V1.Controllers
{
    [ApiController]
    [RequestContentTypeApplicationJson]
    [ResponseContentTypeApplicationJson]
    [RequireDealsRole(JwtDefaults.AuthenticationScheme)]
    [Route("Deals/Statuses/Changes/v1")]
    public class DealStatusChangesController : AllowingCheckControllerBase
    {
        private readonly IDealStatusesService _dealStatusesService;
        private readonly IDealStatusChangesService _dealStatusChangesService;

        public DealStatusChangesController(
            IUserContext userContext,
            IDealStatusChangesService dealStatusChangesService,
            IDealStatusesService dealStatusesService)
            : base(userContext)
        {
            _dealStatusChangesService = dealStatusChangesService;
            _dealStatusesService = dealStatusesService;
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<DealStatusChangeGetPagedListResponse>> GetPagedList(
            DealStatusChangeGetPagedListRequest request,
            CancellationToken ct = default)
        {
            var status = await _dealStatusesService.GetAsync(request.StatusId, false, ct);
            var response = await _dealStatusChangesService.GetPagedListAsync(request, ct);

            return ReturnIfAllowed(response, Roles.Deals, status.AccountId);
        }
    }
}
