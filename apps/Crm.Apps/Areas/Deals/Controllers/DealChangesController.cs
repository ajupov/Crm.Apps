using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Deals.Models;
using Crm.Apps.Areas.Deals.Parameters;
using Crm.Apps.Areas.Deals.Services;
using Crm.Common.UserContext;
using Crm.Common.UserContext.Attributes;
using Crm.Common.UserContext.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Areas.Deals.Controllers
{
    [ApiController]
    [RequirePrivileged(Role.AccountOwning, Role.SalesManagement)]
    [Route("Api/Deals/Changes")]
    public class DealChangesController : UserContextController
    {
        private readonly IDealsService _dealsService;
        private readonly IDealChangesService _dealChangesService;

        public DealChangesController(
            IUserContext userContext,
            IDealsService dealsService,
            IDealChangesService dealChangesService)
            : base(userContext)
        {
            _dealChangesService = dealChangesService;
            _dealsService = dealsService;
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<List<DealChange>>> GetPagedList(
            DealChangeGetPagedListParameter parameter,
            CancellationToken ct = default)
        {
            var deal = await _dealsService.GetAsync(parameter.DealId, ct);
            var changes = await _dealChangesService.GetPagedListAsync(parameter, ct);

            return ReturnIfAllowed(changes, new[] {Role.AccountOwning, Role.SalesManagement}, deal.AccountId);
        }
    }
}