using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Products.Models;
using Crm.Apps.Products.RequestParameters;
using Crm.Apps.Products.Roles;
using Crm.Apps.Products.Services;
using Crm.Common.All.UserContext;
using Crm.Common.All.UserContext.BaseControllers;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Products.Controllers
{
    [ApiController]
    [RequireProductsRole]
    [Route("Api/Products/Statuses/Changes")]
    public class ProductStatusesChangesController : AllowingCheckControllerBase
    {
        private readonly IProductStatusesService _productStatusesService;
        private readonly IProductStatusChangesService _userStatusChangesService;

        public ProductStatusesChangesController(
            IUserContext userContext,
            IProductStatusesService productStatusesService,
            IProductStatusChangesService userStatusChangesService)
            : base(userContext)

        {
            _productStatusesService = productStatusesService;
            _userStatusChangesService = userStatusChangesService;
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<List<ProductStatusChange>>> GetPagedList(
            ProductStatusChangeGetPagedListRequestParameter request,
            CancellationToken ct = default)
        {
            var status = await _productStatusesService.GetAsync(request.StatusId, ct);
            var changes = await _userStatusChangesService.GetPagedListAsync(request, ct);

            return ReturnIfAllowed(changes, ProductsRoles.Value, status.AccountId);
        }
    }
}