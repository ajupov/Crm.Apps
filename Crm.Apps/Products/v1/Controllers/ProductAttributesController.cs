using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Infrastructure.All.Jwt;
using Ajupov.Utils.All.Enums;
using Ajupov.Utils.All.Guid;
using Crm.Apps.Products.Services;
using Crm.Apps.Products.v1.Models;
using Crm.Apps.Products.v1.RequestParameters;
using Crm.Common.All.BaseControllers;
using Crm.Common.All.Roles;
using Crm.Common.All.Roles.Attributes;
using Crm.Common.All.Types.AttributeType;
using Crm.Common.All.UserContext;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Products.v1.Controllers
{
    [ApiController]
    [RequireProductsRole(JwtDefaults.AuthenticationScheme)]
    [Route("api/v1/Products/Attributes")]
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
        public Dictionary<string, AttributeType> GetTypes()
        {
            return EnumsExtensions.GetAsDictionary<AttributeType>();
        }

        [HttpGet("Get")]
        public async Task<ActionResult<ProductAttribute>> Get([Required] Guid id, CancellationToken ct = default)
        {
            var attribute = await _productAttributesService.GetAsync(id, ct);
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
        public async Task<ActionResult<List<ProductAttribute>>> GetPagedList(
            ProductAttributeGetPagedListRequestParameter request,
            CancellationToken ct = default)
        {
            request.AccountId = _userContext.AccountId;

            var attributes = await _productAttributesService.GetPagedListAsync(request, ct);

            return ReturnIfAllowed(
                attributes,
                Roles.Products,
                attributes.Select(x => x.AccountId));
        }

        [HttpPost("Create")]
        public async Task<ActionResult<Guid>> Create(ProductAttribute attribute, CancellationToken ct = default)
        {
            attribute.AccountId = _userContext.AccountId;

            var id = await _productAttributesService.CreateAsync(_userContext.UserId, attribute, ct);

            return Created(nameof(Get), id);
        }

        [HttpPost("Update")]
        public async Task<ActionResult> Update(ProductAttribute attribute, CancellationToken ct = default)
        {
            var oldAttribute = await _productAttributesService.GetAsync(attribute.Id, ct);
            if (oldAttribute == null)
            {
                return NotFound(attribute.Id);
            }

            return await ActionIfAllowed(
                () => _productAttributesService.UpdateAsync(_userContext.UserId, oldAttribute, attribute, ct),
                Roles.Products,
                attribute.AccountId, oldAttribute.AccountId);
        }

        [HttpPost("Delete")]
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

        [HttpPost("Restore")]
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