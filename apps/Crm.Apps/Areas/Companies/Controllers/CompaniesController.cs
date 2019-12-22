using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Companies.Models;
using Crm.Apps.Areas.Companies.RequestParameters;
using Crm.Apps.Areas.Companies.Services;
using Crm.Apps.Utils;
using Crm.Common.UserContext;
using Crm.Common.UserContext.Attributes;
using Crm.Common.UserContext.BaseControllers;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Areas.Companies.Controllers
{
    [ApiController]
    [RequirePrivileged(Role.AccountOwning, Role.SalesManagement)]
    [Route("Api/Companies")]
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
        public Dictionary<string, CompanyType> GetIndustryTypes()
        {
            return EnumsExtensions.GetAsDictionary<CompanyType>();
        }

        [HttpGet("Get")]
        public async Task<ActionResult<Company>> Get([Required] Guid id, CancellationToken ct = default)
        {
            var company = await _companiesService.GetAsync(id, ct);
            if (company == null)
            {
                return NotFound(id);
            }

            return ReturnIfAllowed(company, new[] {Role.AccountOwning, Role.SalesManagement}, company.AccountId);
        }

        [HttpPost("GetList")]
        public async Task<ActionResult<List<Company>>> GetList(
            [Required] List<Guid> ids,
            CancellationToken ct = default)
        {
            var companies = await _companiesService.GetListAsync(ids, ct);

            return ReturnIfAllowed(
                companies,
                new[] {Role.AccountOwning, Role.SalesManagement},
                companies.Select(x => x.AccountId));
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<List<Company>>> GetPagedList(
            CompanyGetPagedListRequestParameter request,
            CancellationToken ct = default)
        {
            request.AccountId = _userContext.AccountId;

            var companies = await _companiesService.GetPagedListAsync(request, ct);

            return ReturnIfAllowed(
                companies,
                new[] {Role.AccountOwning, Role.SalesManagement},
                companies.Select(x => x.AccountId));
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
                new[] {Role.AccountOwning, Role.SalesManagement},
                company.AccountId, oldCompany.AccountId);
        }

        [HttpPost("Delete")]
        public async Task<ActionResult> Delete([Required] List<Guid> ids, CancellationToken ct = default)
        {
            var companies = await _companiesService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _companiesService.DeleteAsync(_userContext.UserId, companies.Select(x => x.Id), ct),
                new[] {Role.AccountOwning, Role.SalesManagement},
                companies.Select(x => x.AccountId));
        }

        [HttpPost("Restore")]
        public async Task<ActionResult> Restore([Required] List<Guid> ids, CancellationToken ct = default)
        {
            var companies = await _companiesService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _companiesService.RestoreAsync(_userContext.UserId, companies.Select(x => x.Id), ct),
                new[] {Role.AccountOwning, Role.SalesManagement},
                companies.Select(x => x.AccountId));
        }
    }
}