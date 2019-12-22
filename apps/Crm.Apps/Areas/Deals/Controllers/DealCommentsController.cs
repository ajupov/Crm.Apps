using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Deals.Models;
using Crm.Apps.Areas.Deals.RequestParameters;
using Crm.Apps.Areas.Deals.Services;
using Crm.Common.UserContext;
using Crm.Common.UserContext.Attributes;
using Crm.Common.UserContext.BaseControllers;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Areas.Deals.Controllers
{
    [ApiController]
    [RequirePrivileged(Role.AccountOwning, Role.SalesManagement)]
    [Route("Api/Deals/Comments")]
    public class DealCommentsController : AllowingCheckControllerBase
    {
        private readonly IUserContext _userContext;
        private readonly IDealsService _dealsService;
        private readonly IDealCommentsService _dealCommentsService;

        public DealCommentsController(
            IUserContext userContext,
            IDealsService dealsService,
            IDealCommentsService dealCommentsService)
            : base(userContext)
        {
            _userContext = userContext;
            _dealsService = dealsService;
            _dealCommentsService = dealCommentsService;
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<List<DealComment>>> GetPagedList(
            DealCommentGetPagedListRequestParameter request,
            CancellationToken ct = default)
        {
            var deal = await _dealsService.GetAsync(request.DealId, ct);
            var comments = await _dealCommentsService.GetPagedListAsync(request, ct);

            return ReturnIfAllowed(comments, new[] {Role.AccountOwning, Role.SalesManagement}, deal.AccountId);
        }

        [HttpPost("Create")]
        public async Task<ActionResult> Create(DealComment comment, CancellationToken ct = default)
        {
            var deal = await _dealsService.GetAsync(comment.DealId, ct);

            return await ActionIfAllowed(
                () => _dealCommentsService.CreateAsync(_userContext.UserId, comment, ct),
                new[] {Role.AccountOwning, Role.SalesManagement},
                deal.AccountId);
        }
    }
}