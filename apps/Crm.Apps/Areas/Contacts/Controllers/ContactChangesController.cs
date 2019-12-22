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
    [Route("Api/Contacts/Changes")]
    public class ContactChangesController : ControllerBase
    {
        private readonly IContactChangesService _contactChangesService;

        public ContactChangesController(IContactChangesService contactChangesService)
        {
            _contactChangesService = contactChangesService;
        }

        [HttpPost("GetPagedList")]
        [RequireAny(Role.System, Role.Development, Role.Administration, Role.TechnicalSupport)]
        public async Task<ActionResult<List<ContactChange>>> GetPagedList(ContactChangeGetPagedListParameter parameter,
            CancellationToken ct = default)
        {
            return await _contactChangesService.GetPagedListAsync(parameter, ct);
        }
    }
}