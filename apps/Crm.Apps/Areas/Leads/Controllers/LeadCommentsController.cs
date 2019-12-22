using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Leads.Models;
using Crm.Apps.Areas.Leads.Parameters;
using Crm.Apps.Areas.Leads.Services;
using Crm.Common.UserContext;
using Crm.Common.UserContext.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Areas.Leads.Controllers
{
    [ApiController]
    [Route("Api/Leads/Comments")]
    public class LeadCommentsController : ControllerBase
    {
        private readonly IUserContext _userContext;
        private readonly ILeadsService _leadsService;
        private readonly ILeadCommentsService _leadCommentsService;

        public LeadCommentsController(IUserContext userContext, ILeadsService leadsService,
            ILeadCommentsService leadCommentsService)
        {
            _userContext = userContext;
            _leadsService = leadsService;
            _leadCommentsService = leadCommentsService;
        }

        [HttpPost("GetPagedList")]
        [RequireAny(Role.System, Role.Development, Role.Administration, Role.TechnicalSupport,
            Role.LeadsManagement)]
        public async Task<ActionResult<List<LeadComment>>> GetPagedList(LeadCommentGetPagedListParameter parameter,
            CancellationToken ct = default)
        {
            var lead = await _leadsService.GetAsync(parameter.LeadId, ct);
            var comments = await _leadCommentsService.GetPagedListAsync(parameter, ct);

            return ReturnIfAllowed(comments, new[] {lead.AccountId});
        }

        [HttpPost("Create")]
        [RequireAny(Role.System, Role.Development, Role.Administration, Role.AccountOwning,
            Role.LeadsManagement)]
        public async Task<ActionResult> Create(LeadComment comment, CancellationToken ct = default)
        {
            if (comment == null)
            {
                return BadRequest();
            }

            var lead = await _leadsService.GetAsync(comment.LeadId, ct);

            return await ActionIfAllowed(
                () => _leadCommentsService.CreateAsync(_userContext.UserId, comment, ct)
                , new[] {lead.AccountId});
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

            if (_userContext.HasAny(Role.AccountOwning, Role.LeadsManagement) &&
                _userContext.Belongs(accountIdsAsArray))
            {
                return result;
            }

            if (_userContext.HasAny(Role.AccountOwning, Role.LeadsManagement) &&
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