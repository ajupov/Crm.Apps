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
    [Route("Api/Products/Changes")]
    public class ProductChangesController : ControllerBase
    {
        private readonly IProductChangesService _productChangesService;

        public ProductChangesController(IProductChangesService productChangesService)
        {
            _productChangesService = productChangesService;
        }

        [HttpPost("GetPagedList")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.TechnicalSupport)]
        public async Task<ActionResult<List<ProductChange>>> GetPagedList(ProductChangeGetPagedListParameter parameter,
            CancellationToken ct = default)
        {
            return await _productChangesService.GetPagedListAsync(parameter, ct).ConfigureAwait(false);
        }
    }
}