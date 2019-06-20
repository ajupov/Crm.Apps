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
    [Route("Api/Products/Statuses/Changes")]
    public class ProductStatusesChangesController : ControllerBase
    {
        private readonly IProductStatusChangesService _userStatusChangesService;

        public ProductStatusesChangesController(IProductStatusChangesService userStatusChangesService)
        {
            _userStatusChangesService = userStatusChangesService;
        }

        [HttpPost("GetPagedList")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.TechnicalSupport)]
        public async Task<ActionResult<List<ProductStatusChange>>> GetPagedList(
            ProductStatusChangeGetPagedListParameter parameter, CancellationToken ct = default)
        {
            return await _userStatusChangesService.GetPagedListAsync(parameter, ct).ConfigureAwait(false);
        }
    }
}