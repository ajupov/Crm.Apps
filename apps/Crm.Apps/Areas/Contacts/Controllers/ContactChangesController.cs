using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Contacts.Models;
using Crm.Apps.Areas.Contacts.Parameters;
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
            ContactChangeGetPagedListParameter parameter,
            CancellationToken ct = default)
        {
            var contact = await _contactsService.GetAsync(parameter.ContactId, ct);
            var changes = await _contactChangesService.GetPagedListAsync(parameter, ct);

            return ReturnIfAllowed(changes, new[] {Role.AccountOwning, Role.SalesManagement}, contact.AccountId);
        }
    }
}