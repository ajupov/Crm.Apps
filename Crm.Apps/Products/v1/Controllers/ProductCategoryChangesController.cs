using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Infrastructure.All.Jwt;
using Crm.Apps.Products.Services;
using Crm.Apps.Products.v1.Models;
using Crm.Apps.Products.v1.RequestParameters;
using Crm.Common.All.BaseControllers;
using Crm.Common.All.Roles;
using Crm.Common.All.Roles.Attributes;
using Crm.Common.All.UserContext;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Products.v1.Controllers
{
    [ApiController]
    [RequireProductsRole(JwtDefaults.AuthenticationScheme)]
    [Route("api/v1/Products/Categories/Changes")]
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

            return ReturnIfAllowed(changes, Roles.Products, category.AccountId);
        }
    }
}