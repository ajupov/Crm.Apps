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
    [RequireOrdersRole(JwtDefaults.AuthenticationScheme)]
    [Route("Orders/Types/Changes/v1")]
    public class StockRoomChangesController : AllowingCheckControllerBase
    {
        private readonly IStockRoomsService _stockRoomsService;
        private readonly IStockRoomChangesService _stockRoomChangesService;

        public StockRoomChangesController(
            IUserContext userContext,
            IStockRoomsService stockRoomsService,
            IStockRoomChangesService stockRoomChangesService)
            : base(userContext)
        {
            _stockRoomsService = stockRoomsService;
            _stockRoomChangesService = stockRoomChangesService;
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<StockRoomChangeGetPagedListResponse>> GetPagedList(
            StockRoomChangeGetPagedListRequest request,
            CancellationToken ct = default)
        {
            var type = await _stockRoomsService.GetAsync(request.StockRoomId, false, ct);
            var response = await _stockRoomChangesService.GetPagedListAsync(request, ct);

            return ReturnIfAllowed(response, Roles.Orders, type.AccountId);
        }
    }
}
