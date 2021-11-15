using System.Threading;
using System.Threading.Tasks;
using Ajupov.Infrastructure.All.Api.Attributes;
using Ajupov.Infrastructure.All.Jwt;
using Crm.Apps.Suppliers.Models;
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
    [Route("Suppliers/Comments/v1")]
    public class SupplierCommentsController : AllowingCheckControllerBase
    {
        private readonly IUserContext _userContext;
        private readonly ISuppliersService _suppliersService;
        private readonly ISupplierCommentsService _supplierCommentsService;

        public SupplierCommentsController(
            IUserContext userContext,
            ISuppliersService suppliersService,
            ISupplierCommentsService supplierCommentsService)
            : base(userContext)
        {
            _userContext = userContext;
            _suppliersService = suppliersService;
            _supplierCommentsService = supplierCommentsService;
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<SupplierCommentGetPagedListResponse>> GetPagedList(
            SupplierCommentGetPagedListRequest request,
            CancellationToken ct = default)
        {
            var supplier = await _suppliersService.GetAsync(request.SupplierId, false, ct);
            var response = await _supplierCommentsService.GetPagedListAsync(request, ct);

            return ReturnIfAllowed(response, Roles.Suppliers, supplier.AccountId);
        }

        [HttpPost("Create")]
        public async Task<ActionResult> Create(SupplierComment comment, CancellationToken ct = default)
        {
            var supplier = await _suppliersService.GetAsync(comment.SupplierId, false, ct);

            return await ActionIfAllowed(
                () => _supplierCommentsService.CreateAsync(_userContext.UserId, comment, ct),
                Roles.Suppliers,
                supplier.AccountId);
        }
    }
}
