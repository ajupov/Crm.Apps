using System.Threading;
using System.Threading.Tasks;
using Ajupov.Infrastructure.All.Api.Attributes;
using Ajupov.Infrastructure.All.Jwt;
using Crm.Apps.Orders.Models;
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
    [Route("Orders/Comments/v1")]
    public class OrderCommentsController : AllowingCheckControllerBase
    {
        private readonly IUserContext _userContext;
        private readonly IOrdersService _ordersService;
        private readonly IOrderCommentsService _orderCommentsService;

        public OrderCommentsController(
            IUserContext userContext,
            IOrdersService ordersService,
            IOrderCommentsService orderCommentsService)
            : base(userContext)
        {
            _userContext = userContext;
            _ordersService = ordersService;
            _orderCommentsService = orderCommentsService;
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<OrderCommentGetPagedListResponse>> GetPagedList(
            OrderCommentGetPagedListRequest request,
            CancellationToken ct = default)
        {
            var order = await _ordersService.GetAsync(request.OrderId, false, ct);
            var response = await _orderCommentsService.GetPagedListAsync(request, ct);

            return ReturnIfAllowed(response, Roles.Orders, order.AccountId);
        }

        [HttpPost("Create")]
        public async Task<ActionResult> Create(OrderComment comment, CancellationToken ct = default)
        {
            var order = await _ordersService.GetAsync(comment.OrderId, false, ct);

            return await ActionIfAllowed(
                () => _orderCommentsService.CreateAsync(_userContext.UserId, comment, ct),
                Roles.Orders,
                order.AccountId);
        }
    }
}
