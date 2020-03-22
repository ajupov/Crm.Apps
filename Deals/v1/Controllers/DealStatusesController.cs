using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Infrastructure.All.Jwt;
using Ajupov.Infrastructure.All.Mvc.Attributes;
using Crm.Apps.Deals.Models;
using Crm.Apps.Deals.Services;
using Crm.Apps.Deals.v1.Requests;
using Crm.Apps.Deals.v1.Responses;
using Crm.Common.All.BaseControllers;
using Crm.Common.All.Roles;
using Crm.Common.All.Roles.Attributes;
using Crm.Common.All.UserContext;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Deals.v1.Controllers
{
    [ApiController]
    [RequestContentTypeApplicationJson]
    [ResponseContentTypeApplicationJson]
    [RequireSalesRole(JwtDefaults.AuthenticationScheme)]
    [Route("Deals/Statuses/v1")]
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

            return ReturnIfAllowed(status, Roles.Sales, status.AccountId);
        }

        [HttpPost("GetList")]
        public async Task<ActionResult<List<DealStatus>>> GetList(
            [Required] List<Guid> ids,
            CancellationToken ct = default)
        {
            var statuses = await _dealStatusesService.GetListAsync(ids, ct);

            return ReturnIfAllowed(
                statuses,
                Roles.Sales,
                statuses.Select(x => x.AccountId));
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<DealStatusGetPagedListResponse>> GetPagedList(
            DealStatusGetPagedListRequest request,
            CancellationToken ct = default)
        {
            var response = await _dealStatusesService.GetPagedListAsync(_userContext.AccountId, request, ct);

            return ReturnIfAllowed(
                response,
                Roles.Sales,
                response.Statuses.Select(x => x.AccountId));
        }

        [HttpPut("Create")]
        public async Task<ActionResult<Guid>> Create(DealStatus status, CancellationToken ct = default)
        {
            status.AccountId = _userContext.AccountId;

            var id = await _dealStatusesService.CreateAsync(_userContext.UserId, status, ct);

            return Created(nameof(Get), id);
        }

        [HttpPatch("Update")]
        public async Task<ActionResult> Update(DealStatus status, CancellationToken ct = default)
        {
            var oldStatus = await _dealStatusesService.GetAsync(status.Id, ct);
            if (oldStatus == null)
            {
                return NotFound(status.Id);
            }

            return await ActionIfAllowed(
                () => _dealStatusesService.UpdateAsync(_userContext.UserId, oldStatus, status, ct),
                Roles.Sales,
                status.AccountId, oldStatus.AccountId);
        }

        [HttpDelete("Delete")]
        public async Task<ActionResult> Delete([Required] List<Guid> ids, CancellationToken ct = default)
        {
            var attributes = await _dealStatusesService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _dealStatusesService.DeleteAsync(_userContext.UserId, attributes.Select(x => x.Id), ct),
                Roles.Sales,
                attributes.Select(x => x.AccountId));
        }

        [HttpPatch("Restore")]
        public async Task<ActionResult> Restore([Required] List<Guid> ids, CancellationToken ct = default)
        {
            var attributes = await _dealStatusesService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _dealStatusesService.RestoreAsync(_userContext.UserId, attributes.Select(x => x.Id), ct),
                Roles.Sales,
                attributes.Select(x => x.AccountId));
        }
    }
}