using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Clients.Companies.Models;

namespace Crm.Clients.Companies.Clients
{
    public interface ICompaniesClient
    {
        Task<List<CompanyType>> GetTypesAsync(CancellationToken ct = default);
        
        Task<List<CompanyIndustryType>> GetIndustryTypesAsync(CancellationToken ct = default);

        Task<Company> GetAsync(Guid id, CancellationToken ct = default);

        Task<List<Company>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct = default);

        Task<List<Company>> GetPagedListAsync(Guid? accountId = default, Guid? companyId = default,
            string fullName = default, string shortName = default, string phone = default, string email = default,
            string taxNumber = default, string registrationNumber = default, DateTime? minRegistrationDate = default,
            DateTime? maxRegistrationDate = default, int? minEmployeesCount = default, int? maxEmployeesCount = default,
            decimal? minYearlyTurnover = default, decimal? maxYearlyTurnover = default,
            string juridicalPostcode = default, string juridicalCountry = default, string juridicalRegion = default,
            string juridicalProvince = default, string juridicalCity = default, string juridicalStreet = default,
            string juridicalHouse = default, string juridicalApartment = default, string legalPostcode = default,
            string legalCountry = default, string legalRegion = default, string legalProvince = default,
            string legalCity = default, string legalStreet = default, string legalHouse = default,
            string legalApartment = default, bool isDeleted = default, DateTime? minCreateDate = default,
            DateTime? maxCreateDate = default, List<CompanyType> types = default,
            List<CompanyIndustryType> industryTypes = default, bool? allAttributes = default,
            IDictionary<Guid, string> attributes = default, string bankAccountNumber = default,
            string bankAccountBankNumber = default, string bankAccountBankCorrespondentNumber = default,
            string bankAccountBankName = default, List<Guid> sourceIds = default, List<Guid> createUserIds = default,
            List<Guid> responsibleUserIds = default, int offset = default, int limit = 10, string sortBy = default,
            string orderBy = default, CancellationToken ct = default);

        Task<Guid> CreateAsync(Company lead, CancellationToken ct = default);

        Task UpdateAsync(Company lead, CancellationToken ct = default);

        Task DeleteAsync(IEnumerable<Guid> ids, CancellationToken ct = default);

        Task RestoreAsync(IEnumerable<Guid> ids, CancellationToken ct = default);
    }
}