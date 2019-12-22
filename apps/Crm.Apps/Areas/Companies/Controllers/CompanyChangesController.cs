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
    [Route("Api/Companies/Changes")]
    public class CompanyChangesController : ControllerBase
    {
        private readonly ICompanyChangesService _companyChangesService;

        public CompanyChangesController(ICompanyChangesService companyChangesService)
        {
            _companyChangesService = companyChangesService;
        }

        [HttpPost("GetPagedList")]
        [RequireAny(Role.System, Role.Development, Role.Administration, Role.TechnicalSupport)]
        public async Task<ActionResult<List<CompanyChange>>> GetPagedList(CompanyChangeGetPagedListParameter parameter,
            CancellationToken ct = default)
        {
            return await _companyChangesService.GetPagedListAsync(parameter, ct);
        }
    }
}