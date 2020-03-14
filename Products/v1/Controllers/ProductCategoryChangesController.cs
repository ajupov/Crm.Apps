using System.Threading;
using System.Threading.Tasks;
using Ajupov.Infrastructure.All.Jwt;
using Ajupov.Infrastructure.All.Mvc.Attributes;
using Crm.Apps.Products.Services;
using Crm.Apps.Products.v1.Requests;
using Crm.Apps.Products.v1.Responses;
using Crm.Common.All.BaseControllers;
using Crm.Common.All.Roles;
using Crm.Common.All.Roles.Attributes;
using Crm.Common.All.UserContext;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Products.v1.Controllers
{
    [ApiController]
    [RequestContentTypeApplicationJson]
    [ResponseContentTypeApplicationJson]
    [RequireProductsRole(JwtDefaults.AuthenticationScheme)]
    [Route("Products/Categories/Changes/v1")]
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
        public async Task<ActionResult<ProductCategoryChangeGetPagedListResponse>> GetPagedList(
            ProductCategoryChangeGetPagedListRequest request,
            CancellationToken ct = default)
        {
            var category = await _productCategoriesService.GetAsync(request.CategoryId, ct);
            var response = await _productCategoryChangesService.GetPagedListAsync(request, ct);

            return ReturnIfAllowed(response, Roles.Products, category.AccountId);
        }
    }
}