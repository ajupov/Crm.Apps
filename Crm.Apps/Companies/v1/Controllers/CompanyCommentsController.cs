using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Infrastructure.All.Jwt;
using Crm.Apps.Companies.Services;
using Crm.Apps.Companies.v1.Models;
using Crm.Apps.Companies.v1.RequestParameters;
using Crm.Common.All.BaseControllers;
using Crm.Common.All.Roles;
using Crm.Common.All.Roles.Attributes;
using Crm.Common.All.UserContext;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Companies.v1.Controllers
{
    [ApiController]
    [RequireSalesRole(JwtDefaults.AuthenticationScheme)]
    [Route("api/v1/Companies/Comments")]
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
        public async Task<ActionResult<List<CompanyComment>>> GetPagedList(
            CompanyCommentGetPagedListRequestParameter request,
            CancellationToken ct = default)
        {
            var company = await _companiesService.GetAsync(request.CompanyId, ct);
            var comments = await _companyCommentsService.GetPagedListAsync(request, ct);

            return ReturnIfAllowed(comments, Roles.Sales, company.AccountId);
        }

        [HttpPost("Create")]
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