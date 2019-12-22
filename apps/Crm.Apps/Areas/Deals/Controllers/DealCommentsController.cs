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
    [Route("Api/Deals/Comments")]
    public class DealCommentsController : ControllerBase
    {
        private readonly IUserContext _userContext;
        private readonly IDealsService _dealsService;
        private readonly IDealCommentsService _dealCommentsService;

        public DealCommentsController(IUserContext userContext, IDealsService dealsService,
            IDealCommentsService dealCommentsService)
        {
            _userContext = userContext;
            _dealsService = dealsService;
            _dealCommentsService = dealCommentsService;
        }

        [HttpPost("GetPagedList")]
        [RequireAny(Role.System, Role.Development, Role.Administration, Role.TechnicalSupport,
            Role.SalesManagement)]
        public async Task<ActionResult<List<DealComment>>> GetPagedList(DealCommentGetPagedListParameter parameter,
            CancellationToken ct = default)
        {
            var deal = await _dealsService.GetAsync(parameter.DealId, ct);
            var comments = await _dealCommentsService.GetPagedListAsync(parameter, ct);

            return ReturnIfAllowed(comments, new[] {deal.AccountId});
        }

        [HttpPost("Create")]
        [RequireAny(Role.System, Role.Development, Role.Administration, Role.AccountOwning,
            Role.SalesManagement)]
        public async Task<ActionResult> Create(DealComment comment, CancellationToken ct = default)
        {
            if (comment == null)
            {
                return BadRequest();
            }

            var deal = await _dealsService.GetAsync(comment.DealId, ct);

            return await ActionIfAllowed(
                () => _dealCommentsService.CreateAsync(_userContext.UserId, comment, ct)
                , new[] {deal.AccountId});
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

            if (_userContext.HasAny(Role.AccountOwning, Role.ProductsManagement) &&
                _userContext.Belongs(accountIdsAsArray))
            {
                await action();

                return NoContent();
            }

            if (_userContext.HasAny(Role.AccountOwning, Role.ProductsManagement) &&
                !_userContext.Belongs(accountIdsAsArray))
            {
                return Forbid();
            }

            throw new Exception();
        }
    }
}