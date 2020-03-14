using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Infrastructure.All.Jwt;
using Ajupov.Infrastructure.All.Mvc.Attributes;
using Ajupov.Utils.All.Enums;
using Crm.Apps.Leads.Models;
using Crm.Apps.Leads.Services;
using Crm.Apps.Leads.v1.Requests;
using Crm.Apps.Leads.v1.Responses;
using Crm.Common.All.BaseControllers;
using Crm.Common.All.Roles;
using Crm.Common.All.Roles.Attributes;
using Crm.Common.All.Types.AttributeType;
using Crm.Common.All.UserContext;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Leads.v1.Controllers
{
    [ApiController]
    [RequestContentTypeApplicationJson]
    [ResponseContentTypeApplicationJson]
    [RequireLeadsRole(JwtDefaults.AuthenticationScheme)]
    [Route("Leads/Attributes/v1")]
    public class LeadAttributesController : AllowingCheckControllerBase
    {
        private readonly IUserContext _userContext;
        private readonly ILeadAttributesService _leadAttributesService;

        public LeadAttributesController(IUserContext userContext, ILeadAttributesService leadAttributesService)
            : base(userContext)
        {
            _userContext = userContext;
            _leadAttributesService = leadAttributesService;
        }

        [HttpGet("GetTypes")]
        public Dictionary<string, AttributeType> GetTypes()
        {
            return EnumsExtensions.GetAsDictionary<AttributeType>();
        }

        [HttpGet("Get")]
        public async Task<ActionResult<LeadAttribute>> Get([Required] Guid id, CancellationToken ct = default)
        {
            var attribute = await _leadAttributesService.GetAsync(id, ct);
            if (attribute == null)
            {
                return NotFound(id);
            }

            return ReturnIfAllowed(attribute, Roles.Leads, attribute.AccountId);
        }

        [HttpPost("GetList")]
        public async Task<ActionResult<List<LeadAttribute>>> GetList(
            [Required] List<Guid> ids,
            CancellationToken ct = default)
        {
            var attributes = await _leadAttributesService.GetListAsync(ids, ct);

            return ReturnIfAllowed(
                attributes,
                Roles.Leads,
                attributes.Select(x => x.AccountId));
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<LeadAttributeGetPagedListResponse>> GetPagedList(
            LeadAttributeGetPagedListRequest request,
            CancellationToken ct = default)
        {
            var response = await _leadAttributesService.GetPagedListAsync(_userContext.AccountId, request, ct);

            return ReturnIfAllowed(
                response,
                Roles.Leads,
                response.Attributes.Select(x => x.AccountId));
        }

        [HttpPost("Create")]
        public async Task<ActionResult<Guid>> Create(LeadAttribute attribute, CancellationToken ct = default)
        {
            attribute.AccountId = _userContext.AccountId;

            var id = await _leadAttributesService.CreateAsync(_userContext.UserId, attribute, ct);

            return Created(nameof(Get), id);
        }

        [HttpPost("Update")]
        public async Task<ActionResult> Update(LeadAttribute attribute, CancellationToken ct = default)
        {
            var oldAttribute = await _leadAttributesService.GetAsync(attribute.Id, ct);
            if (oldAttribute == null)
            {
                return NotFound(attribute.Id);
            }

            return await ActionIfAllowed(
                () => _leadAttributesService.UpdateAsync(_userContext.UserId, oldAttribute, attribute, ct),
                Roles.Leads,
                attribute.AccountId, oldAttribute.AccountId);
        }

        [HttpPost("Delete")]
        public async Task<ActionResult> Delete([Required] List<Guid> ids, CancellationToken ct = default)
        {
            var attributes = await _leadAttributesService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _leadAttributesService.DeleteAsync(_userContext.UserId, attributes.Select(x => x.Id), ct),
                Roles.Leads,
                attributes.Select(x => x.AccountId));
        }

        [HttpPost("Restore")]
        public async Task<ActionResult> Restore([Required] List<Guid> ids, CancellationToken ct = default)
        {
            var attributes = await _leadAttributesService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _leadAttributesService.RestoreAsync(_userContext.UserId, attributes.Select(x => x.Id), ct),
                Roles.Leads,
                attributes.Select(x => x.AccountId));
        }
    }
}