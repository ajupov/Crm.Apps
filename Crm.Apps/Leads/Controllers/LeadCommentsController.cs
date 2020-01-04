using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Leads.Models;
using Crm.Apps.Leads.RequestParameters;
using Crm.Apps.Leads.Services;
using Crm.Common.All.UserContext;
using Crm.Common.All.UserContext.Attributes;
using Crm.Common.All.UserContext.BaseControllers;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Leads.Controllers
{
    [ApiController]
    [RequirePrivileged(Role.AccountOwning, Role.LeadsManagement)]
    [Route("Api/Leads/Comments")]
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
        public async Task<ActionResult<List<LeadComment>>> GetPagedList(
            LeadCommentGetPagedListRequestParameter request,
            CancellationToken ct = default)
        {
            var lead = await _leadsService.GetAsync(request.LeadId, ct);
            var comments = await _leadCommentsService.GetPagedListAsync(request, ct);

            return ReturnIfAllowed(comments, new[] {Role.AccountOwning, Role.LeadsManagement}, lead.AccountId);
        }

        [HttpPost("Create")]
        public async Task<ActionResult> Create(LeadComment comment, CancellationToken ct = default)
        {
            var lead = await _leadsService.GetAsync(comment.LeadId, ct);

            return await ActionIfAllowed(
                () => _leadCommentsService.CreateAsync(_userContext.UserId, comment, ct),
                new[] {Role.AccountOwning, Role.LeadsManagement},
                lead.AccountId);
        }
    }
}