using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Companies.Models;
using Crm.Apps.Companies.Parameters;
using Crm.Apps.Companies.Services;
using Crm.Common.UserContext;
using Crm.Common.UserContext.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Companies.Controllers
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
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.TechnicalSupport)]
        public async Task<ActionResult<List<CompanyChange>>> GetPagedList(CompanyChangeGetPagedListParameter parameter,
            CancellationToken ct = default)
        {
            return await _companyChangesService.GetPagedListAsync(parameter, ct);
        }
    }
}