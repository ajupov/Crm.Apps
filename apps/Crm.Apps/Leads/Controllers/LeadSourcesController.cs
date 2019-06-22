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
using Crm.Utils.Guid;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Leads.Controllers
{
    [ApiController]
    [Route("Api/Leads/Sources")]
    public class LeadSourcesController : ControllerBase
    {
        private readonly IUserContext _userContext;
        private readonly ILeadSourcesService _leadSourcesService;

        public LeadSourcesController(IUserContext userContext, ILeadSourcesService leadSourcesService)
        {
            _userContext = userContext;
            _leadSourcesService = leadSourcesService;
        }

        [HttpGet("Get")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.TechnicalSupport,
            Permission.AccountOwning, Permission.LeadsManagement)]
        public async Task<ActionResult<LeadSource>> Get(Guid id, CancellationToken ct = default)
        {
            if (id.IsEmpty())
            {
                return BadRequest();
            }

            var group = await _leadSourcesService.GetAsync(id, ct).ConfigureAwait(false);
            if (group == null)
            {
                return NotFound();
            }

            return ReturnIfAllowed(group, new[] {group.AccountId});
        }

        [HttpPost("GetList")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.TechnicalSupport,
            Permission.AccountOwning, Permission.LeadsManagement)]
        public async Task<ActionResult<List<LeadSource>>> GetList(List<Guid> ids, CancellationToken ct = default)
        {
            if (ids == null || ids.All(x => x.IsEmpty()))
            {
                return BadRequest();
            }

            var groups = await _leadSourcesService.GetListAsync(ids, ct).ConfigureAwait(false);

            return ReturnIfAllowed(groups, groups.Select(x => x.AccountId));
        }

        [HttpPost("GetPagedList")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.TechnicalSupport,
            Permission.AccountOwning, Permission.LeadsManagement)]
        public async Task<ActionResult<List<LeadSource>>> GetPagedList(
            LeadSourceGetPagedListParameter parameter,
            CancellationToken ct = default)
        {
            var groups = await _leadSourcesService.GetPagedListAsync(parameter, ct).ConfigureAwait(false);

            return ReturnIfAllowed(groups, groups.Select(x => x.AccountId));
        }

        [HttpPost("Create")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.AccountOwning,
            Permission.LeadsManagement)]
        public async Task<ActionResult<Guid>> Create(LeadSource group, CancellationToken ct = default)
        {
            if (group == null)
            {
                return BadRequest();
            }

            if (!_userContext.HasAny(Permission.System, Permission.Development, Permission.Administration,
                Permission.TechnicalSupport))
            {
                group.AccountId = _userContext.AccountId;
            }

            var id = await _leadSourcesService.CreateAsync(_userContext.UserId, group, ct).ConfigureAwait(false);

            return Created(nameof(Get), id);
        }

        [HttpPost("Update")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.TechnicalSupport,
            Permission.LeadsManagement)]
        public async Task<ActionResult> Update(LeadSource source, CancellationToken ct = default)
        {
            if (source.Id.IsEmpty())
            {
                return BadRequest();
            }

            var oldSource = await _leadSourcesService.GetAsync(source.Id, ct).ConfigureAwait(false);
            if (oldSource == null)
            {
                return NotFound();
            }

            return await ActionIfAllowed(() => _leadSourcesService.UpdateAsync(_userContext.UserId, oldSource,
                    source, ct),
                new[] {source.AccountId, oldSource.AccountId}).ConfigureAwait(false);
        }

        [HttpPost("Delete")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.TechnicalSupport,
            Permission.AccountOwning, Permission.LeadsManagement)]
        public async Task<ActionResult> Delete(List<Guid> ids, CancellationToken ct = default)
        {
            if (ids == null || ids.All(x => x.IsEmpty()))
            {
                return BadRequest();
            }

            var attributes = await _leadSourcesService.GetListAsync(ids, ct).ConfigureAwait(false);

            return await ActionIfAllowed(
                () => _leadSourcesService.DeleteAsync(_userContext.UserId, attributes.Select(x => x.Id), ct),
                attributes.Select(x => x.AccountId)).ConfigureAwait(false);
        }

        [HttpPost("Restore")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.TechnicalSupport,
            Permission.AccountOwning, Permission.LeadsManagement)]
        public async Task<ActionResult> Restore(List<Guid> ids, CancellationToken ct = default)
        {
            if (ids == null || ids.All(x => x.IsEmpty()))
            {
                return BadRequest();
            }

            var attributes = await _leadSourcesService.GetListAsync(ids, ct).ConfigureAwait(false);

            return await ActionIfAllowed(
                () => _leadSourcesService.RestoreAsync(_userContext.UserId, attributes.Select(x => x.Id), ct),
                attributes.Select(x => x.AccountId)).ConfigureAwait(false);
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

            if (_userContext.HasAny(Permission.AccountOwning, Permission.LeadsManagement) &&
                _userContext.Belongs(accountIdsAsArray))
            {
                await action().ConfigureAwait(false);

                return NoContent();
            }

            if (_userContext.HasAny(Permission.AccountOwning, Permission.LeadsManagement) &&
                !_userContext.Belongs(accountIdsAsArray))
            {
                return Forbid();
            }

            throw new Exception();
        }
    }
}