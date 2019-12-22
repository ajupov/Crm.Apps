using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Companies.Models;
using Crm.Apps.Areas.Companies.Parameters;
using Crm.Apps.Areas.Companies.Services;
using Crm.Common.UserContext;
using Crm.Common.UserContext.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Areas.Companies.Controllers
{
    [ApiController]
    [Route("Api/Companies")]
    public class CompaniesController : ControllerBase
    {
        private readonly IUserContext _userContext;
        private readonly ICompaniesService _companiesService;

        public CompaniesController(IUserContext userContext, ICompaniesService companiesService)
        {
            _userContext = userContext;
            _companiesService = companiesService;
        }

        [HttpGet("GetTypes")]
        [RequireAny(Role.System, Role.Development, Role.Administration, Role.TechnicalSupport,
            Role.AccountOwning, Role.SalesManagement)]
        public ActionResult<List<CompanyType>> GetTypes()
        {
            return EnumsExtensions.GetValues<CompanyType>().ToList();
        }

        [HttpGet("GetIndustryTypes")]
        [RequireAny(Role.System, Role.Development, Role.Administration, Role.TechnicalSupport,
            Role.AccountOwning, Role.SalesManagement)]
        public ActionResult<List<CompanyIndustryType>> GetIndustryTypes()
        {
            return EnumsExtensions.GetValues<CompanyIndustryType>().ToList();
        }

        [HttpGet("Get")]
        [RequireAny(Role.System, Role.Development, Role.Administration, Role.TechnicalSupport,
            Role.AccountOwning, Role.SalesManagement)]
        public async Task<ActionResult<Company>> Get(Guid id, CancellationToken ct = default)
        {
            if (id.IsEmpty())
            {
                return BadRequest();
            }

            var company = await _companiesService.GetAsync(id, ct);
            if (company == null)
            {
                return NotFound();
            }

            return ReturnIfAllowed(company, new[] {company.AccountId});
        }

        [HttpPost("GetList")]
        [RequireAny(Role.System, Role.Development, Role.Administration, Role.TechnicalSupport,
            Role.AccountOwning, Role.SalesManagement)]
        public async Task<ActionResult<List<Company>>> GetList(List<Guid> ids, CancellationToken ct = default)
        {
            if (ids == null || ids.All(x => x.IsEmpty()))
            {
                return BadRequest();
            }

            var companies = await _companiesService.GetListAsync(ids, ct);

            return ReturnIfAllowed(companies, companies.Select(x => x.AccountId));
        }

        [HttpPost("GetPagedList")]
        [RequireAny(Role.System, Role.Development, Role.Administration, Role.TechnicalSupport,
            Role.AccountOwning, Role.SalesManagement)]
        public async Task<ActionResult<List<Company>>> GetPagedList(CompanyGetPagedListParameter parameter,
            CancellationToken ct = default)
        {
            var companies = await _companiesService.GetPagedListAsync(parameter, ct);

            return ReturnIfAllowed(companies, companies.Select(x => x.AccountId));
        }

        [HttpPost("Create")]
        [RequireAny(Role.System, Role.Development, Role.Administration, Role.AccountOwning,
            Role.SalesManagement)]
        public async Task<ActionResult<Guid>> Create(Company company, CancellationToken ct = default)
        {
            if (company == null)
            {
                return BadRequest();
            }

            if (!_userContext.HasAny(Role.System, Role.Development, Role.Administration,
                Role.TechnicalSupport))
            {
                company.AccountId = _userContext.AccountId;
            }

            var id = await _companiesService.CreateAsync(_userContext.UserId, company, ct);

            return Created(nameof(Get), id);
        }

        [HttpPost("Update")]
        [RequireAny(Role.System, Role.Development, Role.Administration, Role.TechnicalSupport,
            Role.AccountOwning, Role.SalesManagement)]
        public async Task<ActionResult> Update(Company company, CancellationToken ct = default)
        {
            if (company.Id.IsEmpty())
            {
                return BadRequest();
            }

            var oldCompany = await _companiesService.GetAsync(company.Id, ct);
            if (oldCompany == null)
            {
                return NotFound();
            }

            return await ActionIfAllowed(
                () => _companiesService.UpdateAsync(_userContext.UserId, oldCompany, company, ct),
                new[] {company.AccountId, oldCompany.AccountId});
        }

        [HttpPost("Delete")]
        [RequireAny(Role.System, Role.Development, Role.Administration, Role.TechnicalSupport,
            Role.AccountOwning, Role.SalesManagement)]
        public async Task<ActionResult> Delete(List<Guid> ids, CancellationToken ct = default)
        {
            if (ids == null || ids.All(x => x.IsEmpty()))
            {
                return BadRequest();
            }

            var companies = await _companiesService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _companiesService.DeleteAsync(_userContext.UserId, companies.Select(x => x.Id), ct),
                companies.Select(x => x.AccountId));
        }

        [HttpPost("Restore")]
        [RequireAny(Role.System, Role.Development, Role.Administration, Role.TechnicalSupport,
            Role.AccountOwning, Role.SalesManagement)]
        public async Task<ActionResult> Restore(List<Guid> ids, CancellationToken ct = default)
        {
            if (ids == null || ids.All(x => x.IsEmpty()))
            {
                return BadRequest();
            }

            var companies = await _companiesService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _companiesService.RestoreAsync(_userContext.UserId, companies.Select(x => x.Id), ct),
                companies.Select(x => x.AccountId));
        }

        [NonAction]
        private ActionResult<TResult> ReturnIfAllowed<TResult>(TResult result, IEnumerable<Guid> accountIds)
        {
            if (_userContext.HasAny(Role.System, Role.Development, Role.Administration,
                Role.TechnicalSupport))
            {
                return result;
            }

            var accountIdsAsArray = accountIds.ToArray();

            if (_userContext.HasAny(Role.AccountOwning, Role.SalesManagement) &&
                _userContext.Belongs(accountIdsAsArray))
            {
                return result;
            }

            if (_userContext.HasAny(Role.AccountOwning, Role.SalesManagement) &&
                !_userContext.Belongs(accountIdsAsArray))
            {
                return Forbid();
            }

            throw new Exception();
        }

        [NonAction]
        private async Task<ActionResult> ActionIfAllowed(Func<Task> action, IEnumerable<Guid> accountIds)
        {
            if (_userContext.HasAny(Role.System, Role.Development, Role.Administration,
                Role.TechnicalSupport))
            {
                await action();

                return NoContent();
            }

            var accountIdsAsArray = accountIds.ToArray();

            if (_userContext.HasAny(Role.AccountOwning, Role.SalesManagement) &&
                _userContext.Belongs(accountIdsAsArray))
            {
                await action();

                return NoContent();
            }

            if (_userContext.HasAny(Role.AccountOwning, Role.SalesManagement) &&
                !_userContext.Belongs(accountIdsAsArray))
            {
                return Forbid();
            }

            throw new Exception();
        }
    }
}