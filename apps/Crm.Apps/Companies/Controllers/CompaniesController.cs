using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Companies.Models;
using Crm.Apps.Companies.Parameters;
using Crm.Apps.Companies.Services;
using Crm.Common.UserContext;
using Crm.Common.UserContext.Attributes;
using Crm.Utils.Enums;
using Crm.Utils.Guid;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Companies.Controllers
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
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.TechnicalSupport,
            Permission.AccountOwning, Permission.SalesManagement)]
        public ActionResult<List<CompanyType>> GetTypes()
        {
            return EnumsExtensions.GetValues<CompanyType>().ToList();
        }

        [HttpGet("GetIndustryTypes")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.TechnicalSupport,
            Permission.AccountOwning, Permission.SalesManagement)]
        public ActionResult<List<CompanyIndustryType>> GetIndustryTypes()
        {
            return EnumsExtensions.GetValues<CompanyIndustryType>().ToList();
        }

        [HttpGet("Get")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.TechnicalSupport,
            Permission.AccountOwning, Permission.SalesManagement)]
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
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.TechnicalSupport,
            Permission.AccountOwning, Permission.SalesManagement)]
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
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.TechnicalSupport,
            Permission.AccountOwning, Permission.SalesManagement)]
        public async Task<ActionResult<List<Company>>> GetPagedList(CompanyGetPagedListParameter parameter,
            CancellationToken ct = default)
        {
            var companies = await _companiesService.GetPagedListAsync(parameter, ct);

            return ReturnIfAllowed(companies, companies.Select(x => x.AccountId));
        }

        [HttpPost("Create")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.AccountOwning,
            Permission.SalesManagement)]
        public async Task<ActionResult<Guid>> Create(Company company, CancellationToken ct = default)
        {
            if (company == null)
            {
                return BadRequest();
            }

            if (!_userContext.HasAny(Permission.System, Permission.Development, Permission.Administration,
                Permission.TechnicalSupport))
            {
                company.AccountId = _userContext.AccountId;
            }

            var id = await _companiesService.CreateAsync(_userContext.UserId, company, ct);

            return Created(nameof(Get), id);
        }

        [HttpPost("Update")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.TechnicalSupport,
            Permission.AccountOwning, Permission.SalesManagement)]
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
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.TechnicalSupport,
            Permission.AccountOwning, Permission.SalesManagement)]
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
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.TechnicalSupport,
            Permission.AccountOwning, Permission.SalesManagement)]
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
            if (_userContext.HasAny(Permission.System, Permission.Development, Permission.Administration,
                Permission.TechnicalSupport))
            {
                return result;
            }

            var accountIdsAsArray = accountIds.ToArray();

            if (_userContext.HasAny(Permission.AccountOwning, Permission.SalesManagement) &&
                _userContext.Belongs(accountIdsAsArray))
            {
                return result;
            }

            if (_userContext.HasAny(Permission.AccountOwning, Permission.SalesManagement) &&
                !_userContext.Belongs(accountIdsAsArray))
            {
                return Forbid();
            }

            throw new Exception();
        }

        [NonAction]
        private async Task<ActionResult> ActionIfAllowed(Func<Task> action, IEnumerable<Guid> accountIds)
        {
            if (_userContext.HasAny(Permission.System, Permission.Development, Permission.Administration,
                Permission.TechnicalSupport))
            {
                await action();

                return NoContent();
            }

            var accountIdsAsArray = accountIds.ToArray();

            if (_userContext.HasAny(Permission.AccountOwning, Permission.SalesManagement) &&
                _userContext.Belongs(accountIdsAsArray))
            {
                await action();

                return NoContent();
            }

            if (_userContext.HasAny(Permission.AccountOwning, Permission.SalesManagement) &&
                !_userContext.Belongs(accountIdsAsArray))
            {
                return Forbid();
            }

            throw new Exception();
        }
    }
}