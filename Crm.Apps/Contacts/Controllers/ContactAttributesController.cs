using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Utils.All.Enums;
using Crm.Apps.Contacts.Models;
using Crm.Apps.Contacts.RequestParameters;
using Crm.Apps.Contacts.Services;
using Crm.Common.All.Types.AttributeType;
using Crm.Common.All.UserContext;
using Crm.Common.All.UserContext.Attributes;
using Crm.Common.All.UserContext.BaseControllers;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Contacts.Controllers
{
    [ApiController]
    [RequirePrivileged(Role.AccountOwning, Role.SalesManagement)]
    [Route("Api/Contacts/Attributes")]
    public class ContactAttributesController : AllowingCheckControllerBase
    {
        private readonly IUserContext _userContext;
        private readonly IContactAttributesService _contactAttributesService;

        public ContactAttributesController(IUserContext userContext, IContactAttributesService contactAttributesService)
            : base(userContext)
        {
            _userContext = userContext;
            _contactAttributesService = contactAttributesService;
        }

        [HttpGet("GetTypes")]
        public Dictionary<string, AttributeType> GetTypes()
        {
            return EnumsExtensions.GetAsDictionary<AttributeType>();
        }

        [HttpGet("Get")]
        public async Task<ActionResult<ContactAttribute>> Get([Required] Guid id, CancellationToken ct = default)
        {
            var attribute = await _contactAttributesService.GetAsync(id, ct);
            if (attribute == null)
            {
                return NotFound(id);
            }

            return ReturnIfAllowed(attribute, new[] {Role.AccountOwning, Role.SalesManagement}, attribute.AccountId);
        }

        [HttpPost("GetList")]
        public async Task<ActionResult<List<ContactAttribute>>> GetList(
            [Required] List<Guid> ids,
            CancellationToken ct = default)
        {
            var attributes = await _contactAttributesService.GetListAsync(ids, ct);

            return ReturnIfAllowed(
                attributes,
                new[] {Role.AccountOwning, Role.SalesManagement},
                attributes.Select(x => x.AccountId));
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<List<ContactAttribute>>> GetPagedList(
            ContactAttributeGetPagedListRequestParameter request,
            CancellationToken ct = default)
        {
            request.AccountId = _userContext.AccountId;

            var attributes = await _contactAttributesService.GetPagedListAsync(request, ct);

            return ReturnIfAllowed(
                attributes,
                new[] {Role.AccountOwning, Role.SalesManagement},
                attributes.Select(x => x.AccountId));
        }

        [HttpPost("Create")]
        public async Task<ActionResult<Guid>> Create(ContactAttribute attribute, CancellationToken ct = default)
        {
            attribute.AccountId = _userContext.AccountId;

            var id = await _contactAttributesService.CreateAsync(_userContext.UserId, attribute, ct);

            return Created(nameof(Get), id);
        }

        [HttpPost("Update")]
        public async Task<ActionResult> Update(ContactAttribute attribute, CancellationToken ct = default)
        {
            var oldAttribute = await _contactAttributesService.GetAsync(attribute.Id, ct);
            if (oldAttribute == null)
            {
                return NotFound(attribute.Id);
            }

            return await ActionIfAllowed(
                () => _contactAttributesService.UpdateAsync(_userContext.UserId, oldAttribute, attribute, ct),
                new[] {Role.AccountOwning, Role.SalesManagement},
                attribute.AccountId, oldAttribute.AccountId);
        }

        [HttpPost("Delete")]
        public async Task<ActionResult> Delete([Required] List<Guid> ids, CancellationToken ct = default)
        {
            var attributes = await _contactAttributesService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _contactAttributesService.DeleteAsync(_userContext.UserId, attributes.Select(x => x.Id), ct),
                new[] {Role.AccountOwning, Role.SalesManagement},
                attributes.Select(x => x.AccountId));
        }

        [HttpPost("Restore")]
        public async Task<ActionResult> Restore([Required] List<Guid> ids, CancellationToken ct = default)
        {
            var attributes = await _contactAttributesService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _contactAttributesService.RestoreAsync(_userContext.UserId, attributes.Select(x => x.Id), ct),
                new[] {Role.AccountOwning, Role.SalesManagement},
                attributes.Select(x => x.AccountId));
        }
    }
}