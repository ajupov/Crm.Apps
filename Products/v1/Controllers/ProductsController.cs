using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Infrastructure.All.Jwt;
using Ajupov.Infrastructure.All.Mvc.Attributes;
using Ajupov.Utils.All.Enums;
using Crm.Apps.Products.Services;
using Crm.Apps.Products.v1.Models;
using Crm.Apps.Products.v1.Requests;
using Crm.Common.All.BaseControllers;
using Crm.Common.All.Roles;
using Crm.Common.All.Roles.Attributes;
using Crm.Common.All.UserContext;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Products.v1.Controllers
{
    [ApiController]
    [RequestContentTypeApplicationJson]
    [ResponseContentTypeApplicationJson]
    [RequireProductsRole(JwtDefaults.AuthenticationScheme)]
    [Route("Products/v1")]
    public class ProductsController : AllowingCheckControllerBase
    {
        private readonly IUserContext _userContext;
        private readonly IProductsService _productsService;

        public ProductsController(IUserContext userContext, IProductsService productsService)
            : base(userContext)
        {
            _userContext = userContext;
            _productsService = productsService;
        }

        [HttpGet("GetTypes")]
        public Dictionary<string, ProductType> GetTypes()
        {
            return EnumsExtensions.GetAsDictionary<ProductType>();
        }

        [HttpGet("Get")]
        public async Task<ActionResult<Product>> Get([Required] Guid id, CancellationToken ct = default)
        {
            var product = await _productsService.GetAsync(id, ct);
            if (product == null)
            {
                return NotFound(id);
            }

            return ReturnIfAllowed(product, Roles.Products, product.AccountId);
        }

        [HttpPost("GetList")]
        public async Task<ActionResult<List<Product>>> GetList(
            [Required] List<Guid> ids,
            CancellationToken ct = default)
        {
            var products = await _productsService.GetListAsync(ids, ct);

            return ReturnIfAllowed(
                products,
                Roles.Products,
                products.Select(x => x.AccountId));
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<List<Product>>> GetPagedList(
            ProductGetPagedListRequest request,
            CancellationToken ct = default)
        {
            var products = await _productsService.GetPagedListAsync(_userContext.AccountId, request, ct);

            return ReturnIfAllowed(
                products,
                Roles.Products,
                products.Select(x => x.AccountId));
        }

        [HttpPost("Create")]
        public async Task<ActionResult<Guid>> Create(Product product, CancellationToken ct = default)
        {
            product.AccountId = _userContext.AccountId;

            var id = await _productsService.CreateAsync(_userContext.UserId, product, ct);

            return Created(nameof(Get), id);
        }

        [HttpPost("Update")]
        public async Task<ActionResult> Update(Product product, CancellationToken ct = default)
        {
            var oldProduct = await _productsService.GetAsync(product.Id, ct);
            if (oldProduct == null)
            {
                return NotFound(product.Id);
            }

            return await ActionIfAllowed(
                () => _productsService.UpdateAsync(_userContext.UserId, oldProduct, product, ct),
                Roles.Products,
                product.AccountId, oldProduct.AccountId);
        }

        [HttpPost("Hide")]
        public async Task<ActionResult> Hide([Required] List<Guid> ids, CancellationToken ct = default)
        {
            var products = await _productsService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _productsService.HideAsync(_userContext.UserId, products.Select(x => x.Id), ct),
                Roles.Products,
                products.Select(x => x.AccountId));
        }

        [HttpPost("Show")]
        public async Task<ActionResult> Show([Required] List<Guid> ids, CancellationToken ct = default)
        {
            var products = await _productsService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _productsService.ShowAsync(_userContext.UserId, products.Select(x => x.Id), ct),
                Roles.Products,
                products.Select(x => x.AccountId));
        }

        [HttpPost("Delete")]
        public async Task<ActionResult> Delete([Required] List<Guid> ids, CancellationToken ct = default)
        {
            var products = await _productsService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _productsService.DeleteAsync(_userContext.UserId, products.Select(x => x.Id), ct),
                Roles.Products,
                products.Select(x => x.AccountId));
        }

        [HttpPost("Restore")]
        public async Task<ActionResult> Restore([Required] List<Guid> ids, CancellationToken ct = default)
        {
            var products = await _productsService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _productsService.RestoreAsync(_userContext.UserId, products.Select(x => x.Id), ct),
                Roles.Products,
                products.Select(x => x.AccountId));
        }
    }
}