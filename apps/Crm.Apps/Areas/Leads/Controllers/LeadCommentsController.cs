using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Leads.Models;
using Crm.Apps.Areas.Leads.Parameters;
using Crm.Apps.Areas.Leads.Services;
using Crm.Common.UserContext;
using Crm.Common.UserContext.Attributes;
using Crm.Common.UserContext.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Areas.Leads.Controllers
{
    [ApiController]
    [RequirePrivileged(Role.AccountOwning, Role.LeadsManagement)]
    [Route("Api/Leads/Comments")]
    public class LeadCommentsController : UserContextController
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
            LeadCommentGetPagedListParameter parameter,
            CancellationToken ct = default)
        {
            var lead = await _leadsService.GetAsync(parameter.LeadId, ct);
            var comments = await _leadCommentsService.GetPagedListAsync(parameter, ct);

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