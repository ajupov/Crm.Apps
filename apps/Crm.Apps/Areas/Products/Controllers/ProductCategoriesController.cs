using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Products.Models;
using Crm.Apps.Areas.Products.Parameters;
using Crm.Apps.Areas.Products.Services;
using Crm.Common.UserContext;
using Crm.Common.UserContext.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Areas.Products.Controllers
{
    [ApiController]
    [Route("Api/Products/Categories")]
    public class ProductCategoriesController : ControllerBase
    {
        private readonly IUserContext _userContext;
        private readonly IProductCategoriesService _userCategoriesService;

        public ProductCategoriesController(IUserContext userContext, IProductCategoriesService userCategoriesService)
        {
            _userContext = userContext;
            _userCategoriesService = userCategoriesService;
        }

        [HttpGet("Get")]
        [RequireAny(Role.System, Role.Development, Role.Administration, Role.TechnicalSupport,
            Role.AccountOwning, Role.ProductsManagement)]
        public async Task<ActionResult<ProductCategory>> Get(Guid id, CancellationToken ct = default)
        {
            if (id.IsEmpty())
            {
                return BadRequest();
            }

            var category = await _userCategoriesService.GetAsync(id, ct);
            if (category == null)
            {
                return NotFound();
            }

            return ReturnIfAllowed(category, new[] {category.AccountId});
        }

        [HttpPost("GetList")]
        [RequireAny(Role.System, Role.Development, Role.Administration, Role.TechnicalSupport,
            Role.AccountOwning, Role.ProductsManagement)]
        public async Task<ActionResult<List<ProductCategory>>> GetList(List<Guid> ids, CancellationToken ct = default)
        {
            if (ids == null || ids.All(x => x.IsEmpty()))
            {
                return BadRequest();
            }

            var categorys = await _userCategoriesService.GetListAsync(ids, ct);

            return ReturnIfAllowed(categorys, categorys.Select(x => x.AccountId));
        }

        [HttpPost("GetPagedList")]
        [RequireAny(Role.System, Role.Development, Role.Administration, Role.TechnicalSupport,
            Role.AccountOwning, Role.ProductsManagement)]
        public async Task<ActionResult<List<ProductCategory>>> GetPagedList(
            ProductCategoryGetPagedListParameter parameter,
            CancellationToken ct = default)
        {
            var categorys = await _userCategoriesService.GetPagedListAsync(parameter, ct);

            return ReturnIfAllowed(categorys, categorys.Select(x => x.AccountId));
        }

        [HttpPost("Create")]
        [RequireAny(Role.System, Role.Development, Role.Administration, Role.AccountOwning,
            Role.ProductsManagement)]
        public async Task<ActionResult<Guid>> Create(ProductCategory category, CancellationToken ct = default)
        {
            if (category == null)
            {
                return BadRequest();
            }

            if (!_userContext.HasAny(Role.System, Role.Development, Role.Administration,
                Role.TechnicalSupport))
            {
                category.AccountId = _userContext.AccountId;
            }

            var id = await _userCategoriesService.CreateAsync(_userContext.UserId, category, ct);

            return Created(nameof(Get), id);
        }

        [HttpPost("Update")]
        [RequireAny(Role.System, Role.Development, Role.Administration, Role.TechnicalSupport,
            Role.ProductsManagement)]
        public async Task<ActionResult> Update(ProductCategory category, CancellationToken ct = default)
        {
            if (category.Id.IsEmpty())
            {
                return BadRequest();
            }

            var oldCategory = await _userCategoriesService.GetAsync(category.Id, ct);
            if (oldCategory == null)
            {
                return NotFound();
            }

            return await ActionIfAllowed(() => _userCategoriesService.UpdateAsync(_userContext.UserId, oldCategory,
                category, ct), new[] {category.AccountId, oldCategory.AccountId});
        }

        [HttpPost("Delete")]
        [RequireAny(Role.System, Role.Development, Role.Administration, Role.TechnicalSupport,
            Role.AccountOwning, Role.ProductsManagement)]
        public async Task<ActionResult> Delete(List<Guid> ids, CancellationToken ct = default)
        {
            if (ids == null || ids.All(x => x.IsEmpty()))
            {
                return BadRequest();
            }

            var attributes = await _userCategoriesService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _userCategoriesService.DeleteAsync(_userContext.UserId, attributes.Select(x => x.Id), ct),
                attributes.Select(x => x.AccountId));
        }

        [HttpPost("Restore")]
        [RequireAny(Role.System, Role.Development, Role.Administration, Role.TechnicalSupport,
            Role.AccountOwning, Role.ProductsManagement)]
        public async Task<ActionResult> Restore(List<Guid> ids, CancellationToken ct = default)
        {
            if (ids == null || ids.All(x => x.IsEmpty()))
            {
                return BadRequest();
            }

            var attributes = await _userCategoriesService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _userCategoriesService.RestoreAsync(_userContext.UserId, attributes.Select(x => x.Id), ct),
                attributes.Select(x => x.AccountId));
        }

        [NonAction]
        private ActionResult<TResult> ReturnIfAllowed<TResult>(TResult result, IEnumerable<Guid> accountIds)
        {
            if (_userContext.HasAny(Role.System, Role.Development, Role.Administration,
                Role.TechnicalSupport))
            {
                return result;
            }

            var accountIdsAsArray = accountIds.ToArray();

            if (_userContext.HasAny(Role.AccountOwning, Role.ProductsManagement) &&
                _userContext.Belongs(accountIdsAsArray))
            {
                return result;
            }

            if (_userContext.HasAny(Role.AccountOwning, Role.ProductsManagement) &&
                !_userContext.Belongs(accountIdsAsArray))
            {
                return Forbid();
            }

            throw new Exception();
        }

        [NonAction]
        private async Task<ActionResult> ActionIfAllowed(Func<Task> action, IEnumerable<Guid> accountIds)
        {
            if (_userContext.HasAny(Role.System, Role.Development, Role.Administration,
                Role.TechnicalSupport))
            {
                await action();

                return NoContent();
            }

            var accountIdsAsArray = accountIds.ToArray();

            if (_userContext.HasAny(Role.AccountOwning, Role.ProductsManagement) &&
                _userContext.Belongs(accountIdsAsArray))
            {
                await action();

                return NoContent();
            }

            if (_userContext.HasAny(Role.AccountOwning, Role.ProductsManagement) &&
                !_userContext.Belongs(accountIdsAsArray))
            {
                return Forbid();
            }

            throw new Exception();
        }
    }
}