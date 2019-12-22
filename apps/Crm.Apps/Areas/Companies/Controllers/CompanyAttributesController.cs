using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Companies.Models;
using Crm.Apps.Areas.Companies.Parameters;
using Crm.Apps.Areas.Companies.Services;
using Crm.Apps.Utils;
using Crm.Common.Types;
using Crm.Common.UserContext;
using Crm.Common.UserContext.Attributes;
using Crm.Common.UserContext.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Areas.Companies.Controllers
{
    [ApiController]
    [RequirePrivileged(Role.AccountOwning, Role.SalesManagement)]
    [Route("Api/Companies/Attributes")]
    public class CompanyAttributesController : UserContextController
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
        public Dictionary<string, AttributeType> GetTypes()
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

            return ReturnIfAllowed(attribute, new[] {Role.AccountOwning, Role.SalesManagement}, attribute.AccountId);
        }

        [HttpPost("GetList")]
        public async Task<ActionResult<List<CompanyAttribute>>> GetList(
            [Required] List<Guid> ids,
            CancellationToken ct = default)
        {
            var attributes = await _companyAttributesService.GetListAsync(ids, ct);

            return ReturnIfAllowed(
                attributes,
                new[] {Role.AccountOwning, Role.SalesManagement},
                attributes.Select(x => x.AccountId));
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<List<CompanyAttribute>>> GetPagedList(
            CompanyAttributeGetPagedListParameter parameter,
            CancellationToken ct = default)
        {
            var attributes = await _companyAttributesService.GetPagedListAsync(parameter, ct);

            return ReturnIfAllowed(
                attributes,
                new[] {Role.AccountOwning, Role.SalesManagement},
                attributes.Select(x => x.AccountId));
        }

        [HttpPost("Create")]
        public async Task<ActionResult<Guid>> Create(CompanyAttribute attribute, CancellationToken ct = default)
        {
            attribute.AccountId = _userContext.AccountId;

            var id = await _companyAttributesService.CreateAsync(_userContext.UserId, attribute, ct);

            return Created(nameof(Get), id);
        }

        [HttpPost("Update")]
        public async Task<ActionResult> Update(CompanyAttribute attribute, CancellationToken ct = default)
        {
            var oldAttribute = await _companyAttributesService.GetAsync(attribute.Id, ct);
            if (oldAttribute == null)
            {
                return NotFound(attribute.Id);
            }

            return await ActionIfAllowed(
                () => _companyAttributesService.UpdateAsync(_userContext.UserId, oldAttribute, attribute, ct),
                new[] {Role.AccountOwning, Role.SalesManagement},
                attribute.AccountId, oldAttribute.AccountId);
        }

        [HttpPost("Delete")]
        public async Task<ActionResult> Delete([Required] List<Guid> ids, CancellationToken ct = default)
        {
            var attributes = await _companyAttributesService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _companyAttributesService.DeleteAsync(_userContext.UserId, attributes.Select(x => x.Id), ct),
                new[] {Role.AccountOwning, Role.SalesManagement},
                attributes.Select(x => x.AccountId));
        }

        [HttpPost("Restore")]
        public async Task<ActionResult> Restore([Required] List<Guid> ids, CancellationToken ct = default)
        {
            var attributes = await _companyAttributesService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _companyAttributesService.RestoreAsync(_userContext.UserId, attributes.Select(x => x.Id), ct),
                new[] {Role.AccountOwning, Role.SalesManagement},
                attributes.Select(x => x.AccountId));
        }
    }
}