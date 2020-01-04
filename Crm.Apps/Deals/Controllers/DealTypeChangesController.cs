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
    [Route("Api/Deals/Types/Changes")]
    public class DealTypesChangesController : AllowingCheckControllerBase
    {
        private readonly IDealTypesService _dealTypesService;
        private readonly IDealTypeChangesService _dealTypeChangesService;

        public DealTypesChangesController(
            IUserContext userContext,
            IDealTypesService dealTypesService,
            IDealTypeChangesService dealTypeChangesService)
            : base(userContext)
        {
            _dealTypesService = dealTypesService;
            _dealTypeChangesService = dealTypeChangesService;
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<List<DealTypeChange>>> GetPagedList(
            DealTypeChangeGetPagedListRequestParameter request,
            CancellationToken ct = default)
        {
            var type = await _dealTypesService.GetAsync(request.TypeId, ct);
            var changes = await _dealTypeChangesService.GetPagedListAsync(request, ct);

            return ReturnIfAllowed(changes, new[] {Role.AccountOwning, Role.SalesManagement}, type.AccountId);
        }
    }
}