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
    [Route("Customers/Attributes/Changes/v1")]
    public class CustomerAttributeChangesController : AllowingCheckControllerBase
    {
        private readonly ICustomerAttributesService _customerAttributesService;
        private readonly ICustomerAttributeChangesService _customerAttributeChangesService;

        public CustomerAttributeChangesController(
            IUserContext userContext,
            ICustomerAttributeChangesService customerAttributeChangesService,
            ICustomerAttributesService customerAttributesService)
            : base(userContext)
        {
            _customerAttributeChangesService = customerAttributeChangesService;
            _customerAttributesService = customerAttributesService;
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<CustomerAttributeChangeGetPagedListResponse>> GetPagedList(
            CustomerAttributeChangeGetPagedListRequest request,
            CancellationToken ct = default)
        {
            var attribute = await _customerAttributesService.GetAsync(request.AttributeId, false, ct);
            var response = await _customerAttributeChangesService.GetPagedListAsync(request, ct);

            return ReturnIfAllowed(response, Roles.Customers, attribute.AccountId);
        }
    }
}
