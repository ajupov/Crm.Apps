using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Infrastructure.All.Jwt;
using Ajupov.Infrastructure.All.Mvc.Attributes;
using Ajupov.Utils.All.Enums;
using Crm.Apps.Companies.Models;
using Crm.Apps.Companies.Services;
using Crm.Apps.Companies.V1.Requests;
using Crm.Apps.Companies.V1.Responses;
using Crm.Common.All.BaseControllers;
using Crm.Common.All.Roles;
using Crm.Common.All.Roles.Attributes;
using Crm.Common.All.Types.AttributeType;
using Crm.Common.All.UserContext;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Companies.V1.Controllers
{
    [ApiController]
    [RequestContentTypeApplicationJson]
    [ResponseContentTypeApplicationJson]
    [RequireSalesRole(JwtDefaults.AuthenticationScheme)]
    [Route("Companies/Attributes/v1")]
    public class CompanyAttributesController : AllowingCheckControllerBase
    {
        private readonly IUserContext _userContext;
        private readonly ICompanyAttributesService _companyAttributesService;

        public CompanyAttributesController(IUserContext userContext, ICompanyAttributesService companyAttributesService)
            : base(userContext)
        {
            _userContext = userContext;
            _companyAttributesService = companyAttributesService;
        }

        [HttpGet("GetTypes")]
        public ActionResult<Dictionary<string, AttributeType>> GetTypes()
        {
            return EnumsExtensions.GetAsDictionary<AttributeType>();
        }

        [HttpGet("Get")]
        public async Task<ActionResult<CompanyAttribute>> Get([Required] Guid id, CancellationToken ct = default)
        {
            var attribute = await _companyAttributesService.GetAsync(id, ct);
            if (attribute == null)
            {
                return NotFound(id);
            }

            return ReturnIfAllowed(attribute, Roles.Sales, attribute.AccountId);
        }

        [HttpPost("GetList")]
        public async Task<ActionResult<List<CompanyAttribute>>> GetList(
            [Required] List<Guid> ids,
            CancellationToken ct = default)
        {
            var attributes = await _companyAttributesService.GetListAsync(ids, ct);

            return ReturnIfAllowed(
                attributes,
                Roles.Sales,
                attributes.Select(x => x.AccountId));
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<CompanyAttributeGetPagedListResponse>> GetPagedList(
            CompanyAttributeGetPagedListRequest request,
            CancellationToken ct = default)
        {
            var response = await _companyAttributesService.GetPagedListAsync(_userContext.AccountId, request, ct);

            return ReturnIfAllowed(
                response,
                Roles.Sales,
                response.Attributes.Select(x => x.AccountId));
        }

        [HttpPut("Create")]
        public async Task<ActionResult<Guid>> Create(CompanyAttribute attribute, CancellationToken ct = default)
        {
            attribute.AccountId = _userContext.AccountId;

            var id = await _companyAttributesService.CreateAsync(_userContext.UserId, attribute, ct);

            return Created(nameof(Get), id);
        }

        [HttpPatch("Update")]
        public async Task<ActionResult> Update(CompanyAttribute attribute, CancellationToken ct = default)
        {
            var oldAttribute = await _companyAttributesService.GetAsync(attribute.Id, ct);
            if (oldAttribute == null)
            {
                return NotFound(attribute.Id);
            }

            return await ActionIfAllowed(
                () => _companyAttributesService.UpdateAsync(_userContext.UserId, oldAttribute, attribute, ct),
                Roles.Sales,
                oldAttribute.AccountId);
        }

        [HttpPatch("Delete")]
        public async Task<ActionResult> Delete([Required] List<Guid> ids, CancellationToken ct = default)
        {
            var attributes = await _companyAttributesService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _companyAttributesService.DeleteAsync(_userContext.UserId, attributes.Select(x => x.Id), ct),
                Roles.Sales,
                attributes.Select(x => x.AccountId));
        }

        [HttpPatch("Restore")]
        public async Task<ActionResult> Restore([Required] List<Guid> ids, CancellationToken ct = default)
        {
            var attributes = await _companyAttributesService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _companyAttributesService.RestoreAsync(_userContext.UserId, attributes.Select(x => x.Id), ct),
                Roles.Sales,
                attributes.Select(x => x.AccountId));
        }
    }
}
