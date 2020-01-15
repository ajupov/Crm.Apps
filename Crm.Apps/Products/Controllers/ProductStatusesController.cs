﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Products.Models;
using Crm.Apps.Products.RequestParameters;
using Crm.Apps.Products.Roles;
using Crm.Apps.Products.Services;
using Crm.Common.All.UserContext;
using Crm.Common.All.UserContext.BaseControllers;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Products.Controllers
{
    [ApiController]
    [RequireProductsRole]
    [Route("Api/Products/Statuses")]
    public class ProductStatusesController : AllowingCheckControllerBase
    {
        private readonly IUserContext _userContext;
        private readonly IProductStatusesService _productStatusesService;

        public ProductStatusesController(IUserContext userContext, IProductStatusesService productStatusesService)
            : base(userContext)
        {
            _userContext = userContext;
            _productStatusesService = productStatusesService;
        }

        [HttpGet("Get")]
        public async Task<ActionResult<ProductStatus>> Get([Required] Guid id, CancellationToken ct = default)
        {
            var status = await _productStatusesService.GetAsync(id, ct);
            if (status == null)
            {
                return NotFound(id);
            }

            return ReturnIfAllowed(status, ProductsRoles.Value, status.AccountId);
        }

        [HttpPost("GetList")]
        public async Task<ActionResult<List<ProductStatus>>> GetList(
            [Required] List<Guid> ids,
            CancellationToken ct = default)
        {
            var statuses = await _productStatusesService.GetListAsync(ids, ct);

            return ReturnIfAllowed(
                statuses,
                ProductsRoles.Value,
                statuses.Select(x => x.AccountId));
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<List<ProductStatus>>> GetPagedList(
            ProductStatusGetPagedListRequestParameter request,
            CancellationToken ct = default)
        {
            request.AccountId = _userContext.AccountId;

            var statuses = await _productStatusesService.GetPagedListAsync(request, ct);

            return ReturnIfAllowed(
                statuses,
                ProductsRoles.Value,
                statuses.Select(x => x.AccountId));
        }

        [HttpPost("Create")]
        public async Task<ActionResult<Guid>> Create(ProductStatus status, CancellationToken ct = default)
        {
            status.AccountId = _userContext.AccountId;

            var id = await _productStatusesService.CreateAsync(_userContext.UserId, status, ct);

            return Created(nameof(Get), id);
        }

        [HttpPost("Update")]
        public async Task<ActionResult> Update(ProductStatus status, CancellationToken ct = default)
        {
            var oldStatus = await _productStatusesService.GetAsync(status.Id, ct);
            if (oldStatus == null)
            {
                return NotFound(status.Id);
            }

            return await ActionIfAllowed(
                () => _productStatusesService.UpdateAsync(_userContext.UserId, oldStatus, status, ct),
                ProductsRoles.Value,
                status.AccountId, oldStatus.AccountId);
        }

        [HttpPost("Delete")]
        public async Task<ActionResult> Delete([Required] List<Guid> ids, CancellationToken ct = default)
        {
            var attributes = await _productStatusesService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _productStatusesService.DeleteAsync(_userContext.UserId, attributes.Select(x => x.Id), ct),
                ProductsRoles.Value,
                attributes.Select(x => x.AccountId));
        }

        [HttpPost("Restore")]
        public async Task<ActionResult> Restore([Required] List<Guid> ids, CancellationToken ct = default)
        {
            var attributes = await _productStatusesService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _productStatusesService.RestoreAsync(_userContext.UserId, attributes.Select(x => x.Id), ct),
                ProductsRoles.Value,
                attributes.Select(x => x.AccountId));
        }
    }
}