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
    [Route("Api/Companies/Comments")]
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

            return ReturnIfAllowed(comments, new[] {Role.AccountOwning, Role.SalesManagement}, company.AccountId);
        }

        [HttpPost("Create")]
        public async Task<ActionResult> Create(CompanyComment comment, CancellationToken ct = default)
        {
            var company = await _companiesService.GetAsync(comment.CompanyId, ct);

            return await ActionIfAllowed(
                () => _companyCommentsService.CreateAsync(_userContext.UserId, comment, ct),
                new[] {Role.AccountOwning, Role.SalesManagement},
                company.AccountId);
        }
    }
}