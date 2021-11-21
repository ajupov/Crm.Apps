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
    [RequireStockRole(JwtDefaults.AuthenticationScheme)]
    [Route("Stock/Arrivals/v1")]
    public class StockArrivalsController : AllowingCheckControllerBase
    {
        private readonly IUserContext _userContext;
        private readonly IStockArrivalsService _stockArrivalsService;

        public StockArrivalsController(IUserContext userContext, IStockArrivalsService stockArrivalsService)
            : base(userContext)
        {
            _userContext = userContext;
            _stockArrivalsService = stockArrivalsService;
        }

        [HttpGet("Get")]
        public async Task<ActionResult<StockArrival>> Get([Required] Guid id, CancellationToken ct = default)
        {
            var stockArrival = await _stockArrivalsService.GetAsync(id, false, ct);
            if (stockArrival == null)
            {
                return NotFound(id);
            }

            return ReturnIfAllowed(stockArrival, Roles.Stock, stockArrival.AccountId);
        }

        [HttpPost("GetList")]
        public async Task<ActionResult<List<StockArrival>>> GetList(
            [Required] List<Guid> ids,
            CancellationToken ct = default)
        {
            var stockArrivals = await _stockArrivalsService.GetListAsync(ids, ct);

            return ReturnIfAllowed(stockArrivals, Roles.Stock, stockArrivals.Select(x => x.AccountId));
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<StockArrivalGetPagedListResponse>> GetPagedList(
            StockArrivalGetPagedListRequest request,
            CancellationToken ct = default)
        {
            var response = await _stockArrivalsService.GetPagedListAsync(_userContext.AccountId, request, ct);

            return ReturnIfAllowed(response, Roles.Stock, response.Arrivals.Select(x => x.AccountId));
        }

        [HttpPost("Create")]
        public async Task<ActionResult<Guid>> Create(StockArrival arrival, CancellationToken ct = default)
        {
            arrival.AccountId = _userContext.AccountId;

            var id = await _stockArrivalsService.CreateAsync(_userContext.UserId, arrival, ct);

            return Created(nameof(Get), id);
        }

        [HttpPatch("Update")]
        public async Task<ActionResult> Update(StockArrival arrival, CancellationToken ct = default)
        {
            var oldArrival = await _stockArrivalsService.GetAsync(arrival.Id, true, ct);
            if (oldArrival == null)
            {
                return NotFound(arrival.Id);
            }

            return await ActionIfAllowed(
                () => _stockArrivalsService.UpdateAsync(_userContext.UserId, oldArrival, arrival, ct),
                Roles.Stock,
                oldArrival.AccountId);
        }

        [HttpPatch("Delete")]
        public async Task<ActionResult> Delete([Required] List<Guid> ids, CancellationToken ct = default)
        {
            var arrivals = await _stockArrivalsService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _stockArrivalsService.DeleteAsync(_userContext.UserId, arrivals.Select(x => x.Id), ct),
                Roles.Stock,
                arrivals.Select(x => x.AccountId));
        }

        [HttpPatch("Restore")]
        public async Task<ActionResult> Restore([Required] List<Guid> ids, CancellationToken ct = default)
        {
            var arrivals = await _stockArrivalsService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _stockArrivalsService.RestoreAsync(_userContext.UserId, arrivals.Select(x => x.Id), ct),
                Roles.Stock,
                arrivals.Select(x => x.AccountId));
        }
    }
}
