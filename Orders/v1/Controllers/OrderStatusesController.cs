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
    [Route("Orders/Statuses/v1")]
    public class OrderStatusesController : AllowingCheckControllerBase
    {
        private readonly IUserContext _userContext;
        private readonly IOrderStatusesService _orderStatusesService;

        public OrderStatusesController(IUserContext userContext, IOrderStatusesService orderStatusesService)
            : base(userContext)
        {
            _userContext = userContext;
            _orderStatusesService = orderStatusesService;
        }

        [HttpGet("Get")]
        public async Task<ActionResult<OrderStatus>> Get([Required] Guid id, CancellationToken ct = default)
        {
            var status = await _orderStatusesService.GetAsync(id, false, ct);
            if (status == null)
            {
                return NotFound(id);
            }

            return ReturnIfAllowed(status, Roles.Orders, status.AccountId);
        }

        [HttpPost("GetList")]
        public async Task<ActionResult<List<OrderStatus>>> GetList(
            [Required] List<Guid> ids,
            CancellationToken ct = default)
        {
            var statuses = await _orderStatusesService.GetListAsync(ids, ct);

            return ReturnIfAllowed(statuses, Roles.Orders, statuses.Select(x => x.AccountId));
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<OrderStatusGetPagedListResponse>> GetPagedList(
            OrderStatusGetPagedListRequest request,
            CancellationToken ct = default)
        {
            var response = await _orderStatusesService.GetPagedListAsync(_userContext.AccountId, request, ct);

            return ReturnIfAllowed(response, Roles.Orders, response.Statuses.Select(x => x.AccountId));
        }

        [HttpPost("Create")]
        public async Task<ActionResult<Guid>> Create(OrderStatus status, CancellationToken ct = default)
        {
            status.AccountId = _userContext.AccountId;

            var id = await _orderStatusesService.CreateAsync(_userContext.UserId, status, ct);

            return Created(nameof(Get), id);
        }

        [HttpPatch("Update")]
        public async Task<ActionResult> Update(OrderStatus status, CancellationToken ct = default)
        {
            var oldStatus = await _orderStatusesService.GetAsync(status.Id, true, ct);
            if (oldStatus == null)
            {
                return NotFound(status.Id);
            }

            return await ActionIfAllowed(
                () => _orderStatusesService.UpdateAsync(_userContext.UserId, oldStatus, status, ct),
                Roles.Orders,
                oldStatus.AccountId);
        }

        [HttpPatch("Delete")]
        public async Task<ActionResult> Delete([Required] List<Guid> ids, CancellationToken ct = default)
        {
            var attributes = await _orderStatusesService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _orderStatusesService.DeleteAsync(_userContext.UserId, attributes.Select(x => x.Id), ct),
                Roles.Orders,
                attributes.Select(x => x.AccountId));
        }

        [HttpPatch("Restore")]
        public async Task<ActionResult> Restore([Required] List<Guid> ids, CancellationToken ct = default)
        {
            var attributes = await _orderStatusesService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _orderStatusesService.RestoreAsync(_userContext.UserId, attributes.Select(x => x.Id), ct),
                Roles.Orders,
                attributes.Select(x => x.AccountId));
        }
    }
}
