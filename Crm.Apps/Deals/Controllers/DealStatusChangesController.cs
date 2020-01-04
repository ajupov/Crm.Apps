using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Deals.Models;
using Crm.Apps.Deals.RequestParameters;
using Crm.Apps.Deals.Services;
using Crm.Common.All.UserContext;
using Crm.Common.All.UserContext.Attributes;
using Crm.Common.All.UserContext.BaseControllers;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Deals.Controllers
{
    [ApiController]
    [RequirePrivileged(Role.AccountOwning, Role.SalesManagement)]
    [Route("Api/Deals/Statuses/Changes")]
    public class DealStatusesChangesController : AllowingCheckControllerBase
    {
        private readonly IDealStatusesService _dealStatusesService;
        private readonly IDealStatusChangesService _dealStatusChangesService;

        public DealStatusesChangesController(
            IUserContext userContext,
            IDealStatusChangesService dealStatusChangesService,
            IDealStatusesService dealStatusesService)
            : base(userContext)
        {
            _dealStatusChangesService = dealStatusChangesService;
            _dealStatusesService = dealStatusesService;
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<List<DealStatusChange>>> GetPagedList(
            DealStatusChangeGetPagedListRequestParameter request,
            CancellationToken ct = default)
        {
            var status = await _dealStatusesService.GetAsync(request.StatusId, ct);
            var changes = await _dealStatusChangesService.GetPagedListAsync(request, ct);

            return ReturnIfAllowed(changes, new[] {Role.AccountOwning, Role.SalesManagement}, status.AccountId);
        }
    }
}