using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Companies.Models;
using Crm.Apps.Areas.Companies.Parameters;
using Crm.Apps.Areas.Companies.Services;
using Crm.Common.UserContext;
using Crm.Common.UserContext.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Areas.Companies.Controllers
{
    [ApiController]
    [Route("Api/Companies/Attributes/Changes")]
    public class CompanyAttributeChangesController : ControllerBase
    {
        private readonly ICompanyAttributeChangesService _companyAttributeChangesService;

        public CompanyAttributeChangesController(ICompanyAttributeChangesService companyAttributeChangesService)
        {
            _companyAttributeChangesService = companyAttributeChangesService;
        }

        [HttpPost("GetPagedList")]
        [RequireAny(Role.System, Role.Development, Role.Administration, Role.TechnicalSupport)]
        public async Task<ActionResult<List<CompanyAttributeChange>>> GetPagedList(
            CompanyAttributeChangeGetPagedListParameter parameter, CancellationToken ct = default)
        {
            return await _companyAttributeChangesService.GetPagedListAsync(parameter, ct);
        }
    }
}