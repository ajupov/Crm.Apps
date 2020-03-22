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
using Crm.Apps.Companies.v1.Requests;
using Crm.Apps.Companies.v1.Responses;
using Crm.Common.All.BaseControllers;
using Crm.Common.All.Roles;
using Crm.Common.All.Roles.Attributes;
using Crm.Common.All.UserContext;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Companies.v1.Controllers
{
    [ApiController]
    [RequestContentTypeApplicationJson]
    [RequireSalesRole(JwtDefaults.AuthenticationScheme)]
    [Route("Companies/v1")]
    public class CompaniesController : AllowingCheckControllerBase
    {
        private readonly IUserContext _userContext;
        private readonly ICompaniesService _companiesService;

        public CompaniesController(IUserContext userContext, ICompaniesService companiesService)
            : base(userContext)
        {
            _userContext = userContext;
            _companiesService = companiesService;
        }

        [HttpGet("GetTypes")]
        public Dictionary<string, CompanyType> GetTypes()
        {
            return EnumsExtensions.GetAsDictionary<CompanyType>();
        }

        [HttpGet("GetIndustryTypes")]
        public Dictionary<string, CompanyIndustryType> GetIndustryTypes()
        {
            return EnumsExtensions.GetAsDictionary<CompanyIndustryType>();
        }

        [HttpGet("Get")]
        public async Task<ActionResult<Company>> Get([Required] Guid id, CancellationToken ct = default)
        {
            var company = await _companiesService.GetAsync(id, ct);
            if (company == null)
            {
                return NotFound(id);
            }

            return ReturnIfAllowed(company, Roles.Sales, company.AccountId);
        }

        [HttpPost("GetList")]
        public async Task<ActionResult<List<Company>>> GetList(
            [Required] List<Guid> ids,
            CancellationToken ct = default)
        {
            var companies = await _companiesService.GetListAsync(ids, ct);

            return ReturnIfAllowed(
                companies,
                Roles.Sales,
                companies.Select(x => x.AccountId));
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<CompanyGetPagedListResponse>> GetPagedList(
            CompanyGetPagedListRequest request,
            CancellationToken ct = default)
        {
            var response = await _companiesService.GetPagedListAsync(_userContext.AccountId, request, ct);

            return ReturnIfAllowed(
                response,
                Roles.Sales,
                response.Companies.Select(x => x.AccountId));
        }

        [HttpPost("Create")]
        public async Task<ActionResult<Guid>> Create(Company company, CancellationToken ct = default)
        {
            company.AccountId = _userContext.AccountId;

            var id = await _companiesService.CreateAsync(_userContext.UserId, company, ct);

            return Created(nameof(Get), id);
        }

        [HttpPost("Update")]
        public async Task<ActionResult> Update(Company company, CancellationToken ct = default)
        {
            var oldCompany = await _companiesService.GetAsync(company.Id, ct);
            if (oldCompany == null)
            {
                return NotFound(company.Id);
            }

            return await ActionIfAllowed(
                () => _companiesService.UpdateAsync(_userContext.UserId, oldCompany, company, ct),
                Roles.Sales,
                company.AccountId, oldCompany.AccountId);
        }

        [HttpPost("Delete")]
        public async Task<ActionResult> Delete([Required] List<Guid> ids, CancellationToken ct = default)
        {
            var companies = await _companiesService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _companiesService.DeleteAsync(_userContext.UserId, companies.Select(x => x.Id), ct),
                Roles.Sales,
                companies.Select(x => x.AccountId));
        }

        [HttpPost("Restore")]
        public async Task<ActionResult> Restore([Required] List<Guid> ids, CancellationToken ct = default)
        {
            var companies = await _companiesService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _companiesService.RestoreAsync(_userContext.UserId, companies.Select(x => x.Id), ct),
                Roles.Sales,
                companies.Select(x => x.AccountId));
        }
    }
}