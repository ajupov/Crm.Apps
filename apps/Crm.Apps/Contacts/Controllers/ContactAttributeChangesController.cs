using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Contacts.Models;
using Crm.Apps.Contacts.Parameters;
using Crm.Apps.Contacts.Services;
using Crm.Common.UserContext;
using Crm.Common.UserContext.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Contacts.Controllers
{
    [ApiController]
    [Route("Api/Contacts/Attributes/Changes")]
    public class ContactAttributeChangesController : ControllerBase
    {
        private readonly IContactAttributeChangesService _contactAttributeChangesService;

        public ContactAttributeChangesController(IContactAttributeChangesService contactAttributeChangesService)
        {
            _contactAttributeChangesService = contactAttributeChangesService;
        }

        [HttpPost("GetPagedList")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.TechnicalSupport)]
        public async Task<ActionResult<List<ContactAttributeChange>>> GetPagedList(
            ContactAttributeChangeGetPagedListParameter parameter, CancellationToken ct = default)
        {
            return await _contactAttributeChangesService.GetPagedListAsync(parameter, ct).ConfigureAwait(false);
        }
    }
}