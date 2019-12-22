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
    [Route("Api/Contacts/Comments")]
    public class ContactCommentsController : ControllerBase
    {
        private readonly IUserContext _userContext;
        private readonly IContactsService _contactsService;
        private readonly IContactCommentsService _contactCommentsService;

        public ContactCommentsController(IUserContext userContext, IContactsService contactsService,
            IContactCommentsService contactCommentsService)
        {
            _userContext = userContext;
            _contactsService = contactsService;
            _contactCommentsService = contactCommentsService;
        }

        [HttpPost("GetPagedList")]
        [RequireAny(Role.System, Role.Development, Role.Administration, Role.TechnicalSupport,
            Role.SalesManagement)]
        public async Task<ActionResult<List<ContactComment>>> GetPagedList(
            ContactCommentGetPagedListParameter parameter, CancellationToken ct = default)
        {
            var contact = await _contactsService.GetAsync(parameter.ContactId, ct);
            var comments = await _contactCommentsService.GetPagedListAsync(parameter, ct);

            return ReturnIfAllowed(comments, new[] {contact.AccountId});
        }

        [HttpPost("Create")]
        [RequireAny(Role.System, Role.Development, Role.Administration, Role.AccountOwning,
            Role.SalesManagement)]
        public async Task<ActionResult> Create(ContactComment comment, CancellationToken ct = default)
        {
            if (comment == null)
            {
                return BadRequest();
            }

            var contact = await _contactsService.GetAsync(comment.ContactId, ct);

            return await ActionIfAllowed(
                () => _contactCommentsService.CreateAsync(_userContext.UserId, comment, ct)
                , new[] {contact.AccountId});
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

            if (_userContext.HasAny(Role.AccountOwning, Role.ProductsManagement) &&
                _userContext.Belongs(accountIdsAsArray))
            {
                await action();

                return NoContent();
            }

            if (_userContext.HasAny(Role.AccountOwning, Role.ProductsManagement) &&
                !_userContext.Belongs(accountIdsAsArray))
            {
                return Forbid();
            }

            throw new Exception();
        }
    }
}