using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Infrastructure.All.Jwt;
using Crm.Apps.Products.Models;
using Crm.Apps.Products.RequestParameters;
using Crm.Apps.Products.Services;
using Crm.Common.All.BaseControllers;
using Crm.Common.All.Roles;
using Crm.Common.All.Roles.Attributes;
using Crm.Common.All.UserContext;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Products.Controllers
{
    [ApiController]
    [RequireProductsRole(JwtDefaults.AuthenticationScheme)]
    [Route("Api/Products/Categories")]
    public class ProductCategoriesController : AllowingCheckControllerBase
    {
        private readonly IUserContext _userContext;
        private readonly IProductCategoriesService _userCategoriesService;

        public ProductCategoriesController(IUserContext userContext, IProductCategoriesService userCategoriesService)
            : base(userContext)
        {
            _userContext = userContext;
            _userCategoriesService = userCategoriesService;
        }

        [HttpGet("Get")]
        public async Task<ActionResult<ProductCategory>> Get([Required] Guid id, CancellationToken ct = default)
        {
            var category = await _userCategoriesService.GetAsync(id, ct);
            if (category == null)
            {
                return NotFound(id);
            }

            return ReturnIfAllowed(category, Roles.Products, category.AccountId);
        }

        [HttpPost("GetList")]
        public async Task<ActionResult<List<ProductCategory>>> GetList([Required] List<Guid> ids,
            CancellationToken ct = default)
        {
            var categories = await _userCategoriesService.GetListAsync(ids, ct);

            return ReturnIfAllowed(
                categories,
                Roles.Products,
                categories.Select(x => x.AccountId));
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<List<ProductCategory>>> GetPagedList(
            ProductCategoryGetPagedListRequestParameter request,
            CancellationToken ct = default)
        {
            request.AccountId = _userContext.AccountId;

            var categories = await _userCategoriesService.GetPagedListAsync(request, ct);

            return ReturnIfAllowed(
                categories,
                Roles.Products,
                categories.Select(x => x.AccountId));
        }

        [HttpPost("Create")]
        public async Task<ActionResult<Guid>> Create(ProductCategory category, CancellationToken ct = default)
        {
            category.AccountId = _userContext.AccountId;

            var id = await _userCategoriesService.CreateAsync(_userContext.UserId, category, ct);

            return Created(nameof(Get), id);
        }

        [HttpPost("Update")]
        public async Task<ActionResult> Update(ProductCategory category, CancellationToken ct = default)
        {
            var oldCategory = await _userCategoriesService.GetAsync(category.Id, ct);
            if (oldCategory == null)
            {
                return NotFound(category.Id);
            }

            return await ActionIfAllowed(
                () => _userCategoriesService.UpdateAsync(_userContext.UserId, oldCategory, category, ct),
                Roles.Products,
                category.AccountId, oldCategory.AccountId);
        }

        [HttpPost("Delete")]
        public async Task<ActionResult> Delete([Required] List<Guid> ids, CancellationToken ct = default)
        {
            var attributes = await _userCategoriesService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _userCategoriesService.DeleteAsync(_userContext.UserId, attributes.Select(x => x.Id), ct),
                Roles.Products,
                attributes.Select(x => x.AccountId));
        }

        [HttpPost("Restore")]
        public async Task<ActionResult> Restore([Required] List<Guid> ids, CancellationToken ct = default)
        {
            var attributes = await _userCategoriesService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _userCategoriesService.RestoreAsync(_userContext.UserId, attributes.Select(x => x.Id), ct),
                Roles.Products,
                attributes.Select(x => x.AccountId));
        }
    }
}