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
    [Route("Api/Products/Categories/Changes")]
    public class ProductCategoryChangesController : AllowingCheckControllerBase
    {
        private readonly IProductCategoriesService _productCategoriesService;
        private readonly IProductCategoryChangesService _productCategoryChangesService;

        public ProductCategoryChangesController(
            IUserContext userContext,
            IProductCategoriesService productCategoriesService,
            IProductCategoryChangesService productCategoryChangesService)
            : base(userContext)
        {
            _productCategoriesService = productCategoriesService;
            _productCategoryChangesService = productCategoryChangesService;
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<List<ProductCategoryChange>>> GetPagedList(
            ProductCategoryChangeGetPagedListRequestParameter request,
            CancellationToken ct = default)
        {
            var category = await _productCategoriesService.GetAsync(request.CategoryId, ct);
            var changes = await _productCategoryChangesService.GetPagedListAsync(request, ct);

            return ReturnIfAllowed(changes, ProductsRoles.Value, category.AccountId);
        }
    }
}