using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Infrastructure.All.Jwt;
using Crm.Apps.Contacts.Services;
using Crm.Apps.Contacts.v1.Models;
using Crm.Apps.Contacts.v1.RequestParameters;
using Crm.Common.All.BaseControllers;
using Crm.Common.All.Roles;
using Crm.Common.All.Roles.Attributes;
using Crm.Common.All.UserContext;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Contacts.v1.Controllers
{
    [ApiController]
    [RequireSalesRole(JwtDefaults.AuthenticationScheme)]
    [Route("api/v1/Contacts/Changes")]
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
        public async Task<ActionResult<List<ContactChange>>> GetPagedList(
            ContactChangeGetPagedListRequestParameter request,
            CancellationToken ct = default)
        {
            var contact = await _contactsService.GetAsync(request.ContactId, ct);
            var changes = await _contactChangesService.GetPagedListAsync(request, ct);

            return ReturnIfAllowed(changes, Roles.Sales, contact.AccountId);
        }
    }
}