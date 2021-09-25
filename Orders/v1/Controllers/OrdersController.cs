using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Infrastructure.All.Api.Attributes;
using Ajupov.Infrastructure.All.Jwt;
using Crm.Apps.Orders.Models;
using Crm.Apps.Orders.Services;
using Crm.Apps.Orders.V1.Requests;
using Crm.Apps.Orders.V1.Responses;
using Crm.Common.All.BaseControllers;
using Crm.Common.All.Roles;
using Crm.Common.All.Roles.Attributes;
using Crm.Common.All.UserContext;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Orders.V1.Controllers
{
    [ApiController]
    [RequestContentTypeApplicationJson]
    [ResponseContentTypeApplicationJson]
    [RequireOrdersRole(JwtDefaults.AuthenticationScheme)]
    [Route("Orders/v1")]
    public class OrdersController : AllowingCheckControllerBase
    {
        private readonly IUserContext _userContext;
        private readonly IOrdersService _ordersService;

        public OrdersController(IUserContext userContext, IOrdersService ordersService)
            : base(userContext)
        {
            _userContext = userContext;
            _ordersService = ordersService;
        }

        [HttpGet("Get")]
        public async Task<ActionResult<Order>> Get([Required] Guid id, CancellationToken ct = default)
        {
            var order = await _ordersService.GetAsync(id, false, ct);
            if (order == null)
            {
                return NotFound(id);
            }

            return ReturnIfAllowed(order, Roles.Orders, order.AccountId);
        }

        [HttpPost("GetList")]
        public async Task<ActionResult<List<Order>>> GetList([Required] List<Guid> ids, CancellationToken ct = default)
        {
            var orders = await _ordersService.GetListAsync(ids, ct);

            return ReturnIfAllowed(orders, Roles.Orders, orders.Select(x => x.AccountId));
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<OrderGetPagedListResponse>> GetPagedList(
            OrderGetPagedListRequest request,
            CancellationToken ct = default)
        {
            var response = await _ordersService.GetPagedListAsync(_userContext.AccountId, request, ct);

            return ReturnIfAllowed(response, Roles.Orders, response.Orders.Select(x => x.AccountId));
        }

        [HttpPost("Create")]
        public async Task<ActionResult<Guid>> Create(Order order, CancellationToken ct = default)
        {
            order.AccountId = _userContext.AccountId;

            var id = await _ordersService.CreateAsync(_userContext.UserId, order, ct);

            return Created(nameof(Get), id);
        }

        [HttpPatch("Update")]
        public async Task<ActionResult> Update(Order order, CancellationToken ct = default)
        {
            var oldOrder = await _ordersService.GetAsync(order.Id, true, ct);
            if (oldOrder == null)
            {
                return NotFound(order.Id);
            }

            return await ActionIfAllowed(
                () => _ordersService.UpdateAsync(_userContext.UserId, oldOrder, order, ct),
                Roles.Orders,
                oldOrder.AccountId);
        }

        [HttpPatch("Delete")]
        public async Task<ActionResult> Delete([Required] List<Guid> ids, CancellationToken ct = default)
        {
            var orders = await _ordersService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _ordersService.DeleteAsync(_userContext.UserId, orders.Select(x => x.Id), ct),
                Roles.Orders,
                orders.Select(x => x.AccountId));
        }

        [HttpPatch("Restore")]
        public async Task<ActionResult> Restore([Required] List<Guid> ids, CancellationToken ct = default)
        {
            var orders = await _ordersService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _ordersService.RestoreAsync(_userContext.UserId, orders.Select(x => x.Id), ct),
                Roles.Orders,
                orders.Select(x => x.AccountId));
        }
    }
}
