using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Users.Models;
using Crm.Apps.Areas.Users.Parameters;
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

        [RequirePrivileged]
        [HttpGet("GetTypes")]
        public ActionResult<Dictionary<string, AttributeType>> GetTypes()
        {
            return EnumsExtensions.GetAsDictionary<AttributeType>();
        }

        [RequirePrivileged]
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

        [RequirePrivileged]
        [HttpPost("GetList")]
        public async Task<ActionResult<List<UserAttribute>>> GetList(
            [Required] List<Guid> ids,
            CancellationToken ct = default)
        {
            var attributes = await _userAttributesService.GetListAsync(ids, ct);

            return ReturnIfAllowed(attributes, Role.AccountOwning, attributes.Select(x => x.AccountId));
        }

        [RequirePrivileged]
        [HttpPost("GetPagedList")]
        public async Task<ActionResult<List<UserAttribute>>> GetPagedList(
            UserAttributeGetPagedListParameter parameter,
            CancellationToken ct = default)
        {
            var attributes = await _userAttributesService.GetPagedListAsync(parameter, ct);

            return ReturnIfAllowed(attributes, Role.AccountOwning, attributes.Select(x => x.AccountId));
        }

        [RequirePrivileged]
        [HttpPost("Create")]
        public async Task<ActionResult<Guid>> Create(UserAttribute attribute, CancellationToken ct = default)
        {
            var id = await _userAttributesService.CreateAsync(_userContext.UserId, attribute, ct);

            return Created(nameof(Get), id);
        }

        [RequirePrivileged]
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

        [RequirePrivileged]
        [HttpPost("Delete")]
        public async Task<ActionResult> Delete([Required] List<Guid> ids, CancellationToken ct = default)
        {
            var attributes = await _userAttributesService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _userAttributesService.DeleteAsync(_userContext.UserId, attributes.Select(x => x.Id), ct),
                Role.AccountOwning,
                attributes.Select(x => x.AccountId));
        }

        [RequirePrivileged]
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