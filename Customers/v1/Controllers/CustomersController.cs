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
    [Route("Customers/v1")]
    public class CustomersController : AllowingCheckControllerBase
    {
        private readonly IUserContext _userContext;
        private readonly ICustomersService _customersService;

        public CustomersController(IUserContext userContext, ICustomersService customersService)
            : base(userContext)
        {
            _userContext = userContext;
            _customersService = customersService;
        }

        [HttpGet("Get")]
        public async Task<ActionResult<Customer>> Get([Required] Guid id, CancellationToken ct = default)
        {
            var customer = await _customersService.GetAsync(id, false, ct);
            if (customer == null)
            {
                return NotFound(id);
            }

            return ReturnIfAllowed(customer, Roles.Customers, customer.AccountId);
        }

        [HttpPost("GetList")]
        public async Task<ActionResult<List<Customer>>> GetList([Required] List<Guid> ids, CancellationToken ct = default)
        {
            var customers = await _customersService.GetListAsync(ids, ct);

            return ReturnIfAllowed(
                customers,
                Roles.Customers,
                customers.Select(x => x.AccountId));
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<CustomerGetPagedListResponse>> GetPagedList(
            CustomerGetPagedListRequest request,
            CancellationToken ct = default)
        {
            var response = await _customersService.GetPagedListAsync(_userContext.AccountId, request, ct);

            return ReturnIfAllowed(
                response,
                Roles.Customers,
                response.Customers.Select(x => x.AccountId));
        }

        [HttpPost("Create")]
        public async Task<ActionResult<Guid>> Create(Customer customer, CancellationToken ct = default)
        {
            customer.AccountId = _userContext.AccountId;

            var id = await _customersService.CreateAsync(_userContext.UserId, customer, ct);

            return Created(nameof(Get), id);
        }

        [HttpPatch("Update")]
        public async Task<ActionResult> Update(Customer customer, CancellationToken ct = default)
        {
            var oldCustomer = await _customersService.GetAsync(customer.Id, true, ct);
            if (oldCustomer == null)
            {
                return NotFound(customer.Id);
            }

            return await ActionIfAllowed(
                () => _customersService.UpdateAsync(_userContext.UserId, oldCustomer, customer, ct),
                Roles.Customers,
                oldCustomer.AccountId);
        }

        [HttpPatch("Delete")]
        public async Task<ActionResult> Delete([Required] List<Guid> ids, CancellationToken ct = default)
        {
            var customers = await _customersService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _customersService.DeleteAsync(_userContext.UserId, customers.Select(x => x.Id), ct),
                Roles.Customers,
                customers.Select(x => x.AccountId));
        }

        [HttpPatch("Restore")]
        public async Task<ActionResult> Restore([Required] List<Guid> ids, CancellationToken ct = default)
        {
            var customers = await _customersService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _customersService.RestoreAsync(_userContext.UserId, customers.Select(x => x.Id), ct),
                Roles.Customers,
                customers.Select(x => x.AccountId));
        }
    }
}
