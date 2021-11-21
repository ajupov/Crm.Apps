using System.Threading;
using System.Threading.Tasks;
using Ajupov.Infrastructure.All.Api.Attributes;
using Ajupov.Infrastructure.All.Jwt;
using Crm.Apps.Stock.Services;
using Crm.Apps.Stock.V1.Requests;
using Crm.Apps.Stock.V1.Responses;
using Crm.Common.All.BaseControllers;
using Crm.Common.All.Roles;
using Crm.Common.All.Roles.Attributes;
using Crm.Common.All.UserContext;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Stock.V1.Controllers
{
    [ApiController]
    [RequestContentTypeApplicationJson]
    [ResponseContentTypeApplicationJson]
    [RequireStockRole(JwtDefaults.AuthenticationScheme)]
    [Route("Stock/Arrivals/Changes/v1")]
    public class StockArrivalChangesController : AllowingCheckControllerBase
    {
        private readonly IStockArrivalsService _stockArrivalsService;
        private readonly IStockArrivalChangesService _stockArrivalChangesService;

        public StockArrivalChangesController(
            IUserContext userContext,
            IStockArrivalsService stockArrivalsService,
            IStockArrivalChangesService stockArrivalChangesService)
            : base(userContext)
        {
            _stockArrivalChangesService = stockArrivalChangesService;
            _stockArrivalsService = stockArrivalsService;
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<StockArrivalChangeGetPagedListResponse>> GetPagedList(
            StockArrivalChangeGetPagedListRequest request,
            CancellationToken ct = default)
        {
            var arrival = await _stockArrivalsService.GetAsync(request.StockArrivalId, false, ct);
            var response = await _stockArrivalChangesService.GetPagedListAsync(request, ct);

            return ReturnIfAllowed(response, Roles.Stock, arrival.AccountId);
        }
    }
}
