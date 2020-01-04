using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Deals.Models;
using Crm.Apps.Deals.RequestParameters;
using Crm.Apps.Deals.Services;
using Crm.Common.All.UserContext;
using Crm.Common.All.UserContext.Attributes;
using Crm.Common.All.UserContext.BaseControllers;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Deals.Controllers
{
    [ApiController]
    [RequirePrivileged(Role.AccountOwning, Role.SalesManagement)]
    [Route("Api/Deals/Statuses")]
    public class DealStatusesController : AllowingCheckControllerBase
    {
        private readonly IUserContext _userContext;
        private readonly IDealStatusesService _dealStatusesService;

        public DealStatusesController(IUserContext userContext, IDealStatusesService dealStatusesService)
            : base(userContext)
        {
            _userContext = userContext;
            _dealStatusesService = dealStatusesService;
        }

        [HttpGet("Get")]
        public async Task<ActionResult<DealStatus>> Get([Required] Guid id, CancellationToken ct = default)
        {
            var status = await _dealStatusesService.GetAsync(id, ct);
            if (status == null)
            {
                return NotFound(id);
            }

            return ReturnIfAllowed(status, new[] {Role.AccountOwning, Role.SalesManagement}, status.AccountId);
        }

        [HttpPost("GetList")]
        public async Task<ActionResult<List<DealStatus>>> GetList(
            [Required] List<Guid> ids,
            CancellationToken ct = default)
        {
            var statuses = await _dealStatusesService.GetListAsync(ids, ct);

            return ReturnIfAllowed(
                statuses,
                new[] {Role.AccountOwning, Role.SalesManagement},
                statuses.Select(x => x.AccountId));
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<List<DealStatus>>> GetPagedList(
            DealStatusGetPagedListRequestParameter request,
            CancellationToken ct = default)
        {
            request.AccountId = _userContext.AccountId;

            var statuses = await _dealStatusesService.GetPagedListAsync(request, ct);

            return ReturnIfAllowed(
                statuses,
                new[] {Role.AccountOwning, Role.SalesManagement},
                statuses.Select(x => x.AccountId));
        }

        [HttpPost("Create")]
        public async Task<ActionResult<Guid>> Create(DealStatus status, CancellationToken ct = default)
        {
            status.AccountId = _userContext.AccountId;

            var id = await _dealStatusesService.CreateAsync(_userContext.UserId, status, ct);

            return Created(nameof(Get), id);
        }

        [HttpPost("Update")]
        public async Task<ActionResult> Update(DealStatus status, CancellationToken ct = default)
        {
            var oldStatus = await _dealStatusesService.GetAsync(status.Id, ct);
            if (oldStatus == null)
            {
                return NotFound(status.Id);
            }

            return await ActionIfAllowed(
                () => _dealStatusesService.UpdateAsync(_userContext.UserId, oldStatus, status, ct),
                new[] {Role.AccountOwning, Role.SalesManagement},
                status.AccountId, oldStatus.AccountId);
        }

        [HttpPost("Delete")]
        public async Task<ActionResult> Delete([Required] List<Guid> ids, CancellationToken ct = default)
        {
            var attributes = await _dealStatusesService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _dealStatusesService.DeleteAsync(_userContext.UserId, attributes.Select(x => x.Id), ct),
                new[] {Role.AccountOwning, Role.SalesManagement},
                attributes.Select(x => x.AccountId));
        }

        [HttpPost("Restore")]
        public async Task<ActionResult> Restore([Required] List<Guid> ids, CancellationToken ct = default)
        {
            var attributes = await _dealStatusesService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _dealStatusesService.RestoreAsync(_userContext.UserId, attributes.Select(x => x.Id), ct),
                new[] {Role.AccountOwning, Role.SalesManagement},
                attributes.Select(x => x.AccountId));
        }
    }
}