using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Products.Models;
using Crm.Apps.Areas.Products.Parameters;
using Crm.Apps.Areas.Products.Services;
using Crm.Common.Types;
using Crm.Common.UserContext;
using Crm.Common.UserContext.Attributes;
using Crm.Utils.Enums;
using Crm.Utils.Guid;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Areas.Products.Controllers
{
    [ApiController]
    [Route("Api/Products/Attributes")]
    public class ProductAttributesController : ControllerBase
    {
        private readonly IUserContext _userContext;
        private readonly IProductAttributesService _productAttributesService;

        public ProductAttributesController(IUserContext userContext, IProductAttributesService productAttributesService)
        {
            _userContext = userContext;
            _productAttributesService = productAttributesService;
        }

        [HttpGet("GetTypes")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.TechnicalSupport)]
        public ActionResult<List<AttributeType>> GetTypes()
        {
            return EnumsExtensions.GetValues<AttributeType>().ToList();
        }

        [HttpGet("Get")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.TechnicalSupport,
            Permission.AccountOwning, Permission.ProductsManagement)]
        public async Task<ActionResult<ProductAttribute>> Get(Guid id, CancellationToken ct = default)
        {
            if (id.IsEmpty())
            {
                return BadRequest();
            }

            var attribute = await _productAttributesService.GetAsync(id, ct).ConfigureAwait(false);
            if (attribute == null)
            {
                return NotFound();
            }

            return ReturnIfAllowed(attribute, new[] {attribute.AccountId});
        }

        [HttpPost("GetList")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.TechnicalSupport,
            Permission.AccountOwning, Permission.ProductsManagement)]
        public async Task<ActionResult<List<ProductAttribute>>> GetList(List<Guid> ids, CancellationToken ct = default)
        {
            if (ids == null || ids.All(x => x.IsEmpty()))
            {
                return BadRequest();
            }

            var attributes = await _productAttributesService.GetListAsync(ids, ct).ConfigureAwait(false);

            return ReturnIfAllowed(attributes, attributes.Select(x => x.AccountId));
        }

        [HttpPost("GetPagedList")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.TechnicalSupport,
            Permission.AccountOwning, Permission.ProductsManagement)]
        public async Task<ActionResult<List<ProductAttribute>>> GetPagedList(
            ProductAttributeGetPagedListParameter parameter,
            CancellationToken ct = default)
        {
            var attributes = await _productAttributesService.GetPagedListAsync(parameter, ct).ConfigureAwait(false);

            return ReturnIfAllowed(attributes, attributes.Select(x => x.AccountId));
        }

        [HttpPost("Create")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.AccountOwning,
            Permission.ProductsManagement)]
        public async Task<ActionResult<Guid>> Create(ProductAttribute attribute, CancellationToken ct = default)
        {
            if (attribute == null)
            {
                return BadRequest();
            }

            if (!_userContext.HasAny(Permission.System, Permission.Development, Permission.Administration,
                Permission.TechnicalSupport))
            {
                attribute.AccountId = _userContext.AccountId;
            }

            var id = await _productAttributesService.CreateAsync(_userContext.UserId, attribute, ct)
                .ConfigureAwait(false);

            return Created(nameof(Get), id);
        }

        [HttpPost("Update")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.TechnicalSupport,
            Permission.ProductsManagement)]
        public async Task<ActionResult> Update(ProductAttribute attribute, CancellationToken ct = default)
        {
            if (attribute.Id.IsEmpty())
            {
                return BadRequest();
            }

            var oldAttribute = await _productAttributesService.GetAsync(attribute.Id, ct).ConfigureAwait(false);
            if (oldAttribute == null)
            {
                return NotFound();
            }

            return await ActionIfAllowed(
                () => _productAttributesService.UpdateAsync(_userContext.UserId, oldAttribute, attribute, ct),
                new[] {oldAttribute.AccountId});
        }

        [HttpPost("Delete")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.TechnicalSupport,
            Permission.AccountOwning, Permission.ProductsManagement)]
        public async Task<ActionResult> Delete(List<Guid> ids, CancellationToken ct = default)
        {
            if (ids == null || ids.All(x => x.IsEmpty()))
            {
                return BadRequest();
            }

            var attributes = await _productAttributesService.GetListAsync(ids, ct).ConfigureAwait(false);

            return await ActionIfAllowed(
                () => _productAttributesService.DeleteAsync(_userContext.UserId, attributes.Select(x => x.Id), ct),
                attributes.Select(x => x.AccountId));
        }

        [HttpPost("Restore")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.TechnicalSupport,
            Permission.AccountOwning, Permission.ProductsManagement)]
        public async Task<ActionResult> Restore(List<Guid> ids, CancellationToken ct = default)
        {
            if (ids == null || ids.All(x => x.IsEmpty()))
            {
                return BadRequest();
            }

            var attributes = await _productAttributesService.GetListAsync(ids, ct).ConfigureAwait(false);

            return await ActionIfAllowed(
                () => _productAttributesService.RestoreAsync(_userContext.UserId, attributes.Select(x => x.Id), ct),
                attributes.Select(x => x.AccountId));
        }

        [NonAction]
        private ActionResult<TResult> ReturnIfAllowed<TResult>(TResult result, IEnumerable<Guid> accountIds)
        {
            if (_userContext.HasAny(Permission.System, Permission.Development, Permission.Administration,
                Permission.TechnicalSupport))
            {
                return result;
            }

            var accountIdsAsArray = accountIds.ToArray();

            if (_userContext.HasAny(Permission.AccountOwning) && _userContext.Belongs(accountIdsAsArray))
            {
                return result;
            }

            if (_userContext.HasAny(Permission.AccountOwning) && !_userContext.Belongs(accountIdsAsArray))
            {
                return Forbid();
            }

            throw new Exception();
        }

        [NonAction]
        private async Task<ActionResult> ActionIfAllowed(Func<Task> action, IEnumerable<Guid> accountIds)
        {
            if (_userContext.HasAny(Permission.System, Permission.Development, Permission.Administration,
                Permission.TechnicalSupport))
            {
                await action().ConfigureAwait(false);

                return NoContent();
            }

            var accountIdsAsArray = accountIds.ToArray();

            if (_userContext.HasAny(Permission.AccountOwning, Permission.ProductsManagement) &&
                _userContext.Belongs(accountIdsAsArray))
            {
                await action().ConfigureAwait(false);

                return NoContent();
            }

            if (_userContext.HasAny(Permission.AccountOwning, Permission.ProductsManagement) &&
                !_userContext.Belongs(accountIdsAsArray))
            {
                return Forbid();
            }

            throw new Exception();
        }
    }
}