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
    [Route("Orders/Types/v1")]
    public class OrderTypesController : AllowingCheckControllerBase
    {
        private readonly IUserContext _userContext;
        private readonly IOrderTypesService _orderTypesService;

        public OrderTypesController(IUserContext userContext, IOrderTypesService orderTypesService)
            : base(userContext)
        {
            _userContext = userContext;
            _orderTypesService = orderTypesService;
        }

        [HttpGet("Get")]
        public async Task<ActionResult<OrderType>> Get([Required] Guid id, CancellationToken ct = default)
        {
            var type = await _orderTypesService.GetAsync(id, false, ct);
            if (type == null)
            {
                return NotFound(id);
            }

            return ReturnIfAllowed(type, Roles.Orders, type.AccountId);
        }

        [HttpPost("GetList")]
        public async Task<ActionResult<List<OrderType>>> GetList(
            [Required] List<Guid> ids,
            CancellationToken ct = default)
        {
            var types = await _orderTypesService.GetListAsync(ids, ct);

            return ReturnIfAllowed(types, Roles.Orders, types.Select(x => x.AccountId));
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<OrderTypeGetPagedListResponse>> GetPagedList(
            OrderTypeGetPagedListRequest request,
            CancellationToken ct = default)
        {
            var response = await _orderTypesService.GetPagedListAsync(_userContext.AccountId, request, ct);

            return ReturnIfAllowed(response, Roles.Orders, response.Types.Select(x => x.AccountId));
        }

        [HttpPost("Create")]
        public async Task<ActionResult<Guid>> Create(OrderType type, CancellationToken ct = default)
        {
            type.AccountId = _userContext.AccountId;

            var id = await _orderTypesService.CreateAsync(_userContext.UserId, type, ct);

            return Created(nameof(Get), id);
        }

        [HttpPatch("Update")]
        public async Task<ActionResult> Update(OrderType type, CancellationToken ct = default)
        {
            var oldType = await _orderTypesService.GetAsync(type.Id, true, ct);
            if (oldType == null)
            {
                return NotFound(type.Id);
            }

            return await ActionIfAllowed(
                () => _orderTypesService.UpdateAsync(_userContext.UserId, oldType, type, ct),
                Roles.Orders,
                oldType.AccountId);
        }

        [HttpPatch("Delete")]
        public async Task<ActionResult> Delete([Required] List<Guid> ids, CancellationToken ct = default)
        {
            var attributes = await _orderTypesService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _orderTypesService.DeleteAsync(_userContext.UserId, attributes.Select(x => x.Id), ct),
                Roles.Orders,
                attributes.Select(x => x.AccountId));
        }

        [HttpPatch("Restore")]
        public async Task<ActionResult> Restore([Required] List<Guid> ids, CancellationToken ct = default)
        {
            var attributes = await _orderTypesService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _orderTypesService.RestoreAsync(_userContext.UserId, attributes.Select(x => x.Id), ct),
                Roles.Orders,
                attributes.Select(x => x.AccountId));
        }
    }
}
