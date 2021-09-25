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
    [Route("Customers/Comments/v1")]
    public class CustomerCommentsController : AllowingCheckControllerBase
    {
        private readonly IUserContext _userContext;
        private readonly ICustomersService _customersService;
        private readonly ICustomerCommentsService _customerCommentsService;

        public CustomerCommentsController(
            IUserContext userContext,
            ICustomersService customersService,
            ICustomerCommentsService customerCommentsService)
            : base(userContext)
        {
            _userContext = userContext;
            _customersService = customersService;
            _customerCommentsService = customerCommentsService;
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<CustomerCommentGetPagedListResponse>> GetPagedList(
            CustomerCommentGetPagedListRequest request,
            CancellationToken ct = default)
        {
            var customer = await _customersService.GetAsync(request.CustomerId, false, ct);
            var response = await _customerCommentsService.GetPagedListAsync(request, ct);

            return ReturnIfAllowed(response, Roles.Customers, customer.AccountId);
        }

        [HttpPost("Create")]
        public async Task<ActionResult> Create(CustomerComment comment, CancellationToken ct = default)
        {
            var customer = await _customersService.GetAsync(comment.CustomerId, false, ct);

            return await ActionIfAllowed(
                () => _customerCommentsService.CreateAsync(_userContext.UserId, comment, ct),
                Roles.Customers,
                customer.AccountId);
        }
    }
}
