using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Infrastructure.All.Jwt;
using Ajupov.Infrastructure.All.Mvc.Attributes;
using Ajupov.Utils.All.Enums;
using Ajupov.Utils.All.Guid;
using Crm.Apps.Products.Models;
using Crm.Apps.Products.Services;
using Crm.Apps.Products.V1.Requests;
using Crm.Apps.Products.V1.Responses;
using Crm.Common.All.BaseControllers;
using Crm.Common.All.Roles;
using Crm.Common.All.Roles.Attributes;
using Crm.Common.All.Types.AttributeType;
using Crm.Common.All.UserContext;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Products.V1.Controllers
{
    [ApiController]
    [RequestContentTypeApplicationJson]
    [ResponseContentTypeApplicationJson]
    [RequireProductsRole(JwtDefaults.AuthenticationScheme)]
    [Route("Products/Attributes/v1")]
    public class ProductAttributesController : AllowingCheckControllerBase
    {
        private readonly IUserContext _userContext;
        private readonly IProductAttributesService _productAttributesService;

        public ProductAttributesController(IUserContext userContext, IProductAttributesService productAttributesService)
            : base(userContext)
        {
            _userContext = userContext;
            _productAttributesService = productAttributesService;
        }

        [HttpGet("GetTypes")]
        public ActionResult<Dictionary<string, AttributeType>> GetTypes()
        {
            return EnumsExtensions.GetAsDictionary<AttributeType>();
        }

        [HttpGet("Get")]
        public async Task<ActionResult<ProductAttribute>> Get([Required] Guid id, CancellationToken ct = default)
        {
            var attribute = await _productAttributesService.GetAsync(id, false, ct);
            if (attribute == null)
            {
                return NotFound(id);
            }

            return ReturnIfAllowed(attribute, Roles.Products, attribute.AccountId);
        }

        [HttpPost("GetList")]
        public async Task<ActionResult<List<ProductAttribute>>> GetList(
            [Required] List<Guid> ids,
            CancellationToken ct = default)
        {
            var attributes = await _productAttributesService.GetListAsync(ids, ct);

            return ReturnIfAllowed(
                attributes,
                Roles.Products,
                attributes.Select(x => x.AccountId));
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<ProductAttributeGetPagedListResponse>> GetPagedList(
            ProductAttributeGetPagedListRequest request,
            CancellationToken ct = default)
        {
            var response = await _productAttributesService.GetPagedListAsync(_userContext.AccountId, request, ct);

            return ReturnIfAllowed(
                response,
                Roles.Products,
                response.Attributes.Select(x => x.AccountId));
        }

        [HttpPut("Create")]
        public async Task<ActionResult<Guid>> Create(ProductAttribute attribute, CancellationToken ct = default)
        {
            attribute.AccountId = _userContext.AccountId;

            var id = await _productAttributesService.CreateAsync(_userContext.UserId, attribute, ct);

            return Created(nameof(Get), id);
        }

        [HttpPatch("Update")]
        public async Task<ActionResult> Update(ProductAttribute attribute, CancellationToken ct = default)
        {
            var oldAttribute = await _productAttributesService.GetAsync(attribute.Id, true, ct);
            if (oldAttribute == null)
            {
                return NotFound(attribute.Id);
            }

            return await ActionIfAllowed(
                () => _productAttributesService.UpdateAsync(_userContext.UserId, oldAttribute, attribute, ct),
                Roles.Products,
                oldAttribute.AccountId);
        }

        [HttpPatch("Delete")]
        public async Task<ActionResult> Delete([Required] List<Guid> ids, CancellationToken ct = default)
        {
            if (ids == null || ids.All(x => x.IsEmpty()))
            {
                return BadRequest();
            }

            var attributes = await _productAttributesService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _productAttributesService.DeleteAsync(_userContext.UserId, attributes.Select(x => x.Id), ct),
                Roles.Products,
                attributes.Select(x => x.AccountId));
        }

        [HttpPatch("Restore")]
        public async Task<ActionResult> Restore([Required] List<Guid> ids, CancellationToken ct = default)
        {
            var attributes = await _productAttributesService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _productAttributesService.RestoreAsync(_userContext.UserId, attributes.Select(x => x.Id), ct),
                Roles.Products,
                attributes.Select(x => x.AccountId));
        }
    }
}
