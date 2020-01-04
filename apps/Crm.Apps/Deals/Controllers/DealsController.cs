using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Utils.All.Enums;
using Crm.Apps.Deals.Models;
using Crm.Apps.Deals.RequestParameters;
using Crm.Apps.Deals.Services;
using Crm.Common.UserContext;
using Crm.Common.UserContext.Attributes;
using Crm.Common.UserContext.BaseControllers;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Deals.Controllers
{
    [ApiController]
    [RequirePrivileged(Role.AccountOwning, Role.SalesManagement)]
    [Route("Api/Deals")]
    public class DealsController : AllowingCheckControllerBase
    {
        private readonly IUserContext _userContext;
        private readonly IDealsService _dealsService;

        public DealsController(IUserContext userContext, IDealsService dealsService)
            : base(userContext)
        {
            _userContext = userContext;
            _dealsService = dealsService;
        }

        [HttpGet("GetTypes")]
        public Dictionary<string, DealType> GetTypes()
        {
            return EnumsExtensions.GetAsDictionary<DealType>();
        }

        [HttpGet("Get")]
        public async Task<ActionResult<Deal>> Get([Required] Guid id, CancellationToken ct = default)
        {
            var deal = await _dealsService.GetAsync(id, ct);
            if (deal == null)
            {
                return NotFound(id);
            }

            return ReturnIfAllowed(deal, new[] {Role.AccountOwning, Role.SalesManagement}, deal.AccountId);
        }

        [HttpPost("GetList")]
        public async Task<ActionResult<List<Deal>>> GetList([Required] List<Guid> ids, CancellationToken ct = default)
        {
            var deals = await _dealsService.GetListAsync(ids, ct);

            return ReturnIfAllowed(
                deals,
                new[] {Role.AccountOwning, Role.SalesManagement},
                deals.Select(x => x.AccountId));
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<List<Deal>>> GetPagedList(
            DealGetPagedListRequestParameter request,
            CancellationToken ct = default)
        {
            request.AccountId = _userContext.AccountId;

            var deals = await _dealsService.GetPagedListAsync(request, ct);

            return ReturnIfAllowed(
                deals,
                new[] {Role.AccountOwning, Role.SalesManagement},
                deals.Select(x => x.AccountId));
        }

        [HttpPost("Create")]
        public async Task<ActionResult<Guid>> Create(Deal deal, CancellationToken ct = default)
        {
            deal.AccountId = _userContext.AccountId;

            var id = await _dealsService.CreateAsync(_userContext.UserId, deal, ct);

            return Created(nameof(Get), id);
        }

        [HttpPost("Update")]
        public async Task<ActionResult> Update(Deal deal, CancellationToken ct = default)
        {
            var oldDeal = await _dealsService.GetAsync(deal.Id, ct);
            if (oldDeal == null)
            {
                return NotFound(deal.Id);
            }

            return await ActionIfAllowed(
                () => _dealsService.UpdateAsync(_userContext.UserId, oldDeal, deal, ct),
                new[] {Role.AccountOwning, Role.SalesManagement},
                deal.AccountId, oldDeal.AccountId);
        }

        [HttpPost("Delete")]
        public async Task<ActionResult> Delete([Required] List<Guid> ids, CancellationToken ct = default)
        {
            var deals = await _dealsService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _dealsService.DeleteAsync(_userContext.UserId, deals.Select(x => x.Id), ct),
                new[] {Role.AccountOwning, Role.SalesManagement},
                deals.Select(x => x.AccountId));
        }

        [HttpPost("Restore")]
        public async Task<ActionResult> Restore([Required] List<Guid> ids, CancellationToken ct = default)
        {
            var deals = await _dealsService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _dealsService.RestoreAsync(_userContext.UserId, deals.Select(x => x.Id), ct),
                new[] {Role.AccountOwning, Role.SalesManagement},
                deals.Select(x => x.AccountId));
        }
    }
}