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
    [Route("Suppliers/Attributes/Changes/v1")]
    public class SupplierAttributeChangesController : AllowingCheckControllerBase
    {
        private readonly ISupplierAttributesService _supplierAttributesService;
        private readonly ISupplierAttributeChangesService _supplierAttributeChangesService;

        public SupplierAttributeChangesController(
            IUserContext userContext,
            ISupplierAttributeChangesService supplierAttributeChangesService,
            ISupplierAttributesService supplierAttributesService)
            : base(userContext)
        {
            _supplierAttributeChangesService = supplierAttributeChangesService;
            _supplierAttributesService = supplierAttributesService;
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<SupplierAttributeChangeGetPagedListResponse>> GetPagedList(
            SupplierAttributeChangeGetPagedListRequest request,
            CancellationToken ct = default)
        {
            var attribute = await _supplierAttributesService.GetAsync(request.AttributeId, false, ct);
            var response = await _supplierAttributeChangesService.GetPagedListAsync(request, ct);

            return ReturnIfAllowed(response, Roles.Suppliers, attribute.AccountId);
        }
    }
}
