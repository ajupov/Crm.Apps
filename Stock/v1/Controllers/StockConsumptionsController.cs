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
    [Route("Stock/Consumptions/v1")]
    public class StockConsumptionsController : AllowingCheckControllerBase
    {
        private readonly IUserContext _userContext;
        private readonly IStockConsumptionsService _stockConsumptionsService;

        public StockConsumptionsController(IUserContext userContext, IStockConsumptionsService stockConsumptionsService)
            : base(userContext)
        {
            _userContext = userContext;
            _stockConsumptionsService = stockConsumptionsService;
        }

        [HttpGet("Get")]
        public async Task<ActionResult<StockConsumption>> Get([Required] Guid id, CancellationToken ct = default)
        {
            var stockConsumption = await _stockConsumptionsService.GetAsync(id, false, ct);
            if (stockConsumption == null)
            {
                return NotFound(id);
            }

            return ReturnIfAllowed(stockConsumption, Roles.Stock, stockConsumption.AccountId);
        }

        [HttpPost("GetList")]
        public async Task<ActionResult<List<StockConsumption>>> GetList(
            [Required] List<Guid> ids,
            CancellationToken ct = default)
        {
            var stockConsumptions = await _stockConsumptionsService.GetListAsync(ids, ct);

            return ReturnIfAllowed(stockConsumptions, Roles.Stock, stockConsumptions.Select(x => x.AccountId));
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<StockConsumptionGetPagedListResponse>> GetPagedList(
            StockConsumptionGetPagedListRequest request,
            CancellationToken ct = default)
        {
            var response = await _stockConsumptionsService.GetPagedListAsync(_userContext.AccountId, request, ct);

            return ReturnIfAllowed(response, Roles.Stock, response.Consumptions.Select(x => x.AccountId));
        }

        [HttpPost("Create")]
        public async Task<ActionResult<Guid>> Create(StockConsumption consumption, CancellationToken ct = default)
        {
            consumption.AccountId = _userContext.AccountId;

            var id = await _stockConsumptionsService.CreateAsync(_userContext.UserId, consumption, ct);

            return Created(nameof(Get), id);
        }

        [HttpPatch("Update")]
        public async Task<ActionResult> Update(StockConsumption consumption, CancellationToken ct = default)
        {
            var oldConsumption = await _stockConsumptionsService.GetAsync(consumption.Id, true, ct);
            if (oldConsumption == null)
            {
                return NotFound(consumption.Id);
            }

            return await ActionIfAllowed(
                () => _stockConsumptionsService.UpdateAsync(_userContext.UserId, oldConsumption, consumption, ct),
                Roles.Stock,
                oldConsumption.AccountId);
        }

        [HttpPatch("Delete")]
        public async Task<ActionResult> Delete([Required] List<Guid> ids, CancellationToken ct = default)
        {
            var consumptions = await _stockConsumptionsService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _stockConsumptionsService.DeleteAsync(_userContext.UserId, consumptions.Select(x => x.Id), ct),
                Roles.Stock,
                consumptions.Select(x => x.AccountId));
        }

        [HttpPatch("Restore")]
        public async Task<ActionResult> Restore([Required] List<Guid> ids, CancellationToken ct = default)
        {
            var consumptions = await _stockConsumptionsService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _stockConsumptionsService.RestoreAsync(_userContext.UserId, consumptions.Select(x => x.Id), ct),
                Roles.Stock,
                consumptions.Select(x => x.AccountId));
        }
    }
}
