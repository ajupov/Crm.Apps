using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Users.Models;
using Crm.Apps.Areas.Users.RequestParameters;
using Crm.Apps.Areas.Users.Services;
using Crm.Apps.Utils;
using Crm.Common.Types;
using Crm.Common.UserContext;
using Crm.Common.UserContext.Attributes;
using Crm.Common.UserContext.Extensions;
using Crm.Infrastructure.ApiDocumentation.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Areas.Users.Controllers
{
    [ApiController]
    [RequirePrivileged]
    [IgnoreApiDocumentation]
    [Route("Api/Users/Attributes")]
    public class UserAttributesController : UserContextController
    {
        private readonly IUserContext _userContext;
        private readonly IUserAttributesService _userAttributesService;

        public UserAttributesController(IUserContext userContext, IUserAttributesService userAttributesService)
            : base(userContext)
        {
            _userContext = userContext;
            _userAttributesService = userAttributesService;
        }

        [HttpGet("GetTypes")]
        public ActionResult<Dictionary<string, AttributeType>> GetTypes()
        {
            return EnumsExtensions.GetAsDictionary<AttributeType>();
        }

        [HttpGet("Get")]
        public async Task<ActionResult<UserAttribute>> Get([Required] Guid id, CancellationToken ct = default)
        {
            var attribute = await _userAttributesService.GetAsync(id, ct);
            if (attribute == null)
            {
                return NotFound(id);
            }

            return ReturnIfAllowed(attribute, Role.AccountOwning, attribute.AccountId);
        }

        [HttpPost("GetList")]
        public async Task<ActionResult<List<UserAttribute>>> GetList(
            [Required] List<Guid> ids,
            CancellationToken ct = default)
        {
            var attributes = await _userAttributesService.GetListAsync(ids, ct);

            return ReturnIfAllowed(attributes, Role.AccountOwning, attributes.Select(x => x.AccountId));
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<List<UserAttribute>>> GetPagedList(
            UserAttributeGetPagedListRequestParameter request,
            CancellationToken ct = default)
        {
            request.AccountId = _userContext.AccountId;

            var attributes = await _userAttributesService.GetPagedListAsync(request, ct);

            return ReturnIfAllowed(attributes, Role.AccountOwning, attributes.Select(x => x.AccountId));
        }

        [HttpPost("Create")]
        public async Task<ActionResult<Guid>> Create(UserAttribute attribute, CancellationToken ct = default)
        {
            attribute.AccountId = _userContext.AccountId;

            var id = await _userAttributesService.CreateAsync(_userContext.UserId, attribute, ct);

            return Created(nameof(Get), id);
        }

        [HttpPost("Update")]
        public async Task<ActionResult> Update(UserAttribute attribute, CancellationToken ct = default)
        {
            var oldAttribute = await _userAttributesService.GetAsync(attribute.Id, ct);
            if (oldAttribute == null)
            {
                return NotFound(attribute.Id);
            }

            return await ActionIfAllowed(
                () => _userAttributesService.UpdateAsync(_userContext.UserId, oldAttribute, attribute, ct),
                Role.AccountOwning,
                attribute.AccountId, oldAttribute.AccountId);
        }

        [HttpPost("Delete")]
        public async Task<ActionResult> Delete([Required] List<Guid> ids, CancellationToken ct = default)
        {
            var attributes = await _userAttributesService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _userAttributesService.DeleteAsync(_userContext.UserId, attributes.Select(x => x.Id), ct),
                Role.AccountOwning,
                attributes.Select(x => x.AccountId));
        }

        [HttpPost("Restore")]
        public async Task<ActionResult> Restore([Required] List<Guid> ids, CancellationToken ct = default)
        {
            var attributes = await _userAttributesService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _userAttributesService.RestoreAsync(_userContext.UserId, attributes.Select(x => x.Id), ct),
                Role.AccountOwning,
                attributes.Select(x => x.AccountId));
        }
    }
}