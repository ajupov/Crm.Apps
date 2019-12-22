using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Contacts.Models;
using Crm.Apps.Areas.Contacts.Parameters;
using Crm.Apps.Areas.Contacts.Services;
using Crm.Common.UserContext;
using Crm.Common.UserContext.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Areas.Contacts.Controllers
{
    [ApiController]
    [Route("Api/Contacts")]
    public class ContactsController : ControllerBase
    {
        private readonly IUserContext _userContext;
        private readonly IContactsService _contactsService;

        public ContactsController(IUserContext userContext, IContactsService contactsService)
        {
            _userContext = userContext;
            _contactsService = contactsService;
        }

        [HttpGet("Get")]
        [RequireAny(Role.System, Role.Development, Role.Administration, Role.TechnicalSupport,
            Role.AccountOwning, Role.SalesManagement)]
        public async Task<ActionResult<Contact>> Get(Guid id, CancellationToken ct = default)
        {
            if (id.IsEmpty())
            {
                return BadRequest();
            }

            var contact = await _contactsService.GetAsync(id, ct);
            if (contact == null)
            {
                return NotFound();
            }

            return ReturnIfAllowed(contact, new[] {contact.AccountId});
        }

        [HttpPost("GetList")]
        [RequireAny(Role.System, Role.Development, Role.Administration, Role.TechnicalSupport,
            Role.AccountOwning, Role.SalesManagement)]
        public async Task<ActionResult<List<Contact>>> GetList(List<Guid> ids, CancellationToken ct = default)
        {
            if (ids == null || ids.All(x => x.IsEmpty()))
            {
                return BadRequest();
            }

            var contacts = await _contactsService.GetListAsync(ids, ct);

            return ReturnIfAllowed(contacts, contacts.Select(x => x.AccountId));
        }

        [HttpPost("GetPagedList")]
        [RequireAny(Role.System, Role.Development, Role.Administration, Role.TechnicalSupport,
            Role.AccountOwning, Role.SalesManagement)]
        public async Task<ActionResult<List<Contact>>> GetPagedList(ContactGetPagedListParameter parameter,
            CancellationToken ct = default)
        {
            var contacts = await _contactsService.GetPagedListAsync(parameter, ct);

            return ReturnIfAllowed(contacts, contacts.Select(x => x.AccountId));
        }

        [HttpPost("Create")]
        [RequireAny(Role.System, Role.Development, Role.Administration, Role.AccountOwning,
            Role.SalesManagement)]
        public async Task<ActionResult<Guid>> Create(Contact contact, CancellationToken ct = default)
        {
            if (contact == null)
            {
                return BadRequest();
            }

            if (!_userContext.HasAny(Role.System, Role.Development, Role.Administration,
                Role.TechnicalSupport))
            {
                contact.AccountId = _userContext.AccountId;
            }

            var id = await _contactsService.CreateAsync(_userContext.UserId, contact, ct);

            return Created(nameof(Get), id);
        }

        [HttpPost("Update")]
        [RequireAny(Role.System, Role.Development, Role.Administration, Role.TechnicalSupport,
            Role.AccountOwning, Role.SalesManagement)]
        public async Task<ActionResult> Update(Contact contact, CancellationToken ct = default)
        {
            if (contact.Id.IsEmpty())
            {
                return BadRequest();
            }

            var oldContact = await _contactsService.GetAsync(contact.Id, ct);
            if (oldContact == null)
            {
                return NotFound();
            }

            return await ActionIfAllowed(
                () => _contactsService.UpdateAsync(_userContext.UserId, oldContact, contact, ct),
                new[] {contact.AccountId, oldContact.AccountId});
        }

        [HttpPost("Delete")]
        [RequireAny(Role.System, Role.Development, Role.Administration, Role.TechnicalSupport,
            Role.AccountOwning, Role.SalesManagement)]
        public async Task<ActionResult> Delete(List<Guid> ids, CancellationToken ct = default)
        {
            if (ids == null || ids.All(x => x.IsEmpty()))
            {
                return BadRequest();
            }

            var contacts = await _contactsService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _contactsService.DeleteAsync(_userContext.UserId, contacts.Select(x => x.Id), ct),
                contacts.Select(x => x.AccountId));
        }

        [HttpPost("Restore")]
        [RequireAny(Role.System, Role.Development, Role.Administration, Role.TechnicalSupport,
            Role.AccountOwning, Role.SalesManagement)]
        public async Task<ActionResult> Restore(List<Guid> ids, CancellationToken ct = default)
        {
            if (ids == null || ids.All(x => x.IsEmpty()))
            {
                return BadRequest();
            }

            var contacts = await _contactsService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _contactsService.RestoreAsync(_userContext.UserId, contacts.Select(x => x.Id), ct),
                contacts.Select(x => x.AccountId));
        }

        [NonAction]
        private ActionResult<TResult> ReturnIfAllowed<TResult>(TResult result, IEnumerable<Guid> accountIds)
        {
            if (_userContext.HasAny(Role.System, Role.Development, Role.Administration,
                Role.TechnicalSupport))
            {
                return result;
            }

            var accountIdsAsArray = accountIds.ToArray();

            if (_userContext.HasAny(Role.AccountOwning, Role.SalesManagement) &&
                _userContext.Belongs(accountIdsAsArray))
            {
                return result;
            }

            if (_userContext.HasAny(Role.AccountOwning, Role.SalesManagement) &&
                !_userContext.Belongs(accountIdsAsArray))
            {
                return Forbid();
            }

            throw new Exception();
        }

        [NonAction]
        private async Task<ActionResult> ActionIfAllowed(Func<Task> action, IEnumerable<Guid> accountIds)
        {
            if (_userContext.HasAny(Role.System, Role.Development, Role.Administration,
                Role.TechnicalSupport))
            {
                await action();

                return NoContent();
            }

            var accountIdsAsArray = accountIds.ToArray();

            if (_userContext.HasAny(Role.AccountOwning, Role.SalesManagement) &&
                _userContext.Belongs(accountIdsAsArray))
            {
                await action();

                return NoContent();
            }

            if (_userContext.HasAny(Role.AccountOwning, Role.SalesManagement) &&
                !_userContext.Belongs(accountIdsAsArray))
            {
                return Forbid();
            }

            throw new Exception();
        }
    }
}