using System.Threading;
using System.Threading.Tasks;
using Ajupov.Infrastructure.All.Api.Attributes;
using Ajupov.Infrastructure.All.Jwt;
using Crm.Apps.Deals.Models;
using Crm.Apps.Deals.Services;
using Crm.Apps.Deals.V1.Requests;
using Crm.Apps.Deals.V1.Responses;
using Crm.Common.All.BaseControllers;
using Crm.Common.All.Roles;
using Crm.Common.All.Roles.Attributes;
using Crm.Common.All.UserContext;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Deals.V1.Controllers
{
    [ApiController]
    [RequestContentTypeApplicationJson]
    [ResponseContentTypeApplicationJson]
    [RequireDealsRole(JwtDefaults.AuthenticationScheme)]
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
        public async Task<ActionResult<DealCommentGetPagedListResponse>> GetPagedList(
            DealCommentGetPagedListRequest request,
            CancellationToken ct = default)
        {
            var deal = await _dealsService.GetAsync(request.DealId, false, ct);
            var response = await _dealCommentsService.GetPagedListAsync(request, ct);

            return ReturnIfAllowed(response, Roles.Deals, deal.AccountId);
        }

        [HttpPut("Create")]
        public async Task<ActionResult> Create(DealComment comment, CancellationToken ct = default)
        {
            var deal = await _dealsService.GetAsync(comment.DealId, false, ct);

            return await ActionIfAllowed(
                () => _dealCommentsService.CreateAsync(_userContext.UserId, comment, ct),
                Roles.Deals,
                deal.AccountId);
        }
    }
}
