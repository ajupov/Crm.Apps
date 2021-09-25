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
    [Route("Orders/Attributes/Changes/v1")]
    public class OrderAttributeChangesController : AllowingCheckControllerBase
    {
        private readonly IOrderAttributesService _orderAttributesService;
        private readonly IOrderAttributeChangesService _orderAttributeChangesService;

        public OrderAttributeChangesController(
            IUserContext userContext,
            IOrderAttributesService orderAttributesService,
            IOrderAttributeChangesService orderAttributeChangesService)
            : base(userContext)
        {
            _orderAttributeChangesService = orderAttributeChangesService;
            _orderAttributesService = orderAttributesService;
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<OrderAttributeChangeGetPagedListResponse>> GetPagedList(
            OrderAttributeChangeGetPagedListRequest request,
            CancellationToken ct = default)
        {
            var attribute = await _orderAttributesService.GetAsync(request.AttributeId, false, ct);
            var response = await _orderAttributeChangesService.GetPagedListAsync(request, ct);

            return ReturnIfAllowed(response, Roles.Orders, attribute.AccountId);
        }
    }
}
