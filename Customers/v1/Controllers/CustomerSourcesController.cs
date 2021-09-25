using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Infrastructure.All.Api.Attributes;
using Ajupov.Infrastructure.All.Jwt;
using Crm.Apps.Customers.Models;
using Crm.Apps.Customers.Services;
using Crm.Apps.Customers.V1.Requests;
using Crm.Apps.Customers.V1.Responses;
using Crm.Common.All.BaseControllers;
using Crm.Common.All.Roles;
using Crm.Common.All.Roles.Attributes;
using Crm.Common.All.UserContext;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Customers.V1.Controllers
{
    [ApiController]
    [RequestContentTypeApplicationJson]
    [ResponseContentTypeApplicationJson]
    [RequireCustomersRole(JwtDefaults.AuthenticationScheme)]
    [Route("Customers/Sources/v1")]
    public class CustomerSourcesController : AllowingCheckControllerBase
    {
        private readonly IUserContext _userContext;
        private readonly ICustomerSourcesService _customerSourcesService;

        public CustomerSourcesController(IUserContext userContext, ICustomerSourcesService customerSourcesService)
            : base(userContext)
        {
            _userContext = userContext;
            _customerSourcesService = customerSourcesService;
        }

        [HttpGet("Get")]
        public async Task<ActionResult<CustomerSource>> Get([Required] Guid id, CancellationToken ct = default)
        {
            var source = await _customerSourcesService.GetAsync(id, false, ct);
            if (source == null)
            {
                return NotFound(id);
            }

            return ReturnIfAllowed(source, Roles.Customers, source.AccountId);
        }

        [HttpPost("GetList")]
        public async Task<ActionResult<List<CustomerSource>>> GetList(
            [Required] List<Guid> ids,
            CancellationToken ct = default)
        {
            var sources = await _customerSourcesService.GetListAsync(ids, ct);

            return ReturnIfAllowed(
                sources,
                Roles.Customers,
                sources.Select(x => x.AccountId));
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<CustomerSourceGetPagedListResponse>> GetPagedList(
            CustomerSourceGetPagedListRequest request,
            CancellationToken ct = default)
        {
            var response = await _customerSourcesService.GetPagedListAsync(_userContext.AccountId, request, ct);

            return ReturnIfAllowed(
                response,
                Roles.Customers,
                response.Sources.Select(x => x.AccountId));
        }

        [HttpPost("Create")]
        public async Task<ActionResult<Guid>> Create(CustomerSource source, CancellationToken ct = default)
        {
            source.AccountId = _userContext.AccountId;

            var id = await _customerSourcesService.CreateAsync(_userContext.UserId, source, ct);

            return Created(nameof(Get), id);
        }

        [HttpPatch("Update")]
        public async Task<ActionResult> Update(CustomerSource source, CancellationToken ct = default)
        {
            var oldSource = await _customerSourcesService.GetAsync(source.Id, true, ct);
            if (oldSource == null)
            {
                return NotFound(source.Id);
            }

            return await ActionIfAllowed(
                () => _customerSourcesService.UpdateAsync(_userContext.UserId, oldSource, source, ct),
                Roles.Customers,
                oldSource.AccountId);
        }

        [HttpPatch("Delete")]
        public async Task<ActionResult> Delete([Required] List<Guid> ids, CancellationToken ct = default)
        {
            var sources = await _customerSourcesService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _customerSourcesService.DeleteAsync(_userContext.UserId, sources.Select(x => x.Id), ct),
                Roles.Customers,
                sources.Select(x => x.AccountId));
        }

        [HttpPatch("Restore")]
        public async Task<ActionResult> Restore([Required] List<Guid> ids, CancellationToken ct = default)
        {
            var sources = await _customerSourcesService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _customerSourcesService.RestoreAsync(_userContext.UserId, sources.Select(x => x.Id), ct),
                Roles.Customers,
                sources.Select(x => x.AccountId));
        }
    }
}
