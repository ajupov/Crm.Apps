using System.Threading;
using System.Threading.Tasks;
using Ajupov.Infrastructure.All.Jwt;
using Ajupov.Infrastructure.All.Mvc.Attributes;
using Crm.Apps.Companies.Services;
using Crm.Apps.Companies.v1.Requests;
using Crm.Apps.Companies.v1.Responses;
using Crm.Common.All.BaseControllers;
using Crm.Common.All.Roles;
using Crm.Common.All.Roles.Attributes;
using Crm.Common.All.UserContext;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Companies.v1.Controllers
{
    [ApiController]
    [RequestContentTypeApplicationJson]
    [ResponseContentTypeApplicationJson]
    [RequireSalesRole(JwtDefaults.AuthenticationScheme)]
    [Route("Companies/Changes/v1")]
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
        public async Task<ActionResult<CompanyChangeGetPagedListResponse>> GetPagedList(
            CompanyChangeGetPagedListRequest request,
            CancellationToken ct = default)
        {
            var company = await _companiesService.GetAsync(request.CompanyId, ct);
            var response = await _companyChangesService.GetPagedListAsync(request, ct);

            return ReturnIfAllowed(response, Roles.Sales, company.AccountId);
        }
    }
}