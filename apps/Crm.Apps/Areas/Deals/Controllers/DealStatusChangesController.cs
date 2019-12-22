using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Deals.Models;
using Crm.Apps.Areas.Deals.RequestParameters;
using Crm.Apps.Areas.Deals.Services;
using Crm.Common.UserContext;
using Crm.Common.UserContext.Attributes;
using Crm.Common.UserContext.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Areas.Deals.Controllers
{
    [ApiController]
    [RequirePrivileged(Role.AccountOwning, Role.SalesManagement)]
    [Route("Api/Deals/Statuses/Changes")]
    public class DealStatusesChangesController : UserContextController
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