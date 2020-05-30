using System.Threading;
using System.Threading.Tasks;
using Ajupov.Infrastructure.All.Jwt;
using Ajupov.Infrastructure.All.Mvc.Attributes;
using Crm.Apps.Companies.Models;
using Crm.Apps.Companies.Services;
using Crm.Apps.Companies.V1.Requests;
using Crm.Apps.Companies.V1.Responses;
using Crm.Common.All.BaseControllers;
using Crm.Common.All.Roles;
using Crm.Common.All.Roles.Attributes;
using Crm.Common.All.UserContext;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Companies.V1.Controllers
{
    [ApiController]
    [RequestContentTypeApplicationJson]
    [ResponseContentTypeApplicationJson]
    [RequireSalesRole(JwtDefaults.AuthenticationScheme)]
    [Route("Companies/Comments/v1")]
    public class CompanyCommentsController : AllowingCheckControllerBase
    {
        private readonly IUserContext _userContext;
        private readonly ICompaniesService _companiesService;
        private readonly ICompanyCommentsService _companyCommentsService;

        public CompanyCommentsController(
            IUserContext userContext,
            ICompaniesService companiesService,
            ICompanyCommentsService companyCommentsService)
            : base(userContext)
        {
            _userContext = userContext;
            _companiesService = companiesService;
            _companyCommentsService = companyCommentsService;
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<CompanyCommentGetPagedListResponse>> GetPagedList(
            CompanyCommentGetPagedListRequest request,
            CancellationToken ct = default)
        {
            var company = await _companiesService.GetAsync(request.CompanyId, ct);
            var response = await _companyCommentsService.GetPagedListAsync(request, ct);

            return ReturnIfAllowed(response, Roles.Sales, company.AccountId);
        }

        [HttpPut("Create")]
        public async Task<ActionResult> Create(CompanyComment comment, CancellationToken ct = default)
        {
            var company = await _companiesService.GetAsync(comment.CompanyId, ct);

            return await ActionIfAllowed(
                () => _companyCommentsService.CreateAsync(_userContext.UserId, comment, ct),
                Roles.Sales,
                company.AccountId);
        }
    }
}
