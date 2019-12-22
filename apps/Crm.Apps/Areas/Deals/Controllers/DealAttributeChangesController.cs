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
    [Route("Api/Deals/Attributes/Changes")]
    public class DealAttributeChangesController : UserContextController
    {
        private readonly IDealAttributesService _dealAttributesService;
        private readonly IDealAttributeChangesService _dealAttributeChangesService;

        public DealAttributeChangesController(
            IUserContext userContext,
            IDealAttributesService dealAttributesService,
            IDealAttributeChangesService dealAttributeChangesService)
            : base(userContext)
        {
            _dealAttributeChangesService = dealAttributeChangesService;
            _dealAttributesService = dealAttributesService;
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<List<DealAttributeChange>>> GetPagedList(
            DealAttributeChangeGetPagedListRequestParameter request,
            CancellationToken ct = default)
        {
            var attribute = await _dealAttributesService.GetAsync(request.AttributeId, ct);
            var changes = await _dealAttributeChangesService.GetPagedListAsync(request, ct);

            return ReturnIfAllowed(changes, new[] {Role.AccountOwning, Role.SalesManagement}, attribute.AccountId);
        }
    }
}