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
    [Route("Stock/Balances/v1")]
    public class StockBalancesController : AllowingCheckControllerBase
    {
        private readonly IUserContext _userContext;
        private readonly IStockBalancesService _stockBalancesService;

        public StockBalancesController(IUserContext userContext, IStockBalancesService stockBalancesService)
            : base(userContext)
        {
            _userContext = userContext;
            _stockBalancesService = stockBalancesService;
        }

        [HttpGet("Get")]
        public async Task<ActionResult<StockBalance>> Get([Required] Guid id, CancellationToken ct = default)
        {
            var stockBalance = await _stockBalancesService.GetAsync(id, false, ct);
            if (stockBalance == null)
            {
                return NotFound(id);
            }

            return ReturnIfAllowed(stockBalance, Roles.Stock, stockBalance.AccountId);
        }

        [HttpPost("GetList")]
        public async Task<ActionResult<List<StockBalance>>> GetList(
            [Required] List<Guid> ids,
            CancellationToken ct = default)
        {
            var stockBalances = await _stockBalancesService.GetListAsync(ids, ct);

            return ReturnIfAllowed(stockBalances, Roles.Stock, stockBalances.Select(x => x.AccountId));
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<StockBalanceGetPagedListResponse>> GetPagedList(
            StockBalanceGetPagedListRequest request,
            CancellationToken ct = default)
        {
            var response = await _stockBalancesService.GetPagedListAsync(_userContext.AccountId, request, ct);

            return ReturnIfAllowed(response, Roles.Stock, response.Balances.Select(x => x.AccountId));
        }

        [HttpPost("Create")]
        public async Task<ActionResult<Guid>> Create(StockBalance balance, CancellationToken ct = default)
        {
            balance.AccountId = _userContext.AccountId;

            var id = await _stockBalancesService.CreateAsync(_userContext.UserId, balance, ct);

            return Created(nameof(Get), id);
        }

        [HttpPatch("Update")]
        public async Task<ActionResult> Update(StockBalance balance, CancellationToken ct = default)
        {
            var oldBalance = await _stockBalancesService.GetAsync(balance.Id, true, ct);
            if (oldBalance == null)
            {
                return NotFound(balance.Id);
            }

            return await ActionIfAllowed(
                () => _stockBalancesService.UpdateAsync(_userContext.UserId, oldBalance, balance, ct),
                Roles.Stock,
                oldBalance.AccountId);
        }

        [HttpPatch("Delete")]
        public async Task<ActionResult> Delete([Required] List<Guid> ids, CancellationToken ct = default)
        {
            var balances = await _stockBalancesService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _stockBalancesService.DeleteAsync(_userContext.UserId, balances.Select(x => x.Id), ct),
                Roles.Stock,
                balances.Select(x => x.AccountId));
        }

        [HttpPatch("Restore")]
        public async Task<ActionResult> Restore([Required] List<Guid> ids, CancellationToken ct = default)
        {
            var balances = await _stockBalancesService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _stockBalancesService.RestoreAsync(_userContext.UserId, balances.Select(x => x.Id), ct),
                Roles.Stock,
                balances.Select(x => x.AccountId));
        }
    }
}
