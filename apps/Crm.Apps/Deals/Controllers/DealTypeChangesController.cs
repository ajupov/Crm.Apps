using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Deals.Models;
using Crm.Apps.Deals.Parameters;
using Crm.Apps.Deals.Services;
using Crm.Common.UserContext;
using Crm.Common.UserContext.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Deals.Controllers
{
    [ApiController]
    [Route("Api/Deals/Types/Changes")]
    public class DealTypesChangesController : ControllerBase
    {
        private readonly IDealTypeChangesService _dealTypeChangesService;

        public DealTypesChangesController(IDealTypeChangesService dealTypeChangesService)
        {
            _dealTypeChangesService = dealTypeChangesService;
        }

        [HttpPost("GetPagedList")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.TechnicalSupport)]
        public async Task<ActionResult<List<DealTypeChange>>> GetPagedList(
            DealTypeChangeGetPagedListParameter parameter, CancellationToken ct = default)
        {
            return await _dealTypeChangesService.GetPagedListAsync(parameter, ct);
        }
    }
}