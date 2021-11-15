using System.Threading;
using System.Threading.Tasks;
using Ajupov.Infrastructure.All.Api.Attributes;
using Ajupov.Infrastructure.All.Jwt;
using Crm.Apps.Suppliers.Services;
using Crm.Apps.Suppliers.V1.Requests;
using Crm.Apps.Suppliers.V1.Responses;
using Crm.Common.All.BaseControllers;
using Crm.Common.All.Roles;
using Crm.Common.All.Roles.Attributes;
using Crm.Common.All.UserContext;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Suppliers.V1.Controllers
{
    [ApiController]
    [RequestContentTypeApplicationJson]
    [ResponseContentTypeApplicationJson]
    [RequireSuppliersRole(JwtDefaults.AuthenticationScheme)]
    [Route("Suppliers/Changes/v1")]
    public class SupplierChangesController : AllowingCheckControllerBase
    {
        private readonly ISuppliersService _suppliersService;
        private readonly ISupplierChangesService _supplierChangesService;

        public SupplierChangesController(
            IUserContext userContext,
            ISuppliersService suppliersService,
            ISupplierChangesService supplierChangesService)
            : base(userContext)
        {
            _suppliersService = suppliersService;
            _supplierChangesService = supplierChangesService;
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<SupplierChangeGetPagedListResponse>> GetPagedList(
            SupplierChangeGetPagedListRequest request,
            CancellationToken ct = default)
        {
            var supplier = await _suppliersService.GetAsync(request.SupplierId, false, ct);
            var response = await _supplierChangesService.GetPagedListAsync(request, ct);

            return ReturnIfAllowed(response, Roles.Suppliers, supplier.AccountId);
        }
    }
}
