using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Infrastructure.All.Jwt;
using Crm.Apps.Contacts.Models;
using Crm.Apps.Contacts.RequestParameters;
using Crm.Apps.Contacts.Services;
using Crm.Common.All.BaseControllers;
using Crm.Common.All.Roles;
using Crm.Common.All.Roles.Attributes;
using Crm.Common.All.UserContext;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Contacts.Controllers
{
    [ApiController]
    [RequireSalesRole(JwtDefaults.AuthenticationScheme)]
    [Route("Api/Contacts/Comments")]
    public class ContactCommentsController : AllowingCheckControllerBase
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
            ContactCommentGetPagedListRequestParameter request, 
            CancellationToken ct = default)
        {
            var contact = await _contactsService.GetAsync(request.ContactId, ct);
            var comments = await _contactCommentsService.GetPagedListAsync(request, ct);

            return ReturnIfAllowed(comments, Roles.Sales, contact.AccountId);
        }

        [HttpPost("Create")]
        public async Task<ActionResult> Create(ContactComment comment, CancellationToken ct = default)
        {
            var contact = await _contactsService.GetAsync(comment.ContactId, ct);

            return await ActionIfAllowed(
                () => _contactCommentsService.CreateAsync(_userContext.UserId, comment, ct),
                Roles.Sales,
                contact.AccountId);
        }
    }
}