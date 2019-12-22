using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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
    [Route("Api/Contacts")]
    public class ContactsController : UserContextController
    {
        private readonly IUserContext _userContext;
        private readonly IContactsService _contactsService;

        public ContactsController(IUserContext userContext, IContactsService contactsService)
            : base(userContext)
        {
            _userContext = userContext;
            _contactsService = contactsService;
        }

        [HttpGet("Get")]
        public async Task<ActionResult<Contact>> Get([Required] Guid id, CancellationToken ct = default)
        {
            var contact = await _contactsService.GetAsync(id, ct);
            if (contact == null)
            {
                return NotFound(id);
            }

            return ReturnIfAllowed(contact, new[] {Role.AccountOwning, Role.SalesManagement}, contact.AccountId);
        }

        [HttpPost("GetList")]
        public async Task<ActionResult<List<Contact>>> GetList(
            [Required] List<Guid> ids,
            CancellationToken ct = default)
        {
            var contacts = await _contactsService.GetListAsync(ids, ct);

            return ReturnIfAllowed(
                contacts,
                new[] {Role.AccountOwning, Role.SalesManagement},
                contacts.Select(x => x.AccountId));
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<List<Contact>>> GetPagedList(
            ContactGetPagedListRequestParameter request,
            CancellationToken ct = default)
        {
            request.AccountId = _userContext.AccountId;

            var contacts = await _contactsService.GetPagedListAsync(request, ct);

            return ReturnIfAllowed(
                contacts,
                new[] {Role.AccountOwning, Role.SalesManagement},
                contacts.Select(x => x.AccountId));
        }

        [HttpPost("Create")]
        public async Task<ActionResult<Guid>> Create(Contact contact, CancellationToken ct = default)
        {
            contact.AccountId = _userContext.AccountId;

            var id = await _contactsService.CreateAsync(_userContext.UserId, contact, ct);

            return Created(nameof(Get), id);
        }

        [HttpPost("Update")]
        public async Task<ActionResult> Update(Contact contact, CancellationToken ct = default)
        {
            var oldContact = await _contactsService.GetAsync(contact.Id, ct);
            if (oldContact == null)
            {
                return NotFound(contact.Id);
            }

            return await ActionIfAllowed(
                () => _contactsService.UpdateAsync(_userContext.UserId, oldContact, contact, ct),
                new[] {Role.AccountOwning, Role.SalesManagement},
                contact.AccountId, oldContact.AccountId);
        }

        [HttpPost("Delete")]
        public async Task<ActionResult> Delete([Required] List<Guid> ids, CancellationToken ct = default)
        {
            var contacts = await _contactsService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _contactsService.DeleteAsync(_userContext.UserId, contacts.Select(x => x.Id), ct),
                new[] {Role.AccountOwning, Role.SalesManagement},
                contacts.Select(x => x.AccountId));
        }

        [HttpPost("Restore")]
        public async Task<ActionResult> Restore([Required] List<Guid> ids, CancellationToken ct = default)
        {
            var contacts = await _contactsService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _contactsService.RestoreAsync(_userContext.UserId, contacts.Select(x => x.Id), ct),
                new[] {Role.AccountOwning, Role.SalesManagement},
                contacts.Select(x => x.AccountId));
        }
    }
}