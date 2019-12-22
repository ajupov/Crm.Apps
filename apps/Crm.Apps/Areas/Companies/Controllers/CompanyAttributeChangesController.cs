using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Companies.Models;
using Crm.Apps.Areas.Companies.Parameters;
using Crm.Apps.Areas.Companies.Services;
using Crm.Common.UserContext;
using Crm.Common.UserContext.Attributes;
using Crm.Common.UserContext.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Areas.Companies.Controllers
{
    [ApiController]
    [RequirePrivileged(Role.AccountOwning, Role.SalesManagement)]
    [Route("Api/Companies/Attributes/Changes")]
    public class CompanyAttributeChangesController : UserContextController
    {
        private readonly ICompanyAttributesService _companyAttributesService;
        private readonly ICompanyAttributeChangesService _companyAttributeChangesService;

        public CompanyAttributeChangesController(
            IUserContext userContext,
            ICompanyAttributesService companyAttributesService,
            ICompanyAttributeChangesService companyAttributeChangesService)
            : base(userContext)
        {
            _companyAttributesService = companyAttributesService;
            _companyAttributeChangesService = companyAttributeChangesService;
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<List<CompanyAttributeChange>>> GetPagedList(
            CompanyAttributeChangeGetPagedListParameter parameter,
            CancellationToken ct = default)
        {
            var attribute = await _companyAttributesService.GetAsync(parameter.AttributeId, ct);
            var changes = await _companyAttributeChangesService.GetPagedListAsync(parameter, ct);

            return ReturnIfAllowed(changes, new[] {Role.AccountOwning, Role.SalesManagement}, attribute.AccountId);
        }
    }
}