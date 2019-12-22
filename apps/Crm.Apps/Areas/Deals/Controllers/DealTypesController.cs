using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Deals.Models;
using Crm.Apps.Areas.Deals.Parameters;
using Crm.Apps.Areas.Deals.Services;
using Crm.Common.UserContext;
using Crm.Common.UserContext.Attributes;
using Crm.Common.UserContext.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Areas.Deals.Controllers
{
    [ApiController]
    [RequirePrivileged(Role.AccountOwning, Role.SalesManagement)]
    [Route("Api/Deals/Types")]
    public class DealTypesController : UserContextController
    {
        private readonly IUserContext _userContext;
        private readonly IDealTypesService _dealTypesService;

        public DealTypesController(IUserContext userContext, IDealTypesService dealTypesService)
            : base(userContext)
        {
            _userContext = userContext;
            _dealTypesService = dealTypesService;
        }

        [HttpGet("Get")]
        public async Task<ActionResult<DealType>> Get([Required] Guid id, CancellationToken ct = default)
        {
            var type = await _dealTypesService.GetAsync(id, ct);
            if (type == null)
            {
                return NotFound(id);
            }

            return ReturnIfAllowed(type, new[] {Role.AccountOwning, Role.SalesManagement}, type.AccountId);
        }

        [HttpPost("GetList")]
        public async Task<ActionResult<List<DealType>>> GetList(
            [Required] List<Guid> ids,
            CancellationToken ct = default)
        {
            var types = await _dealTypesService.GetListAsync(ids, ct);

            return ReturnIfAllowed(
                types,
                new[] {Role.AccountOwning, Role.SalesManagement},
                types.Select(x => x.AccountId));
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<List<DealType>>> GetPagedList(
            DealTypeGetPagedListParameter parameter,
            CancellationToken ct = default)
        {
            parameter.AccountId = _userContext.AccountId;

            var types = await _dealTypesService.GetPagedListAsync(parameter, ct);

            return ReturnIfAllowed(
                types,
                new[] {Role.AccountOwning, Role.SalesManagement},
                types.Select(x => x.AccountId));
        }

        [HttpPost("Create")]
        public async Task<ActionResult<Guid>> Create(DealType type, CancellationToken ct = default)
        {
            type.AccountId = _userContext.AccountId;

            var id = await _dealTypesService.CreateAsync(_userContext.UserId, type, ct);

            return Created(nameof(Get), id);
        }

        [HttpPost("Update")]
        public async Task<ActionResult> Update(DealType type, CancellationToken ct = default)
        {
            var oldType = await _dealTypesService.GetAsync(type.Id, ct);
            if (oldType == null)
            {
                return NotFound(type.Id);
            }

            return await ActionIfAllowed(
                () => _dealTypesService.UpdateAsync(_userContext.UserId, oldType, type, ct),
                new[] {Role.AccountOwning, Role.SalesManagement},
                type.AccountId, oldType.AccountId);
        }

        [HttpPost("Delete")]
        public async Task<ActionResult> Delete([Required] List<Guid> ids, CancellationToken ct = default)
        {
            var attributes = await _dealTypesService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _dealTypesService.DeleteAsync(_userContext.UserId, attributes.Select(x => x.Id), ct),
                new[] {Role.AccountOwning, Role.SalesManagement},
                attributes.Select(x => x.AccountId));
        }

        [HttpPost("Restore")]
        public async Task<ActionResult> Restore([Required] List<Guid> ids, CancellationToken ct = default)
        {
            var attributes = await _dealTypesService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _dealTypesService.RestoreAsync(_userContext.UserId, attributes.Select(x => x.Id), ct),
                new[] {Role.AccountOwning, Role.SalesManagement},
                attributes.Select(x => x.AccountId));
        }
    }
}