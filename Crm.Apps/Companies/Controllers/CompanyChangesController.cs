using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Companies.Models;
using Crm.Apps.Companies.RequestParameters;
using Crm.Apps.Companies.Services;
using Crm.Common.All.UserContext;
using Crm.Common.All.UserContext.Attributes;
using Crm.Common.All.UserContext.BaseControllers;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Companies.Controllers
{
    [ApiController]
    [RequirePrivileged(Role.AccountOwning, Role.SalesManagement)]
    [Route("Api/Companies/Changes")]
    public class CompanyChangesController : AllowingCheckControllerBase
    {
        private readonly ICompaniesService _companiesService;
        private readonly ICompanyChangesService _companyChangesService;

        public CompanyChangesController(
            IUserContext userContext,
            ICompaniesService companiesService,
            ICompanyChangesService companyChangesService)
            : base(userContext)
        {
            _companyChangesService = companyChangesService;
            _companiesService = companiesService;
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<List<CompanyChange>>> GetPagedList(
            CompanyChangeGetPagedListRequestParameter request,
            CancellationToken ct = default)
        {
            var company = await _companiesService.GetAsync(request.CompanyId, ct);
            var changes = await _companyChangesService.GetPagedListAsync(request, ct);

            return ReturnIfAllowed(changes, new[] {Role.AccountOwning, Role.SalesManagement}, company.AccountId);
        }
    }
}