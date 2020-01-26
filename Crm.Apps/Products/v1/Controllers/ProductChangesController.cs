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
    [Route("Products/Changes/v1")]
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