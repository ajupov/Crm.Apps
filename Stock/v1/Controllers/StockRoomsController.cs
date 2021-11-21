using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Infrastructure.All.Api.Attributes;
using Ajupov.Infrastructure.All.Jwt;
using Crm.Apps.Stock.Models;
using Crm.Apps.Stock.Services;
using Crm.Apps.Stock.V1.Requests;
using Crm.Apps.Stock.V1.Responses;
using Crm.Common.All.BaseControllers;
using Crm.Common.All.Roles;
using Crm.Common.All.Roles.Attributes;
using Crm.Common.All.UserContext;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Stock.V1.Controllers
{
    [ApiController]
    [RequestContentTypeApplicationJson]
    [ResponseContentTypeApplicationJson]
    [RequireOrdersRole(JwtDefaults.AuthenticationScheme)]
    [Route("Stock/Rooms/v1")]
    public class StockRoomsController : AllowingCheckControllerBase
    {
        private readonly IUserContext _userContext;
        private readonly IStockRoomsService _stockRoomsService;

        public StockRoomsController(IUserContext userContext, IStockRoomsService stockRoomsService)
            : base(userContext)
        {
            _userContext = userContext;
            _stockRoomsService = stockRoomsService;
        }

        [HttpGet("Get")]
        public async Task<ActionResult<StockRoom>> Get([Required] Guid id, CancellationToken ct = default)
        {
            var room = await _stockRoomsService.GetAsync(id, false, ct);
            if (room == null)
            {
                return NotFound(id);
            }

            return ReturnIfAllowed(room, Roles.Orders, room.AccountId);
        }

        [HttpPost("GetList")]
        public async Task<ActionResult<List<StockRoom>>> GetList(
            [Required] List<Guid> ids,
            CancellationToken ct = default)
        {
            var rooms = await _stockRoomsService.GetListAsync(ids, ct);

            return ReturnIfAllowed(rooms, Roles.Orders, rooms.Select(x => x.AccountId));
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<StockRoomGetPagedListResponse>> GetPagedList(
            StockRoomGetPagedListRequest request,
            CancellationToken ct = default)
        {
            var response = await _stockRoomsService.GetPagedListAsync(_userContext.AccountId, request, ct);

            return ReturnIfAllowed(response, Roles.Orders, response.Rooms.Select(x => x.AccountId));
        }

        [HttpPost("Create")]
        public async Task<ActionResult<Guid>> Create(StockRoom room, CancellationToken ct = default)
        {
            room.AccountId = _userContext.AccountId;

            var id = await _stockRoomsService.CreateAsync(_userContext.UserId, room, ct);

            return Created(nameof(Get), id);
        }

        [HttpPatch("Update")]
        public async Task<ActionResult> Update(StockRoom room, CancellationToken ct = default)
        {
            var oldType = await _stockRoomsService.GetAsync(room.Id, true, ct);
            if (oldType == null)
            {
                return NotFound(room.Id);
            }

            return await ActionIfAllowed(
                () => _stockRoomsService.UpdateAsync(_userContext.UserId, oldType, room, ct),
                Roles.Orders,
                oldType.AccountId);
        }

        [HttpPatch("Delete")]
        public async Task<ActionResult> Delete([Required] List<Guid> ids, CancellationToken ct = default)
        {
            var attributes = await _stockRoomsService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _stockRoomsService.DeleteAsync(_userContext.UserId, attributes.Select(x => x.Id), ct),
                Roles.Orders,
                attributes.Select(x => x.AccountId));
        }

        [HttpPatch("Restore")]
        public async Task<ActionResult> Restore([Required] List<Guid> ids, CancellationToken ct = default)
        {
            var attributes = await _stockRoomsService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _stockRoomsService.RestoreAsync(_userContext.UserId, attributes.Select(x => x.Id), ct),
                Roles.Orders,
                attributes.Select(x => x.AccountId));
        }
    }
}
