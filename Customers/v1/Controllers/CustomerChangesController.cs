using System.Threading;
using System.Threading.Tasks;
using Ajupov.Infrastructure.All.Api.Attributes;
using Ajupov.Infrastructure.All.Jwt;
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
    [Route("Customers/Changes/v1")]
    public class CustomerChangesController : AllowingCheckControllerBase
    {
        private readonly ICustomersService _customersService;
        private readonly ICustomerChangesService _customerChangesService;

        public CustomerChangesController(
            IUserContext userContext,
            ICustomersService customersService,
            ICustomerChangesService customerChangesService)
            : base(userContext)
        {
            _customersService = customersService;
            _customerChangesService = customerChangesService;
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<CustomerChangeGetPagedListResponse>> GetPagedList(
            CustomerChangeGetPagedListRequest request,
            CancellationToken ct = default)
        {
            var customer = await _customersService.GetAsync(request.CustomerId, false, ct);
            var response = await _customerChangesService.GetPagedListAsync(request, ct);

            return ReturnIfAllowed(response, Roles.Customers, customer.AccountId);
        }
    }
}
