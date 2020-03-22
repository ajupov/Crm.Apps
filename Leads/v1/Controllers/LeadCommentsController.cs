using System.Threading;
using System.Threading.Tasks;
using Ajupov.Infrastructure.All.Jwt;
using Ajupov.Infrastructure.All.Mvc.Attributes;
using Crm.Apps.Leads.Models;
using Crm.Apps.Leads.Services;
using Crm.Apps.Leads.v1.Requests;
using Crm.Apps.Leads.v1.Responses;
using Crm.Common.All.BaseControllers;
using Crm.Common.All.Roles;
using Crm.Common.All.Roles.Attributes;
using Crm.Common.All.UserContext;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Leads.v1.Controllers
{
    [ApiController]
    [RequestContentTypeApplicationJson]
    [ResponseContentTypeApplicationJson]
    [RequireLeadsRole(JwtDefaults.AuthenticationScheme)]
    [Route("Leads/Comments/v1")]
    public class LeadCommentsController : AllowingCheckControllerBase
    {
        private readonly IUserContext _userContext;
        private readonly ILeadsService _leadsService;
        private readonly ILeadCommentsService _leadCommentsService;

        public LeadCommentsController(
            IUserContext userContext,
            ILeadsService leadsService,
            ILeadCommentsService leadCommentsService)
            : base(userContext)
        {
            _userContext = userContext;
            _leadsService = leadsService;
            _leadCommentsService = leadCommentsService;
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<LeadCommentGetPagedListResponse>> GetPagedList(
            LeadCommentGetPagedListRequest request,
            CancellationToken ct = default)
        {
            var lead = await _leadsService.GetAsync(request.LeadId, ct);
            var response = await _leadCommentsService.GetPagedListAsync(request, ct);

            return ReturnIfAllowed(response, Roles.Leads, lead.AccountId);
        }

        [HttpPut("Create")]
        public async Task<ActionResult> Create(LeadComment comment, CancellationToken ct = default)
        {
            var lead = await _leadsService.GetAsync(comment.LeadId, ct);

            return await ActionIfAllowed(
                () => _leadCommentsService.CreateAsync(_userContext.UserId, comment, ct),
                Roles.Leads,
                lead.AccountId);
        }
    }
}