using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Leads.Models;
using Crm.Apps.Leads.Parameters;
using Crm.Apps.Leads.Services;
using Crm.Common.UserContext;
using Crm.Common.UserContext.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Leads.Controllers
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
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.TechnicalSupport,
            Permission.LeadsManagement)]
        public async Task<ActionResult<List<LeadComment>>> GetPagedList(LeadCommentGetPagedListParameter parameter,
            CancellationToken ct = default)
        {
            var lead = await _leadsService.GetAsync(parameter.LeadId, ct).ConfigureAwait(false);
            var comments = await _leadCommentsService.GetPagedListAsync(parameter, ct).ConfigureAwait(false);

            return ReturnIfAllowed(comments, new[] {lead.AccountId});
        }

        [HttpPost("Create")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.AccountOwning,
            Permission.LeadsManagement)]
        public async Task<ActionResult<Guid>> Create(LeadComment comment, CancellationToken ct = default)
        {
            if (comment == null)
            {
                return BadRequest();
            }

            var lead = await _leadsService.GetAsync(comment.LeadId, ct).ConfigureAwait(false);

            return await ActionIfAllowed(
                () => _leadCommentsService.CreateAsync(_userContext.UserId, comment, ct)
                , new[] {lead.AccountId}).ConfigureAwait(false);
        }

        [NonAction]
        private ActionResult<TResult> ReturnIfAllowed<TResult>(TResult result, IEnumerable<Guid> accountIds)
        {
            if (_userContext.HasAny(Permission.System, Permission.Development, Permission.Administration,
                Permission.TechnicalSupport))
            {
                return result;
            }

            var accountIdsAsArray = accountIds.ToArray();

            if (_userContext.HasAny(Permission.AccountOwning, Permission.LeadsManagement) &&
                _userContext.Belongs(accountIdsAsArray))
            {
                return result;
            }

            if (_userContext.HasAny(Permission.AccountOwning, Permission.LeadsManagement) &&
                !_userContext.Belongs(accountIdsAsArray))
            {
                return Forbid();
            }

            throw new Exception();
        }

        [NonAction]
        private async Task<ActionResult> ActionIfAllowed(Func<Task> action, IEnumerable<Guid> accountIds)
        {
            if (_userContext.HasAny(Permission.System, Permission.Development, Permission.Administration,
                Permission.TechnicalSupport))
            {
                await action().ConfigureAwait(false);

                return NoContent();
            }

            var accountIdsAsArray = accountIds.ToArray();

            if (_userContext.HasAny(Permission.AccountOwning, Permission.ProductsManagement) &&
                _userContext.Belongs(accountIdsAsArray))
            {
                await action().ConfigureAwait(false);

                return NoContent();
            }

            if (_userContext.HasAny(Permission.AccountOwning, Permission.ProductsManagement) &&
                !_userContext.Belongs(accountIdsAsArray))
            {
                return Forbid();
            }

            throw new Exception();
        }
    }
}