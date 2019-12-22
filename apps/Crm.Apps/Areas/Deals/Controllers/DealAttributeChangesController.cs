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
    [Route("Api/Deals/Attributes/Changes")]
    public class DealAttributeChangesController : ControllerBase
    {
        private readonly IDealAttributeChangesService _dealAttributeChangesService;

        public DealAttributeChangesController(IDealAttributeChangesService dealAttributeChangesService)
        {
            _dealAttributeChangesService = dealAttributeChangesService;
        }

        [HttpPost("GetPagedList")]
        [RequireAny(Role.System, Role.Development, Role.Administration, Role.TechnicalSupport)]
        public async Task<ActionResult<List<DealAttributeChange>>> GetPagedList(
            DealAttributeChangeGetPagedListParameter parameter, CancellationToken ct = default)
        {
            return await _dealAttributeChangesService.GetPagedListAsync(parameter, ct);
        }
    }
}