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
    [Route("Api/Deals/Types/Changes")]
    public class DealTypesChangesController : UserContextController
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
            DealTypeChangeGetPagedListParameter parameter,
            CancellationToken ct = default)
        {
            var type = await _dealTypesService.GetAsync(parameter.TypeId, ct);
            var changes = await _dealTypeChangesService.GetPagedListAsync(parameter, ct);

            return ReturnIfAllowed(changes, new[] {Role.AccountOwning, Role.SalesManagement}, type.AccountId);
        }
    }
}