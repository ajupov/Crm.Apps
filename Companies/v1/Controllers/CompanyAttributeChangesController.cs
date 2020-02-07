using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Infrastructure.All.Jwt;
using Ajupov.Infrastructure.All.Mvc.Attributes;
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
    [RequestContentTypeApplicationJson]
    [ResponseContentTypeApplicationJson]
    [RequireSalesRole(JwtDefaults.AuthenticationScheme)]
    [Route("Companies/Attributes/Changes/v1")]
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