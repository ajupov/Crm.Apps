using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Products.Models;
using Crm.Apps.Areas.Products.RequestParameters;
using Crm.Apps.Areas.Products.Services;
using Crm.Common.UserContext;
using Crm.Common.UserContext.Attributes;
using Crm.Common.UserContext.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Areas.Products.Controllers
{
    [ApiController]
    [RequirePrivileged(Role.AccountOwning, Role.ProductsManagement)]
    [Route("Api/Products/Statuses/Changes")]
    public class ProductStatusesChangesController : UserContextController
    {
        private readonly IProductStatusesService _productStatusesService;
        private readonly IProductStatusChangesService _userStatusChangesService;

        public ProductStatusesChangesController(
            IUserContext userContext,
            IProductStatusesService productStatusesService,
            IProductStatusChangesService userStatusChangesService)
            : base(userContext)

        {
            _productStatusesService = productStatusesService;
            _userStatusChangesService = userStatusChangesService;
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<List<ProductStatusChange>>> GetPagedList(
            ProductStatusChangeGetPagedListRequestParameter request,
            CancellationToken ct = default)
        {
            var status = await _productStatusesService.GetAsync(request.StatusId, ct);
            var changes = await _userStatusChangesService.GetPagedListAsync(request, ct);

            return ReturnIfAllowed(changes, new[] {Role.AccountOwning, Role.ProductsManagement}, status.AccountId);
        }
    }
}