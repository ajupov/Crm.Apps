using System.Threading;
using System.Threading.Tasks;
using Ajupov.Infrastructure.All.Jwt;
using Ajupov.Infrastructure.All.Mvc.Attributes;
using Crm.Apps.Products.Services;
using Crm.Apps.Products.V1.Requests;
using Crm.Apps.Products.V1.Responses;
using Crm.Common.All.BaseControllers;
using Crm.Common.All.Roles;
using Crm.Common.All.Roles.Attributes;
using Crm.Common.All.UserContext;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Products.V1.Controllers
{
    [ApiController]
    [RequestContentTypeApplicationJson]
    [ResponseContentTypeApplicationJson]
    [RequireProductsRole(JwtDefaults.AuthenticationScheme)]
    [Route("Products/Statuses/Changes/v1")]
    public class ProductStatusChangesController : AllowingCheckControllerBase
    {
        private readonly IProductStatusesService _productStatusesService;
        private readonly IProductStatusChangesService _userStatusChangesService;

        public ProductStatusChangesController(
            IUserContext userContext,
            IProductStatusesService productStatusesService,
            IProductStatusChangesService userStatusChangesService)
            : base(userContext)
        {
            _productStatusesService = productStatusesService;
            _userStatusChangesService = userStatusChangesService;
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<ProductStatusChangeGetPagedListResponse>> GetPagedList(
            ProductStatusChangeGetPagedListRequest request,
            CancellationToken ct = default)
        {
            var status = await _productStatusesService.GetAsync(request.StatusId, false, ct);
            var response = await _userStatusChangesService.GetPagedListAsync(request, ct);

            return ReturnIfAllowed(response, Roles.Products, status.AccountId);
        }
    }
}
