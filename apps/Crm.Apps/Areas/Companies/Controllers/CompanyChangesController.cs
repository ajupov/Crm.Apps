using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Companies.Models;
using Crm.Apps.Areas.Companies.RequestParameters;
using Crm.Apps.Areas.Companies.Services;
using Crm.Common.UserContext;
using Crm.Common.UserContext.Attributes;
using Crm.Common.UserContext.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Areas.Companies.Controllers
{
    [ApiController]
    [RequirePrivileged(Role.AccountOwning, Role.SalesManagement)]
    [Route("Api/Companies/Changes")]
    public class CompanyChangesController : UserContextController
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