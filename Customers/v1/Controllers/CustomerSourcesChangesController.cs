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
    [Route("Customers/Sources/Changes/v1")]
    public class CustomerSourcesChangesController : AllowingCheckControllerBase
    {
        private readonly ICustomerSourcesService _customerSourcesService;
        private readonly ICustomerSourceChangesService _customerSourceChangesService;

        public CustomerSourcesChangesController(
            IUserContext userContext,
            ICustomerSourcesService customerSourcesService,
            ICustomerSourceChangesService customerSourceChangesService)
            : base(userContext)
        {
            _customerSourcesService = customerSourcesService;
            _customerSourceChangesService = customerSourceChangesService;
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<CustomerSourceChangeGetPagedListResponse>> GetPagedList(
            CustomerSourceChangeGetPagedListRequest request,
            CancellationToken ct = default)
        {
            var source = await _customerSourcesService.GetAsync(request.SourceId, false, ct);
            var response = await _customerSourceChangesService.GetPagedListAsync(request, ct);

            return ReturnIfAllowed(response, Roles.Customers, source.AccountId);
        }
    }
}
