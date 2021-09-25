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
    [Route("Customers/Attributes/v1")]
    public class CustomerAttributesController : AllowingCheckControllerBase
    {
        private readonly IUserContext _userContext;
        private readonly ICustomerAttributesService _customerAttributesService;

        public CustomerAttributesController(IUserContext userContext,
            ICustomerAttributesService customerAttributesService)
            : base(userContext)
        {
            _userContext = userContext;
            _customerAttributesService = customerAttributesService;
        }

        [HttpGet("Get")]
        public async Task<ActionResult<CustomerAttribute>> Get([Required] Guid id, CancellationToken ct = default)
        {
            var attribute = await _customerAttributesService.GetAsync(id, false, ct);
            if (attribute == null)
            {
                return NotFound(id);
            }

            return ReturnIfAllowed(attribute, Roles.Customers, attribute.AccountId);
        }

        [HttpPost("GetList")]
        public async Task<ActionResult<List<CustomerAttribute>>> GetList(
            [Required] List<Guid> ids,
            CancellationToken ct = default)
        {
            var attributes = await _customerAttributesService.GetListAsync(ids, ct);

            return ReturnIfAllowed(
                attributes,
                Roles.Customers,
                attributes.Select(x => x.AccountId));
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<CustomerAttributeGetPagedListResponse>> GetPagedList(
            CustomerAttributeGetPagedListRequest request,
            CancellationToken ct = default)
        {
            var response = await _customerAttributesService.GetPagedListAsync(_userContext.AccountId, request, ct);

            return ReturnIfAllowed(
                response,
                Roles.Customers,
                response.Attributes.Select(x => x.AccountId));
        }

        [HttpPost("Create")]
        public async Task<ActionResult<Guid>> Create(CustomerAttribute attribute, CancellationToken ct = default)
        {
            attribute.AccountId = _userContext.AccountId;

            var id = await _customerAttributesService.CreateAsync(_userContext.UserId, attribute, ct);

            return Created(nameof(Get), id);
        }

        [HttpPatch("Update")]
        public async Task<ActionResult> Update(CustomerAttribute attribute, CancellationToken ct = default)
        {
            var oldAttribute = await _customerAttributesService.GetAsync(attribute.Id, true, ct);
            if (oldAttribute == null)
            {
                return NotFound(attribute.Id);
            }

            return await ActionIfAllowed(
                () => _customerAttributesService.UpdateAsync(_userContext.UserId, oldAttribute, attribute, ct),
                Roles.Customers,
                oldAttribute.AccountId);
        }

        [HttpPatch("Delete")]
        public async Task<ActionResult> Delete([Required] List<Guid> ids, CancellationToken ct = default)
        {
            var attributes = await _customerAttributesService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _customerAttributesService.DeleteAsync(_userContext.UserId, attributes.Select(x => x.Id), ct),
                Roles.Customers,
                attributes.Select(x => x.AccountId));
        }

        [HttpPatch("Restore")]
        public async Task<ActionResult> Restore([Required] List<Guid> ids, CancellationToken ct = default)
        {
            var attributes = await _customerAttributesService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _customerAttributesService.RestoreAsync(_userContext.UserId, attributes.Select(x => x.Id), ct),
                Roles.Customers,
                attributes.Select(x => x.AccountId));
        }
    }
}
