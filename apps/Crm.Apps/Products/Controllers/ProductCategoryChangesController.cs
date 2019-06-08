using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Products.Models;
using Crm.Apps.Products.Parameters;
using Crm.Apps.Products.Services;
using Crm.Common.UserContext;
using Crm.Common.UserContext.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Products.Controllers
{
    [ApiController]
    [Route("Api/Products/Categories/Changes")]
    public class ProductCategoryChangesController : ControllerBase
    {
        private readonly IProductCategoryChangesService _productCategoryChangesService;

        public ProductCategoryChangesController(IProductCategoryChangesService productCategoryChangesService)
        {
            _productCategoryChangesService = productCategoryChangesService;
        }

        [HttpPost("GetPagedList")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.TechnicalSupport)]
        public async Task<ActionResult<List<ProductCategoryChange>>> GetPagedList(
            ProductCategoryChangeGetPagedListParameter parameter, CancellationToken ct = default)
        {
            return await _productCategoryChangesService.GetPagedListAsync(parameter, ct).ConfigureAwait(false);
        }
    }
}