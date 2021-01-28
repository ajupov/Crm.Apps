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
    [Route("Products/Attributes/Changes/v1")]
    public class ProductAttributeChangesController : AllowingCheckControllerBase
    {
        private readonly IProductAttributesService _productAttributesService;
        private readonly IProductAttributeChangesService _productAttributeChangesService;

        public ProductAttributeChangesController(
            IUserContext userContext,
            IProductAttributesService productAttributesService,
            IProductAttributeChangesService productAttributeChangesService)
            : base(userContext)
        {
            _productAttributesService = productAttributesService;
            _productAttributeChangesService = productAttributeChangesService;
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<ProductAttributeChangeGetPagedListResponse>> GetPagedList(
            ProductAttributeChangeGetPagedListRequest request,
            CancellationToken ct = default)
        {
            var attribute = await _productAttributesService.GetAsync(request.AttributeId, false, ct);
            var response = await _productAttributeChangesService.GetPagedListAsync(request, ct);

            return ReturnIfAllowed(response, Roles.Products, attribute.AccountId);
        }
    }
}
