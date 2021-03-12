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
    [Route("Deals/Types/Changes/v1")]
    public class DealTypeChangesController : AllowingCheckControllerBase
    {
        private readonly IDealTypesService _dealTypesService;
        private readonly IDealTypeChangesService _dealTypeChangesService;

        public DealTypeChangesController(
            IUserContext userContext,
            IDealTypesService dealTypesService,
            IDealTypeChangesService dealTypeChangesService)
            : base(userContext)
        {
            _dealTypesService = dealTypesService;
            _dealTypeChangesService = dealTypeChangesService;
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<DealTypeChangeGetPagedListResponse>> GetPagedList(
            DealTypeChangeGetPagedListRequest request,
            CancellationToken ct = default)
        {
            var type = await _dealTypesService.GetAsync(request.TypeId, false, ct);
            var response = await _dealTypeChangesService.GetPagedListAsync(request, ct);

            return ReturnIfAllowed(response, Roles.Deals, type.AccountId);
        }
    }
}
