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
    [Route("Orders/Types/Changes/v1")]
    public class OrderTypeChangesController : AllowingCheckControllerBase
    {
        private readonly IOrderTypesService _orderTypesService;
        private readonly IOrderTypeChangesService _orderTypeChangesService;

        public OrderTypeChangesController(
            IUserContext userContext,
            IOrderTypesService orderTypesService,
            IOrderTypeChangesService orderTypeChangesService)
            : base(userContext)
        {
            _orderTypesService = orderTypesService;
            _orderTypeChangesService = orderTypeChangesService;
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<OrderTypeChangeGetPagedListResponse>> GetPagedList(
            OrderTypeChangeGetPagedListRequest request,
            CancellationToken ct = default)
        {
            var type = await _orderTypesService.GetAsync(request.TypeId, false, ct);
            var response = await _orderTypeChangesService.GetPagedListAsync(request, ct);

            return ReturnIfAllowed(response, Roles.Orders, type.AccountId);
        }
    }
}
