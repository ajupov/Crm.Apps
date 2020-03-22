using System.Threading;
using System.Threading.Tasks;
using Ajupov.Infrastructure.All.Jwt;
using Ajupov.Infrastructure.All.Mvc.Attributes;
using Crm.Apps.Contacts.Services;
using Crm.Apps.Contacts.v1.Requests;
using Crm.Apps.Contacts.v1.Responses;
using Crm.Common.All.BaseControllers;
using Crm.Common.All.Roles;
using Crm.Common.All.Roles.Attributes;
using Crm.Common.All.UserContext;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Contacts.v1.Controllers
{
    [ApiController]
    [RequestContentTypeApplicationJson]
    [ResponseContentTypeApplicationJson]
    [RequireSalesRole(JwtDefaults.AuthenticationScheme)]
    [Route("Contacts/Attributes/Changes/v1")]
    public class ContactAttributeChangesController : AllowingCheckControllerBase
    {
        private readonly IContactAttributesService _contactAttributesService;
        private readonly IContactAttributeChangesService _contactAttributeChangesService;

        public ContactAttributeChangesController(
            IUserContext userContext,
            IContactAttributesService contactAttributesService,
            IContactAttributeChangesService contactAttributeChangesService)
            : base(userContext)
        {
            _contactAttributesService = contactAttributesService;
            _contactAttributeChangesService = contactAttributeChangesService;
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<ContactAttributeChangeGetPagedListResponse>> GetPagedList(
            ContactAttributeChangeGetPagedListRequest request,
            CancellationToken ct = default)
        {
            var attribute = await _contactAttributesService.GetAsync(request.AttributeId, ct);
            var response = await _contactAttributeChangesService.GetPagedListAsync(request, ct);

            return ReturnIfAllowed(response, Roles.Sales, attribute.AccountId);
        }
    }
}