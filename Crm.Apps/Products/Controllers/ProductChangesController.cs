using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Products.Models;
using Crm.Apps.Products.RequestParameters;
using Crm.Apps.Products.Services;
using Crm.Common.All.UserContext;
using Crm.Common.All.UserContext.Attributes;
using Crm.Common.All.UserContext.BaseControllers;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Products.Controllers
{
    [ApiController]
    [RequirePrivileged(Role.AccountOwning, Role.ProductsManagement)]
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

            return ReturnIfAllowed(changes, new[] {Role.AccountOwning, Role.ProductsManagement}, product.AccountId);
        }
    }
}