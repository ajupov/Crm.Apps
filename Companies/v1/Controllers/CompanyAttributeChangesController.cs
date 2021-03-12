using System.Threading;
using System.Threading.Tasks;
using Ajupov.Infrastructure.All.Api.Attributes;
using Ajupov.Infrastructure.All.Jwt;
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
    [RequireCompaniesRole(JwtDefaults.AuthenticationScheme)]
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
        public async Task<ActionResult<CompanyAttributeChangeGetPagedListResponse>> GetPagedList(
            CompanyAttributeChangeGetPagedListRequest request,
            CancellationToken ct = default)
        {
            var attribute = await _companyAttributesService.GetAsync(request.AttributeId, false, ct);
            var response = await _companyAttributeChangesService.GetPagedListAsync(request, ct);

            return ReturnIfAllowed(response, Roles.Companies, attribute.AccountId);
        }
    }
}
