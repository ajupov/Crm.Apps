using System.Threading;
using System.Threading.Tasks;
using Ajupov.Infrastructure.All.Jwt;
using Ajupov.Infrastructure.All.Mvc.Attributes;
using Crm.Apps.Contacts.Services;
using Crm.Apps.Contacts.V1.Requests;
using Crm.Apps.Contacts.V1.Responses;
using Crm.Common.All.BaseControllers;
using Crm.Common.All.Roles;
using Crm.Common.All.Roles.Attributes;
using Crm.Common.All.UserContext;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Contacts.V1.Controllers
{
    [ApiController]
    [RequestContentTypeApplicationJson]
    [ResponseContentTypeApplicationJson]
    [RequireSalesRole(JwtDefaults.AuthenticationScheme)]
    [Route("Contacts/Changes/v1")]
    public class ContactChangesController : AllowingCheckControllerBase
    {
        private readonly IContactsService _contactsService;
        private readonly IContactChangesService _contactChangesService;

        public ContactChangesController(
            IUserContext userContext,
            IContactsService contactsService,
            IContactChangesService contactChangesService)
            : base(userContext)
        {
            _contactsService = contactsService;
            _contactChangesService = contactChangesService;
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<ContactChangeGetPagedListResponse>> GetPagedList(
            ContactChangeGetPagedListRequest request,
            CancellationToken ct = default)
        {
            var contact = await _contactsService.GetAsync(request.ContactId, false, ct);
            var response = await _contactChangesService.GetPagedListAsync(request, ct);

            return ReturnIfAllowed(response, Roles.Sales, contact.AccountId);
        }
    }
}
