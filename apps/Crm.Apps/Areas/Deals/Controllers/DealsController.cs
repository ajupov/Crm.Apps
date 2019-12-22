using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Deals.Models;
using Crm.Apps.Areas.Deals.Parameters;
using Crm.Apps.Areas.Deals.Services;
using Crm.Common.UserContext;
using Crm.Common.UserContext.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Areas.Deals.Controllers
{
    [ApiController]
    [Route("Api/Deals")]
    public class DealsController : ControllerBase
    {
        private readonly IUserContext _userContext;
        private readonly IDealsService _dealsService;

        public DealsController(IUserContext userContext, IDealsService dealsService)
        {
            _userContext = userContext;
            _dealsService = dealsService;
        }

        [HttpGet("GetTypes")]
        [RequireAny(Role.System, Role.Development, Role.Administration, Role.TechnicalSupport,
            Role.AccountOwning, Role.SalesManagement)]
        public ActionResult<List<DealType>> GetTypes()
        {
            return EnumsExtensions.GetValues<DealType>().ToList();
        }

        [HttpGet("Get")]
        [RequireAny(Role.System, Role.Development, Role.Administration, Role.TechnicalSupport,
            Role.AccountOwning, Role.SalesManagement)]
        public async Task<ActionResult<Deal>> Get(Guid id, CancellationToken ct = default)
        {
            if (id.IsEmpty())
            {
                return BadRequest();
            }

            var deal = await _dealsService.GetAsync(id, ct);
            if (deal == null)
            {
                return NotFound();
            }

            return ReturnIfAllowed(deal, new[] {deal.AccountId});
        }

        [HttpPost("GetList")]
        [RequireAny(Role.System, Role.Development, Role.Administration, Role.TechnicalSupport,
            Role.AccountOwning, Role.SalesManagement)]
        public async Task<ActionResult<List<Deal>>> GetList(List<Guid> ids, CancellationToken ct = default)
        {
            if (ids == null || ids.All(x => x.IsEmpty()))
            {
                return BadRequest();
            }

            var deals = await _dealsService.GetListAsync(ids, ct);

            return ReturnIfAllowed(deals, deals.Select(x => x.AccountId));
        }

        [HttpPost("GetPagedList")]
        [RequireAny(Role.System, Role.Development, Role.Administration, Role.TechnicalSupport,
            Role.AccountOwning, Role.SalesManagement)]
        public async Task<ActionResult<List<Deal>>> GetPagedList(DealGetPagedListParameter parameter,
            CancellationToken ct = default)
        {
            var deals = await _dealsService.GetPagedListAsync(parameter, ct);

            return ReturnIfAllowed(deals, deals.Select(x => x.AccountId));
        }

        [HttpPost("Create")]
        [RequireAny(Role.System, Role.Development, Role.Administration, Role.AccountOwning,
            Role.SalesManagement)]
        public async Task<ActionResult<Guid>> Create(Deal deal, CancellationToken ct = default)
        {
            if (deal == null)
            {
                return BadRequest();
            }

            if (!_userContext.HasAny(Role.System, Role.Development, Role.Administration,
                Role.TechnicalSupport))
            {
                deal.AccountId = _userContext.AccountId;
            }

            var id = await _dealsService.CreateAsync(_userContext.UserId, deal, ct);

            return Created(nameof(Get), id);
        }

        [HttpPost("Update")]
        [RequireAny(Role.System, Role.Development, Role.Administration, Role.TechnicalSupport,
            Role.AccountOwning, Role.SalesManagement)]
        public async Task<ActionResult> Update(Deal deal, CancellationToken ct = default)
        {
            if (deal.Id.IsEmpty())
            {
                return BadRequest();
            }

            var oldDeal = await _dealsService.GetAsync(deal.Id, ct);
            if (oldDeal == null)
            {
                return NotFound();
            }

            return await ActionIfAllowed(
                () => _dealsService.UpdateAsync(_userContext.UserId, oldDeal, deal, ct),
                new[] {deal.AccountId, oldDeal.AccountId});
        }

        [HttpPost("Delete")]
        [RequireAny(Role.System, Role.Development, Role.Administration, Role.TechnicalSupport,
            Role.AccountOwning, Role.SalesManagement)]
        public async Task<ActionResult> Delete(List<Guid> ids, CancellationToken ct = default)
        {
            if (ids == null || ids.All(x => x.IsEmpty()))
            {
                return BadRequest();
            }

            var deals = await _dealsService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _dealsService.DeleteAsync(_userContext.UserId, deals.Select(x => x.Id), ct),
                deals.Select(x => x.AccountId));
        }

        [HttpPost("Restore")]
        [RequireAny(Role.System, Role.Development, Role.Administration, Role.TechnicalSupport,
            Role.AccountOwning, Role.SalesManagement)]
        public async Task<ActionResult> Restore(List<Guid> ids, CancellationToken ct = default)
        {
            if (ids == null || ids.All(x => x.IsEmpty()))
            {
                return BadRequest();
            }

            var deals = await _dealsService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _dealsService.RestoreAsync(_userContext.UserId, deals.Select(x => x.Id), ct),
                deals.Select(x => x.AccountId));
        }

        [NonAction]
        private ActionResult<TResult> ReturnIfAllowed<TResult>(TResult result, IEnumerable<Guid> accountIds)
        {
            if (_userContext.HasAny(Role.System, Role.Development, Role.Administration,
                Role.TechnicalSupport))
            {
                return result;
            }

            var accountIdsAsArray = accountIds.ToArray();

            if (_userContext.HasAny(Role.AccountOwning, Role.SalesManagement) &&
                _userContext.Belongs(accountIdsAsArray))
            {
                return result;
            }

            if (_userContext.HasAny(Role.AccountOwning, Role.SalesManagement) &&
                !_userContext.Belongs(accountIdsAsArray))
            {
                return Forbid();
            }

            throw new Exception();
        }

        [NonAction]
        private async Task<ActionResult> ActionIfAllowed(Func<Task> action, IEnumerable<Guid> accountIds)
        {
            if (_userContext.HasAny(Role.System, Role.Development, Role.Administration,
                Role.TechnicalSupport))
            {
                await action();

                return NoContent();
            }

            var accountIdsAsArray = accountIds.ToArray();

            if (_userContext.HasAny(Role.AccountOwning, Role.SalesManagement) &&
                _userContext.Belongs(accountIdsAsArray))
            {
                await action();

                return NoContent();
            }

            if (_userContext.HasAny(Role.AccountOwning, Role.SalesManagement) &&
                !_userContext.Belongs(accountIdsAsArray))
            {
                return Forbid();
            }

            throw new Exception();
        }
    }
}