using System.Threading;
using System.Threading.Tasks;
using Ajupov.Infrastructure.All.Api.Attributes;
using Ajupov.Infrastructure.All.Jwt;
using Crm.Apps.Orders.Services;
using Crm.Apps.Orders.V1.Requests;
using Crm.Apps.Orders.V1.Responses;
using Crm.Common.All.BaseControllers;
using Crm.Common.All.Roles;
using Crm.Common.All.Roles.Attributes;
using Crm.Common.All.UserContext;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Orders.V1.Controllers
{
    [ApiController]
    [RequestContentTypeApplicationJson]
    [ResponseContentTypeApplicationJson]
    [RequireOrdersRole(JwtDefaults.AuthenticationScheme)]
    [Route("Orders/Changes/v1")]
    public class OrderChangesController : AllowingCheckControllerBase
    {
        private readonly IOrdersService _ordersService;
        private readonly IOrderChangesService _orderChangesService;

        public OrderChangesController(
            IUserContext userContext,
            IOrdersService ordersService,
            IOrderChangesService orderChangesService)
            : base(userContext)
        {
            _orderChangesService = orderChangesService;
            _ordersService = ordersService;
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<OrderChangeGetPagedListResponse>> GetPagedList(
            OrderChangeGetPagedListRequest request,
            CancellationToken ct = default)
        {
            var order = await _ordersService.GetAsync(request.OrderId, false, ct);
            var response = await _orderChangesService.GetPagedListAsync(request, ct);

            return ReturnIfAllowed(response, Roles.Orders, order.AccountId);
        }
    }
}
