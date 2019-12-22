using System.Collections.Generic;
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
    [Route("Api/Contacts/Attributes/Changes")]
    public class ContactAttributeChangesController : ControllerBase
    {
        private readonly IContactAttributeChangesService _contactAttributeChangesService;

        public ContactAttributeChangesController(IContactAttributeChangesService contactAttributeChangesService)
        {
            _contactAttributeChangesService = contactAttributeChangesService;
        }

        [HttpPost("GetPagedList")]
        [RequireAny(Role.System, Role.Development, Role.Administration, Role.TechnicalSupport)]
        public async Task<ActionResult<List<ContactAttributeChange>>> GetPagedList(
            ContactAttributeChangeGetPagedListParameter parameter, CancellationToken ct = default)
        {
            return await _contactAttributeChangesService.GetPagedListAsync(parameter, ct);
        }
    }
}