using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Infrastructure.All.Jwt;
using Crm.Apps.Companies.Models;
using Crm.Apps.Companies.RequestParameters;
using Crm.Apps.Companies.Services;
using Crm.Common.All.BaseControllers;
using Crm.Common.All.Roles;
using Crm.Common.All.Roles.Attributes;
using Crm.Common.All.UserContext;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Companies.Controllers
{
    [ApiController]
    [RequireSalesRole(JwtDefaults.AuthenticationScheme)]
    [Route("Api/Companies/Attributes/Changes")]
    public class CompanyAttributeChangesController : AllowingCheckControllerBase
    {
        private readonly ICompanyAttributesService _companyAttributesService;
        private readonly ICompanyAttributeChangesService _companyAttributeChangesService;

        public CompanyAttributeChangesController(
            IUserContext userContext,
            ICompanyAttributesService companyAttributesService,
            ICompanyAttributeChangesService companyAttributeChangesService)
            : base(userContext)
        {
            _companyAttributesService = companyAttributesService;
            _companyAttributeChangesService = companyAttributeChangesService;
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<List<CompanyAttributeChange>>> GetPagedList(
            CompanyAttributeChangeGetPagedListRequestParameter request,
            CancellationToken ct = default)
        {
            var attribute = await _companyAttributesService.GetAsync(request.AttributeId, ct);
            var changes = await _companyAttributeChangesService.GetPagedListAsync(request, ct);

            return ReturnIfAllowed(changes, Roles.Sales, attribute.AccountId);
        }
    }
}