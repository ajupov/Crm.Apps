using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Infrastructure.All.Jwt;
using Ajupov.Utils.All.Enums;
using Crm.Apps.Deals.Models;
using Crm.Apps.Deals.RequestParameters;
using Crm.Apps.Deals.Services;
using Crm.Common.All.BaseControllers;
using Crm.Common.All.Roles;
using Crm.Common.All.Roles.Attributes;
using Crm.Common.All.Types.AttributeType;
using Crm.Common.All.UserContext;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Deals.Controllers
{
    [ApiController]
    [RequireSalesRole(JwtDefaults.AuthenticationScheme)]
    [Route("Api/Deals/Attributes")]
    public class DealAttributesController : AllowingCheckControllerBase
    {
        private readonly IUserContext _userContext;
        private readonly IDealAttributesService _dealAttributesService;

        public DealAttributesController(IUserContext userContext, IDealAttributesService dealAttributesService)
            : base(userContext)
        {
            _userContext = userContext;
            _dealAttributesService = dealAttributesService;
        }

        [HttpGet("GetTypes")]
        public Dictionary<string, AttributeType> GetTypes()
        {
            return EnumsExtensions.GetAsDictionary<AttributeType>();
        }

        [HttpGet("Get")]
        public async Task<ActionResult<DealAttribute>> Get([Required] Guid id, CancellationToken ct = default)
        {
            var attribute = await _dealAttributesService.GetAsync(id, ct);
            if (attribute == null)
            {
                return NotFound(id);
            }

            return ReturnIfAllowed(attribute, Roles.Sales, attribute.AccountId);
        }

        [HttpPost("GetList")]
        public async Task<ActionResult<List<DealAttribute>>> GetList(
            [Required] List<Guid> ids,
            CancellationToken ct = default)
        {
            var attributes = await _dealAttributesService.GetListAsync(ids, ct);

            return ReturnIfAllowed(
                attributes,
                Roles.Sales,
                attributes.Select(x => x.AccountId));
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<List<DealAttribute>>> GetPagedList(
            DealAttributeGetPagedListRequestParameter request,
            CancellationToken ct = default)
        {
            request.AccountId = _userContext.AccountId;

            var attributes = await _dealAttributesService.GetPagedListAsync(request, ct);

            return ReturnIfAllowed(
                attributes,
                Roles.Sales,
                attributes.Select(x => x.AccountId));
        }

        [HttpPost("Create")]
        public async Task<ActionResult<Guid>> Create(DealAttribute attribute, CancellationToken ct = default)
        {
            attribute.AccountId = _userContext.AccountId;

            var id = await _dealAttributesService.CreateAsync(_userContext.UserId, attribute, ct);

            return Created(nameof(Get), id);
        }

        [HttpPost("Update")]
        public async Task<ActionResult> Update(DealAttribute attribute, CancellationToken ct = default)
        {
            var oldAttribute = await _dealAttributesService.GetAsync(attribute.Id, ct);
            if (oldAttribute == null)
            {
                return NotFound(attribute.Id);
            }

            return await ActionIfAllowed(
                () => _dealAttributesService.UpdateAsync(_userContext.UserId, oldAttribute, attribute, ct),
                Roles.Sales,
                attribute.AccountId, oldAttribute.AccountId);
        }

        [HttpPost("Delete")]
        public async Task<ActionResult> Delete([Required] List<Guid> ids, CancellationToken ct = default)
        {
            var attributes = await _dealAttributesService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _dealAttributesService.DeleteAsync(_userContext.UserId, attributes.Select(x => x.Id), ct),
                Roles.Sales,
                attributes.Select(x => x.AccountId));
        }

        [HttpPost("Restore")]
        public async Task<ActionResult> Restore([Required] List<Guid> ids, CancellationToken ct = default)
        {
            var attributes = await _dealAttributesService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _dealAttributesService.RestoreAsync(_userContext.UserId, attributes.Select(x => x.Id), ct),
                Roles.Sales,
                attributes.Select(x => x.AccountId));
        }
    }
}