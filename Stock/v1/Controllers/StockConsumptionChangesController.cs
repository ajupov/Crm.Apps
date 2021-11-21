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
    [Route("Stock/Consumptions/Changes/v1")]
    public class StockConsumptionChangesController : AllowingCheckControllerBase
    {
        private readonly IStockConsumptionsService _stockConsumptionsService;
        private readonly IStockConsumptionChangesService _stockConsumptionChangesService;

        public StockConsumptionChangesController(
            IUserContext userContext,
            IStockConsumptionsService stockConsumptionsService,
            IStockConsumptionChangesService stockConsumptionChangesService)
            : base(userContext)
        {
            _stockConsumptionChangesService = stockConsumptionChangesService;
            _stockConsumptionsService = stockConsumptionsService;
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<StockConsumptionChangeGetPagedListResponse>> GetPagedList(
            StockConsumptionChangeGetPagedListRequest request,
            CancellationToken ct = default)
        {
            var consumption = await _stockConsumptionsService.GetAsync(request.StockConsumptionId, false, ct);
            var response = await _stockConsumptionChangesService.GetPagedListAsync(request, ct);

            return ReturnIfAllowed(response, Roles.Stock, consumption.AccountId);
        }
    }
}
