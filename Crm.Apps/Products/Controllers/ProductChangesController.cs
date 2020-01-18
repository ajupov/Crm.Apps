using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Products.Models;
using Crm.Apps.Products.RequestParameters;
using Crm.Apps.Products.Services;
using Crm.Apps.UserContext.Attributes.Roles;
using Crm.Common.All.BaseControllers;
using Crm.Common.All.Roles;
using Crm.Common.All.UserContext;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Products.Controllers
{
    [ApiController]
    [RequireProductsRole]
    [Route("Api/Products/Changes")]
    public class ProductChangesController : AllowingCheckControllerBase
    {
        private readonly IProductsService _productsService;
        private readonly IProductChangesService _productChangesService;

        public ProductChangesController(
            IUserContext userContext,
            IProductsService productsService,
            IProductChangesService productChangesService)
            : base(userContext)
        {
            _productsService = productsService;
            _productChangesService = productChangesService;
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<List<ProductChange>>> GetPagedList(
            ProductChangeGetPagedListRequestParameter request,
            CancellationToken ct = default)
        {
            var product = await _productsService.GetAsync(request.ProductId, ct);
            var changes = await _productChangesService.GetPagedListAsync(request, ct);

            return ReturnIfAllowed(changes, Roles.Products, product.AccountId);
        }
    }
}