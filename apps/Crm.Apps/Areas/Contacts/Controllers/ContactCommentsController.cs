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
    [Route("Api/Contacts/Comments")]
    public class ContactCommentsController : UserContextController
    {
        private readonly IUserContext _userContext;
        private readonly IContactsService _contactsService;
        private readonly IContactCommentsService _contactCommentsService;

        public ContactCommentsController(
            IUserContext userContext,
            IContactsService contactsService,
            IContactCommentsService contactCommentsService)
            : base(userContext)
        {
            _userContext = userContext;
            _contactsService = contactsService;
            _contactCommentsService = contactCommentsService;
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<List<ContactComment>>> GetPagedList(
            ContactCommentGetPagedListParameter parameter, 
            CancellationToken ct = default)
        {
            var contact = await _contactsService.GetAsync(parameter.ContactId, ct);
            var comments = await _contactCommentsService.GetPagedListAsync(parameter, ct);

            return ReturnIfAllowed(comments, new[] {Role.AccountOwning, Role.SalesManagement}, contact.AccountId);
        }

        [HttpPost("Create")]
        public async Task<ActionResult> Create(ContactComment comment, CancellationToken ct = default)
        {
            var contact = await _contactsService.GetAsync(comment.ContactId, ct);

            return await ActionIfAllowed(
                () => _contactCommentsService.CreateAsync(_userContext.UserId, comment, ct),
                new[] {Role.AccountOwning, Role.SalesManagement},
                contact.AccountId);
        }
    }
}