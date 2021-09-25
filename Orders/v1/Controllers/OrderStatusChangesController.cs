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
    [Route("Orders/Statuses/Changes/v1")]
    public class OrderStatusChangesController : AllowingCheckControllerBase
    {
        private readonly IOrderStatusesService _orderStatusesService;
        private readonly IOrderStatusChangesService _orderStatusChangesService;

        public OrderStatusChangesController(
            IUserContext userContext,
            IOrderStatusChangesService orderStatusChangesService,
            IOrderStatusesService orderStatusesService)
            : base(userContext)
        {
            _orderStatusChangesService = orderStatusChangesService;
            _orderStatusesService = orderStatusesService;
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<OrderStatusChangeGetPagedListResponse>> GetPagedList(
            OrderStatusChangeGetPagedListRequest request,
            CancellationToken ct = default)
        {
            var status = await _orderStatusesService.GetAsync(request.StatusId, false, ct);
            var response = await _orderStatusChangesService.GetPagedListAsync(request, ct);

            return ReturnIfAllowed(response, Roles.Orders, status.AccountId);
        }
    }
}
