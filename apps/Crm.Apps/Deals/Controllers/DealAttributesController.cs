using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Deals.Models;
using Crm.Apps.Deals.Parameters;
using Crm.Apps.Deals.Services;
using Crm.Common.Types;
using Crm.Common.UserContext;
using Crm.Common.UserContext.Attributes;
using Crm.Utils.Enums;
using Crm.Utils.Guid;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Deals.Controllers
{
    [ApiController]
    [Route("Api/Deals/Attributes")]
    public class DealAttributesController : ControllerBase
    {
        private readonly IUserContext _userContext;
        private readonly IDealAttributesService _dealAttributesService;

        public DealAttributesController(IUserContext userContext, IDealAttributesService dealAttributesService)
        {
            _userContext = userContext;
            _dealAttributesService = dealAttributesService;
        }

        [HttpGet("GetTypes")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.TechnicalSupport)]
        public ActionResult<List<AttributeType>> GetTypes()
        {
            return EnumsExtensions.GetValues<AttributeType>().ToList();
        }

        [HttpGet("Get")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.TechnicalSupport,
            Permission.AccountOwning, Permission.SalesManagement)]
        public async Task<ActionResult<DealAttribute>> Get(Guid id, CancellationToken ct = default)
        {
            if (id.IsEmpty())
            {
                return BadRequest();
            }

            var attribute = await _dealAttributesService.GetAsync(id, ct).ConfigureAwait(false);
            if (attribute == null)
            {
                return NotFound();
            }

            return ReturnIfAllowed(attribute, new[] {attribute.AccountId});
        }

        [HttpPost("GetList")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.TechnicalSupport,
            Permission.AccountOwning, Permission.SalesManagement)]
        public async Task<ActionResult<List<DealAttribute>>> GetList(List<Guid> ids, CancellationToken ct = default)
        {
            if (ids == null || ids.All(x => x.IsEmpty()))
            {
                return BadRequest();
            }

            var attributes = await _dealAttributesService.GetListAsync(ids, ct).ConfigureAwait(false);

            return ReturnIfAllowed(attributes, attributes.Select(x => x.AccountId));
        }

        [HttpPost("GetPagedList")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.TechnicalSupport,
            Permission.AccountOwning, Permission.SalesManagement)]
        public async Task<ActionResult<List<DealAttribute>>> GetPagedList(
            DealAttributeGetPagedListParameter parameter,
            CancellationToken ct = default)
        {
            var attributes = await _dealAttributesService.GetPagedListAsync(parameter, ct).ConfigureAwait(false);

            return ReturnIfAllowed(attributes, attributes.Select(x => x.AccountId));
        }

        [HttpPost("Create")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.AccountOwning,
            Permission.SalesManagement)]
        public async Task<ActionResult<Guid>> Create(DealAttribute attribute, CancellationToken ct = default)
        {
            if (attribute == null)
            {
                return BadRequest();
            }

            if (!_userContext.HasAny(Permission.System, Permission.Development, Permission.Administration,
                Permission.TechnicalSupport))
            {
                attribute.AccountId = _userContext.AccountId;
            }

            var id = await _dealAttributesService.CreateAsync(_userContext.UserId, attribute, ct)
                .ConfigureAwait(false);

            return Created(nameof(Get), id);
        }

        [HttpPost("Update")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.TechnicalSupport,
            Permission.SalesManagement)]
        public async Task<ActionResult> Update(DealAttribute attribute, CancellationToken ct = default)
        {
            if (attribute.Id.IsEmpty())
            {
                return BadRequest();
            }

            var oldAttribute = await _dealAttributesService.GetAsync(attribute.Id, ct).ConfigureAwait(false);
            if (oldAttribute == null)
            {
                return NotFound();
            }

            return await ActionIfAllowed(
                () => _dealAttributesService.UpdateAsync(_userContext.UserId, oldAttribute, attribute, ct),
                new[] {attribute.AccountId, oldAttribute.AccountId}).ConfigureAwait(false);
        }

        [HttpPost("Delete")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.TechnicalSupport,
            Permission.AccountOwning, Permission.SalesManagement)]
        public async Task<ActionResult> Delete(List<Guid> ids, CancellationToken ct = default)
        {
            if (ids == null || ids.All(x => x.IsEmpty()))
            {
                return BadRequest();
            }

            var attributes = await _dealAttributesService.GetListAsync(ids, ct).ConfigureAwait(false);

            return await ActionIfAllowed(
                () => _dealAttributesService.DeleteAsync(_userContext.UserId, attributes.Select(x => x.Id), ct),
                attributes.Select(x => x.AccountId)).ConfigureAwait(false);
        }

        [HttpPost("Restore")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.TechnicalSupport,
            Permission.AccountOwning, Permission.SalesManagement)]
        public async Task<ActionResult> Restore(List<Guid> ids, CancellationToken ct = default)
        {
            if (ids == null || ids.All(x => x.IsEmpty()))
            {
                return BadRequest();
            }

            var attributes = await _dealAttributesService.GetListAsync(ids, ct).ConfigureAwait(false);

            return await ActionIfAllowed(
                () => _dealAttributesService.RestoreAsync(_userContext.UserId, attributes.Select(x => x.Id), ct),
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

            if (_userContext.HasAny(Permission.AccountOwning) && _userContext.Belongs(accountIdsAsArray))
            {
                return result;
            }

            if (_userContext.HasAny(Permission.AccountOwning) && !_userContext.Belongs(accountIdsAsArray))
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

            if (_userContext.HasAny(Permission.AccountOwning, Permission.SalesManagement) &&
                _userContext.Belongs(accountIdsAsArray))
            {
                await action().ConfigureAwait(false);

                return NoContent();
            }

            if (_userContext.HasAny(Permission.AccountOwning, Permission.SalesManagement) &&
                !_userContext.Belongs(accountIdsAsArray))
            {
                return Forbid();
            }

            throw new Exception();
        }
    }
}