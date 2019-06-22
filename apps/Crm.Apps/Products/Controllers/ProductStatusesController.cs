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
using Crm.Utils.Guid;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Products.Controllers
{
    [ApiController]
    [Route("Api/Products/Statuses")]
    public class ProductStatusesController : ControllerBase
    {
        private readonly IUserContext _userContext;
        private readonly IProductStatusesService _productStatusesService;

        public ProductStatusesController(IUserContext userContext, IProductStatusesService productStatusesService)
        {
            _userContext = userContext;
            _productStatusesService = productStatusesService;
        }

        [HttpGet("Get")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.TechnicalSupport,
            Permission.AccountOwning, Permission.ProductsManagement)]
        public async Task<ActionResult<ProductStatus>> Get(Guid id, CancellationToken ct = default)
        {
            if (id.IsEmpty())
            {
                return BadRequest();
            }

            var status = await _productStatusesService.GetAsync(id, ct).ConfigureAwait(false);
            if (status == null)
            {
                return NotFound();
            }

            return ReturnIfAllowed(status, new[] {status.AccountId});
        }

        [HttpPost("GetList")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.TechnicalSupport,
            Permission.AccountOwning, Permission.ProductsManagement)]
        public async Task<ActionResult<List<ProductStatus>>> GetList(List<Guid> ids, CancellationToken ct = default)
        {
            if (ids == null || ids.All(x => x.IsEmpty()))
            {
                return BadRequest();
            }

            var statuss = await _productStatusesService.GetListAsync(ids, ct).ConfigureAwait(false);

            return ReturnIfAllowed(statuss, statuss.Select(x => x.AccountId));
        }

        [HttpPost("GetPagedList")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.TechnicalSupport,
            Permission.AccountOwning, Permission.ProductsManagement)]
        public async Task<ActionResult<List<ProductStatus>>> GetPagedList(
            ProductStatusGetPagedListParameter parameter,
            CancellationToken ct = default)
        {
            var statuss = await _productStatusesService.GetPagedListAsync(parameter, ct).ConfigureAwait(false);

            return ReturnIfAllowed(statuss, statuss.Select(x => x.AccountId));
        }

        [HttpPost("Create")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.AccountOwning,
            Permission.ProductsManagement)]
        public async Task<ActionResult<Guid>> Create(ProductStatus status, CancellationToken ct = default)
        {
            if (status == null)
            {
                return BadRequest();
            }

            if (!_userContext.HasAny(Permission.System, Permission.Development, Permission.Administration,
                Permission.TechnicalSupport))
            {
                status.AccountId = _userContext.AccountId;
            }

            var id = await _productStatusesService.CreateAsync(_userContext.UserId, status, ct).ConfigureAwait(false);

            return Created(nameof(Get), id);
        }

        [HttpPost("Update")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.TechnicalSupport,
            Permission.ProductsManagement)]
        public async Task<ActionResult> Update(ProductStatus status, CancellationToken ct = default)
        {
            if (status.Id.IsEmpty())
            {
                return BadRequest();
            }

            var oldStatus = await _productStatusesService.GetAsync(status.Id, ct).ConfigureAwait(false);
            if (oldStatus == null)
            {
                return NotFound();
            }

            return await ActionIfAllowed(() => _productStatusesService.UpdateAsync(_userContext.UserId, oldStatus,
                status, ct), new[] {status.AccountId, oldStatus.AccountId}).ConfigureAwait(false);
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

            var attributes = await _productStatusesService.GetListAsync(ids, ct).ConfigureAwait(false);

            return await ActionIfAllowed(
                () => _productStatusesService.DeleteAsync(_userContext.UserId, attributes.Select(x => x.Id), ct),
                attributes.Select(x => x.AccountId)).ConfigureAwait(false);
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

            var attributes = await _productStatusesService.GetListAsync(ids, ct).ConfigureAwait(false);

            return await ActionIfAllowed(
                () => _productStatusesService.RestoreAsync(_userContext.UserId, attributes.Select(x => x.Id), ct),
                attributes.Select(x => x.AccountId)).ConfigureAwait(false);
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