using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Contacts.Models;
using Crm.Apps.Areas.Contacts.RequestParameters;
using Crm.Apps.Areas.Contacts.Services;
using Crm.Common.UserContext;
using Crm.Common.UserContext.Attributes;
using Crm.Common.UserContext.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Areas.Contacts.Controllers
{
    [ApiController]
    [RequirePrivileged(Role.AccountOwning, Role.SalesManagement)]
    [Route("Api/Contacts/Changes")]
    public class ContactChangesController : UserContextController
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

            return ReturnIfAllowed(changes, new[] {Role.AccountOwning, Role.SalesManagement}, contact.AccountId);
        }
    }
}