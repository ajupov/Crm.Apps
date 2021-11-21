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
    [Route("Stock/Balances/Changes/v1")]
    public class StockBalanceChangesController : AllowingCheckControllerBase
    {
        private readonly IStockBalancesService _stockBalancesService;
        private readonly IStockBalanceChangesService _stockBalanceChangesService;

        public StockBalanceChangesController(
            IUserContext userContext,
            IStockBalancesService stockBalancesService,
            IStockBalanceChangesService stockBalanceChangesService)
            : base(userContext)
        {
            _stockBalanceChangesService = stockBalanceChangesService;
            _stockBalancesService = stockBalancesService;
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<StockBalanceChangeGetPagedListResponse>> GetPagedList(
            StockBalanceChangeGetPagedListRequest request,
            CancellationToken ct = default)
        {
            var balance = await _stockBalancesService.GetAsync(request.StockBalanceId, false, ct);
            var response = await _stockBalanceChangesService.GetPagedListAsync(request, ct);

            return ReturnIfAllowed(response, Roles.Stock, balance.AccountId);
        }
    }
}
