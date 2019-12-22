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
    [Route("Api/Companies/Comments")]
    public class CompanyCommentsController : UserContextController
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