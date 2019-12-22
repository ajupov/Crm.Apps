using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Companies.Services;
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
    [Route("Api/Contacts/Attributes/Changes")]
    public class ContactAttributeChangesController : UserContextController
    {
        private readonly ICompanyAttributesService _companyAttributesService;
        private readonly IContactAttributeChangesService _contactAttributeChangesService;

        public ContactAttributeChangesController(
            IUserContext userContext,
            ICompanyAttributesService companyAttributesService,
            IContactAttributeChangesService contactAttributeChangesService)
            : base(userContext)
        {
            _companyAttributesService = companyAttributesService;
            _contactAttributeChangesService = contactAttributeChangesService;
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<List<ContactAttributeChange>>> GetPagedList(
            ContactAttributeChangeGetPagedListParameter parameter,
            CancellationToken ct = default)
        {
            var attribute = await _companyAttributesService.GetAsync(parameter.AttributeId, ct);
            var changes = await _contactAttributeChangesService.GetPagedListAsync(parameter, ct);

            return ReturnIfAllowed(changes, new[] {Role.AccountOwning, Role.SalesManagement}, attribute.AccountId);
        }
    }
}