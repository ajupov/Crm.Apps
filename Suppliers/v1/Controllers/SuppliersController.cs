using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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
    [Route("Suppliers/v1")]
    public class SuppliersController : AllowingCheckControllerBase
    {
        private readonly IUserContext _userContext;
        private readonly ISuppliersService _suppliersService;

        public SuppliersController(IUserContext userContext, ISuppliersService suppliersService)
            : base(userContext)
        {
            _userContext = userContext;
            _suppliersService = suppliersService;
        }

        [HttpGet("Get")]
        public async Task<ActionResult<Supplier>> Get([Required] Guid id, CancellationToken ct = default)
        {
            var supplier = await _suppliersService.GetAsync(id, false, ct);
            if (supplier == null)
            {
                return NotFound(id);
            }

            return ReturnIfAllowed(supplier, Roles.Suppliers, supplier.AccountId);
        }

        [HttpPost("GetList")]
        public async Task<ActionResult<List<Supplier>>> GetList(
            [Required] List<Guid> ids,
            CancellationToken ct = default)
        {
            var suppliers = await _suppliersService.GetListAsync(ids, ct);

            return ReturnIfAllowed(
                suppliers,
                Roles.Suppliers,
                suppliers.Select(x => x.AccountId));
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<SupplierGetPagedListResponse>> GetPagedList(
            SupplierGetPagedListRequest request,
            CancellationToken ct = default)
        {
            var response = await _suppliersService.GetPagedListAsync(_userContext.AccountId, request, ct);

            return ReturnIfAllowed(
                response,
                Roles.Suppliers,
                response.Suppliers.Select(x => x.AccountId));
        }

        [HttpPost("Create")]
        public async Task<ActionResult<Guid>> Create(Supplier supplier, CancellationToken ct = default)
        {
            supplier.AccountId = _userContext.AccountId;

            var id = await _suppliersService.CreateAsync(_userContext.UserId, supplier, ct);

            return Created(nameof(Get), id);
        }

        [HttpPatch("Update")]
        public async Task<ActionResult> Update(Supplier supplier, CancellationToken ct = default)
        {
            var oldSupplier = await _suppliersService.GetAsync(supplier.Id, true, ct);
            if (oldSupplier == null)
            {
                return NotFound(supplier.Id);
            }

            return await ActionIfAllowed(
                () => _suppliersService.UpdateAsync(_userContext.UserId, oldSupplier, supplier, ct),
                Roles.Suppliers,
                oldSupplier.AccountId);
        }

        [HttpPatch("Delete")]
        public async Task<ActionResult> Delete([Required] List<Guid> ids, CancellationToken ct = default)
        {
            var suppliers = await _suppliersService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _suppliersService.DeleteAsync(_userContext.UserId, suppliers.Select(x => x.Id), ct),
                Roles.Suppliers,
                suppliers.Select(x => x.AccountId));
        }

        [HttpPatch("Restore")]
        public async Task<ActionResult> Restore([Required] List<Guid> ids, CancellationToken ct = default)
        {
            var suppliers = await _suppliersService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _suppliersService.RestoreAsync(_userContext.UserId, suppliers.Select(x => x.Id), ct),
                Roles.Suppliers,
                suppliers.Select(x => x.AccountId));
        }
    }
}
