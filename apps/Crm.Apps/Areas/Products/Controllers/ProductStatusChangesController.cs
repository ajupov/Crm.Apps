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
    [Route("Api/Products/Statuses/Changes")]
    public class ProductStatusesChangesController : ControllerBase
    {
        private readonly IProductStatusChangesService _userStatusChangesService;

        public ProductStatusesChangesController(IProductStatusChangesService userStatusChangesService)
        {
            _userStatusChangesService = userStatusChangesService;
        }

        [HttpPost("GetPagedList")]
        [RequireAny(Role.System, Role.Development, Role.Administration, Role.TechnicalSupport)]
        public async Task<ActionResult<List<ProductStatusChange>>> GetPagedList(
            ProductStatusChangeGetPagedListParameter parameter, CancellationToken ct = default)
        {
            return await _userStatusChangesService.GetPagedListAsync(parameter, ct);
        }
    }
}