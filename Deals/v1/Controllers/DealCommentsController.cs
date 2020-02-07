using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Infrastructure.All.Jwt;
using Ajupov.Infrastructure.All.Mvc.Attributes;
using Crm.Apps.Deals.Services;
using Crm.Apps.Deals.v1.Models;
using Crm.Apps.Deals.v1.RequestParameters;
using Crm.Common.All.BaseControllers;
using Crm.Common.All.Roles;
using Crm.Common.All.Roles.Attributes;
using Crm.Common.All.UserContext;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Deals.v1.Controllers
{
    [ApiController]
    [RequestContentTypeApplicationJson]
    [ResponseContentTypeApplicationJson]
    [RequireSalesRole(JwtDefaults.AuthenticationScheme)]
    [Route("Deals/Comments/v1")]
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

            return ReturnIfAllowed(comments, Roles.Sales, deal.AccountId);
        }

        [HttpPost("Create")]
        public async Task<ActionResult> Create(DealComment comment, CancellationToken ct = default)
        {
            var deal = await _dealsService.GetAsync(comment.DealId, ct);

            return await ActionIfAllowed(
                () => _dealCommentsService.CreateAsync(_userContext.UserId, comment, ct),
                Roles.Sales,
                deal.AccountId);
        }
    }
}