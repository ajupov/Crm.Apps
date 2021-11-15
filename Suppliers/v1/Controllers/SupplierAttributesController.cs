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
    [Route("Suppliers/Attributes/v1")]
    public class SupplierAttributesController : AllowingCheckControllerBase
    {
        private readonly IUserContext _userContext;
        private readonly ISupplierAttributesService _supplierAttributesService;

        public SupplierAttributesController(
            IUserContext userContext,
            ISupplierAttributesService supplierAttributesService)
            : base(userContext)
        {
            _userContext = userContext;
            _supplierAttributesService = supplierAttributesService;
        }

        [HttpGet("Get")]
        public async Task<ActionResult<SupplierAttribute>> Get([Required] Guid id, CancellationToken ct = default)
        {
            var attribute = await _supplierAttributesService.GetAsync(id, false, ct);
            if (attribute == null)
            {
                return NotFound(id);
            }

            return ReturnIfAllowed(attribute, Roles.Suppliers, attribute.AccountId);
        }

        [HttpPost("GetList")]
        public async Task<ActionResult<List<SupplierAttribute>>> GetList(
            [Required] List<Guid> ids,
            CancellationToken ct = default)
        {
            var attributes = await _supplierAttributesService.GetListAsync(ids, ct);

            return ReturnIfAllowed(
                attributes,
                Roles.Suppliers,
                attributes.Select(x => x.AccountId));
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<SupplierAttributeGetPagedListResponse>> GetPagedList(
            SupplierAttributeGetPagedListRequest request,
            CancellationToken ct = default)
        {
            var response = await _supplierAttributesService.GetPagedListAsync(_userContext.AccountId, request, ct);

            return ReturnIfAllowed(
                response,
                Roles.Suppliers,
                response.Attributes.Select(x => x.AccountId));
        }

        [HttpPost("Create")]
        public async Task<ActionResult<Guid>> Create(SupplierAttribute attribute, CancellationToken ct = default)
        {
            attribute.AccountId = _userContext.AccountId;

            var id = await _supplierAttributesService.CreateAsync(_userContext.UserId, attribute, ct);

            return Created(nameof(Get), id);
        }

        [HttpPatch("Update")]
        public async Task<ActionResult> Update(SupplierAttribute attribute, CancellationToken ct = default)
        {
            var oldAttribute = await _supplierAttributesService.GetAsync(attribute.Id, true, ct);
            if (oldAttribute == null)
            {
                return NotFound(attribute.Id);
            }

            return await ActionIfAllowed(
                () => _supplierAttributesService.UpdateAsync(_userContext.UserId, oldAttribute, attribute, ct),
                Roles.Suppliers,
                oldAttribute.AccountId);
        }

        [HttpPatch("Delete")]
        public async Task<ActionResult> Delete([Required] List<Guid> ids, CancellationToken ct = default)
        {
            var attributes = await _supplierAttributesService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _supplierAttributesService.DeleteAsync(_userContext.UserId, attributes.Select(x => x.Id), ct),
                Roles.Suppliers,
                attributes.Select(x => x.AccountId));
        }

        [HttpPatch("Restore")]
        public async Task<ActionResult> Restore([Required] List<Guid> ids, CancellationToken ct = default)
        {
            var attributes = await _supplierAttributesService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _supplierAttributesService.RestoreAsync(_userContext.UserId, attributes.Select(x => x.Id), ct),
                Roles.Suppliers,
                attributes.Select(x => x.AccountId));
        }
    }
}
