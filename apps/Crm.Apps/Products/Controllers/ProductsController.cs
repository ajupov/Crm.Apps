using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Products.Models;
using Crm.Apps.Products.Parameters;
using Crm.Apps.Products.Services;
using Crm.Common.UserContext;
using Crm.Common.UserContext.Attributes;
using Crm.Utils.Enums;
using Crm.Utils.Guid;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Products.Controllers
{
    [ApiController]
    [Route("Api/Products")]
    public class ProductsController : ControllerBase
    {
        private readonly IUserContext _userContext;
        private readonly IProductsService _productsService;

        public ProductsController(IUserContext userContext, IProductsService productsService)
        {
            _userContext = userContext;
            _productsService = productsService;
        }

        [HttpGet("GetTypes")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.TechnicalSupport,
            Permission.AccountOwning, Permission.ProductsManagement)]
        public ActionResult<List<ProductType>> GetTypes()
        {
            return EnumsExtensions.GetValues<ProductType>().ToList();
        }

        [HttpGet("Get")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.TechnicalSupport,
            Permission.AccountOwning, Permission.ProductsManagement)]
        public async Task<ActionResult<Product>> Get(Guid id, CancellationToken ct = default)
        {
            if (id.IsEmpty())
            {
                return BadRequest();
            }

            var product = await _productsService.GetAsync(id, ct);
            if (product == null)
            {
                return NotFound();
            }

            return ReturnIfAllowed(product, new[] {product.AccountId});
        }

        [HttpPost("GetList")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.TechnicalSupport,
            Permission.AccountOwning, Permission.ProductsManagement)]
        public async Task<ActionResult<List<Product>>> GetList(List<Guid> ids, CancellationToken ct = default)
        {
            if (ids == null || ids.All(x => x.IsEmpty()))
            {
                return BadRequest();
            }

            var products = await _productsService.GetListAsync(ids, ct);

            return ReturnIfAllowed(products, products.Select(x => x.AccountId));
        }

        [HttpPost("GetPagedList")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.TechnicalSupport,
            Permission.AccountOwning, Permission.ProductsManagement)]
        public async Task<ActionResult<List<Product>>> GetPagedList(ProductGetPagedListParameter parameter,
            CancellationToken ct = default)
        {
            var products = await _productsService.GetPagedListAsync(parameter, ct);

            return ReturnIfAllowed(products, products.Select(x => x.AccountId));
        }

        [HttpPost("Create")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.AccountOwning,
            Permission.ProductsManagement)]
        public async Task<ActionResult<Guid>> Create(Product product, CancellationToken ct = default)
        {
            if (product == null)
            {
                return BadRequest();
            }

            if (!_userContext.HasAny(Permission.System, Permission.Development, Permission.Administration,
                Permission.TechnicalSupport))
            {
                product.AccountId = _userContext.AccountId;
            }

            var id = await _productsService.CreateAsync(_userContext.UserId, product, ct);

            return Created(nameof(Get), id);
        }

        [HttpPost("Update")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.TechnicalSupport,
            Permission.AccountOwning, Permission.ProductsManagement)]
        public async Task<ActionResult> Update(Product product, CancellationToken ct = default)
        {
            if (product.Id.IsEmpty())
            {
                return BadRequest();
            }

            var oldProduct = await _productsService.GetAsync(product.Id, ct);
            if (oldProduct == null)
            {
                return NotFound();
            }

            return await ActionIfAllowed(
                () => _productsService.UpdateAsync(_userContext.UserId, oldProduct, product, ct),
                new[] {product.AccountId, oldProduct.AccountId});
        }

        [HttpPost("Hide")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.TechnicalSupport,
            Permission.AccountOwning, Permission.ProductsManagement)]
        public async Task<ActionResult> Hide(List<Guid> ids, CancellationToken ct = default)
        {
            if (ids == null || ids.All(x => x.IsEmpty()))
            {
                return BadRequest();
            }

            var products = await _productsService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _productsService.HideAsync(_userContext.UserId, products.Select(x => x.Id), ct),
                products.Select(x => x.AccountId));
        }

        [HttpPost("Show")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.TechnicalSupport,
            Permission.AccountOwning, Permission.ProductsManagement)]
        public async Task<ActionResult> Show(List<Guid> ids, CancellationToken ct = default)
        {
            if (ids == null || ids.All(x => x.IsEmpty()))
            {
                return BadRequest();
            }

            var products = await _productsService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _productsService.ShowAsync(_userContext.UserId, products.Select(x => x.Id), ct),
                products.Select(x => x.AccountId));
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

            var products = await _productsService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _productsService.DeleteAsync(_userContext.UserId, products.Select(x => x.Id), ct),
                products.Select(x => x.AccountId));
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

            var products = await _productsService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _productsService.RestoreAsync(_userContext.UserId, products.Select(x => x.Id), ct),
                products.Select(x => x.AccountId));
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

            if (_userContext.HasAny(Permission.AccountOwning, Permission.ProductsManagement) &&
                _userContext.Belongs(accountIdsAsArray))
            {
                return result;
            }

            if (_userContext.HasAny(Permission.AccountOwning, Permission.ProductsManagement) &&
                !_userContext.Belongs(accountIdsAsArray))
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
                await action();

                return NoContent();
            }

            var accountIdsAsArray = accountIds.ToArray();

            if (_userContext.HasAny(Permission.AccountOwning, Permission.ProductsManagement) &&
                _userContext.Belongs(accountIdsAsArray))
            {
                await action();

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