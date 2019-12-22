using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Deals.Models;
using Crm.Apps.Areas.Deals.Parameters;
using Crm.Apps.Areas.Deals.Services;
using Crm.Common.UserContext;
using Crm.Common.UserContext.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Areas.Deals.Controllers
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
        [RequireAny(Role.System, Role.Development, Role.Administration, Role.TechnicalSupport)]
        public async Task<ActionResult<List<DealTypeChange>>> GetPagedList(
            DealTypeChangeGetPagedListParameter parameter, CancellationToken ct = default)
        {
            return await _dealTypeChangesService.GetPagedListAsync(parameter, ct);
        }
    }
}