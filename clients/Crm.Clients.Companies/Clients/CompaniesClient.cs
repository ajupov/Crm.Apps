using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Crm.Clients.Companies.Models;
using Crm.Clients.Companies.Settings;
using Crm.Utils.Http;
using Microsoft.Extensions.Options;

namespace Crm.Clients.Companies.Clients
{
    public class CompaniesClient : ICompaniesClient
    {
        private readonly CompaniesClientSettings _settings;
        private readonly IHttpClientFactory _httpClientFactory;

        public CompaniesClient(IOptions<CompaniesClientSettings> options, IHttpClientFactory httpClientFactory)
        {
            _settings = options.Value;
            _httpClientFactory = httpClientFactory;
        }

        public Task<Company> GetAsync(Guid id, CancellationToken ct = default)
        {
            return _httpClientFactory.GetAsync<Company>($"{_settings.Host}/Api/Companies/Get", new {id}, ct);
        }

        public Task<List<Company>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync<List<Company>>($"{_settings.Host}/Api/Companies/GetList", ids, ct);
        }

        public Task<List<Company>> GetPagedListAsync(Guid? accountId = default, Guid? companyId = default,
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
            string orderBy = default, CancellationToken ct = default)
        {
            var parameter = new
            {
                AccountId = accountId,
                LeadId = companyId,
                FullName = fullName,
                ShortName = shortName,
                Phone = phone,
                Email = email,
                TaxNumber = taxNumber,
                RegistrationNumber = registrationNumber,
                MinRegistrationDate = minRegistrationDate,
                MaxRegistrationDate = maxRegistrationDate,
                MinEmployeesCount = minEmployeesCount,
                MaxEmployeesCount = maxEmployeesCount,
                MinYearlyTurnover = minYearlyTurnover,
                MaxYearlyTurnover = maxYearlyTurnover,
                JuridicalPostcode = juridicalPostcode,
                JuridicalCountry = juridicalCountry,
                JuridicalRegion = juridicalRegion,
                JuridicalProvince = juridicalProvince,
                JuridicalCity = juridicalCity,
                JuridicalStreet = juridicalStreet,
                JuridicalHouse = juridicalHouse,
                JuridicalApartment = juridicalApartment,
                LegalPostcode = legalPostcode,
                LegalCountry = legalCountry,
                LegalRegion = legalRegion,
                LegalProvince = legalProvince,
                LegalCity = legalCity,
                LegalStreet = legalStreet,
                LegalHouse = legalHouse,
                LegalApartment = legalApartment,
                IsDeleted = isDeleted,
                MinCreateDate = minCreateDate,
                MaxCreateDate = maxCreateDate,
                Types = types,
                IndustryTypes = industryTypes,
                AllAttributes = allAttributes,
                Attributes = attributes,
                BankAccountNumber = bankAccountNumber,
                BankAccountBankNumber = bankAccountBankNumber,
                BankAccountBankCorrespondentNumber = bankAccountBankCorrespondentNumber,
                BankAccountBankName = bankAccountBankName,
                SourceIds = sourceIds,
                CreateUserIds = createUserIds,
                ResponsibleUserIds = responsibleUserIds,
                Offset = offset,
                Limit = limit,
                SortBy = sortBy,
                OrderBy = orderBy
            };

            return _httpClientFactory.PostAsync<List<Company>>($"{_settings.Host}/Api/Companies/GetPagedList",
                parameter, ct);
        }

        public Task<Guid> CreateAsync(Company company, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync<Guid>($"{_settings.Host}/Api/Companies/Create", company, ct);
        }

        public Task UpdateAsync(Company company, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_settings.Host}/Api/Companies/Update", company, ct);
        }

        public Task DeleteAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_settings.Host}/Api/Companies/Delete", ids, ct);
        }

        public Task RestoreAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_settings.Host}/Api/Companies/Restore", ids, ct);
        }
    }
}