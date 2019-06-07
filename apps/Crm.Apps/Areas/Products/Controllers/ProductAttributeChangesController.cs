using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Products.Models;
using Crm.Apps.Areas.Products.Parameters;
using Crm.Apps.Areas.Products.Services;
using Crm.Common.UserContext;
using Crm.Common.UserContext.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Areas.Products.Controllers
{
    [ApiController]
    [Route("Api/Products/Attributes/Changes")]
    public class ProductAttributeChangesController : ControllerBase
    {
        private readonly IProductAttributeChangesService _productAttributeChangesService;

        public ProductAttributeChangesController(IProductAttributeChangesService productAttributeChangesService)
        {
            _productAttributeChangesService = productAttributeChangesService;
        }

        [HttpPost("GetPagedList")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.TechnicalSupport)]
        public async Task<ActionResult<List<ProductAttributeChange>>> GetPagedList(
            ProductAttributeChangeGetPagedListParameter parameter, CancellationToken ct = default)
        {
            return await _productAttributeChangesService.GetPagedListAsync(parameter, ct).ConfigureAwait(false);
        }
    }
}