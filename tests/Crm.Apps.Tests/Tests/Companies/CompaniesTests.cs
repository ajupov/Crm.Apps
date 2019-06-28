using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Crm.Apps.Tests.Dsl.Creator;
using Crm.Clients.Companies.Clients;
using Crm.Clients.Companies.Models;
using Crm.Utils.DateTime;
using Crm.Utils.Guid;
using Xunit;

namespace Crm.Apps.Tests.Tests.Companies
{
    public class CompanyTests
    {
        private readonly ICreate _create;
        private readonly ICompaniesClient _companiesClient;

        public CompanyTests(ICreate create, ICompaniesClient companiesClient)
        {
            _create = create;
            _companiesClient = companiesClient;
        }

        [Fact]
        public async Task WhenGetTypes_ThenSuccess()
        {
            var types = await _companiesClient.GetTypesAsync().ConfigureAwait(false);

            Assert.NotEmpty(types);
        }

        [Fact]
        public async Task WhenGetIndustryTypes_ThenSuccess()
        {
            var types = await _companiesClient.GetIndustryTypesAsync().ConfigureAwait(false);

            Assert.NotEmpty(types);
        }

        [Fact]
        public async Task WhenGet_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            var companyId = (await _create.Company.WithAccountId(account.Id).BuildAsync().ConfigureAwait(false)).Id;

            var company = await _companiesClient.GetAsync(companyId).ConfigureAwait(false);

            Assert.NotNull(company);
            Assert.Equal(companyId, company.Id);
        }

        [Fact]
        public async Task WhenGetList_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            var companyIds = (await Task.WhenAll(
                    _create.Company.WithAccountId(account.Id).BuildAsync(),
                    _create.Company.WithAccountId(account.Id).BuildAsync())
                .ConfigureAwait(false)).Select(x => x.Id).ToList();

            var companies = await _companiesClient.GetListAsync(companyIds).ConfigureAwait(false);

            Assert.NotEmpty(companies);
            Assert.Equal(companyIds.Count, companies.Count);
        }

        [Fact]
        public async Task WhenGetPagedList_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            var attribute = await _create.CompanyAttribute.WithAccountId(account.Id).BuildAsync().ConfigureAwait(false);

            await Task.WhenAll(
                    _create.Company.WithAccountId(account.Id).WithAttributeLink(attribute.Id, "Test").BuildAsync(),
                    _create.Company.WithAccountId(account.Id).WithAttributeLink(attribute.Id, "Test").BuildAsync())
                .ConfigureAwait(false);
            var filterAttributes = new Dictionary<Guid, string> {{attribute.Id, "Test"}};

            var companies = await _companiesClient.GetPagedListAsync(account.Id, sortBy: "CreateDateTime",
                orderBy: "desc", allAttributes: false, attributes: filterAttributes).ConfigureAwait(false);

            var results = companies.Skip(1)
                .Zip(companies, (previous, current) => current.CreateDateTime >= previous.CreateDateTime);

            Assert.NotEmpty(companies);
            Assert.All(results, Assert.True);
        }

        [Fact]
        public async Task WhenCreate_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            var attribute = await _create.CompanyAttribute.WithAccountId(account.Id).BuildAsync().ConfigureAwait(false);

            var company = new Company
            {
                AccountId = account.Id,
                Type = CompanyType.None,
                IndustryType = CompanyIndustryType.None,
                LeadId = Guid.Empty,
                CreateUserId = Guid.Empty,
                ResponsibleUserId = Guid.Empty,
                FullName = "Test",
                ShortName = "Test",
                Phone = "9999999999",
                Email = "test@test",
                TaxNumber = "999999999999",
                RegistrationNumber = "999999999999999",
                RegistrationDate = new DateTime(2000, 1, 1),
                EmployeesCount = 1,
                YearlyTurnover = 1000000,
                JuridicalPostcode = "000000",
                JuridicalCountry = "Test",
                JuridicalRegion = "Test",
                JuridicalProvince = "Test",
                JuridicalCity = "Test",
                JuridicalStreet = "Test",
                JuridicalHouse = "1",
                JuridicalApartment = "1",
                LegalPostcode = "000000",
                LegalCountry = "Test",
                LegalRegion = "Test",
                LegalProvince = "Test",
                LegalCity = "Test",
                LegalStreet = "Test",
                LegalHouse = "1",
                LegalApartment = "1",
                IsDeleted = true,
                AttributeLinks = new List<CompanyAttributeLink>
                {
                    new CompanyAttributeLink
                    {
                        CompanyAttributeId = attribute.Id,
                        Value = "Test"
                    }
                },
                BankAccounts = new List<CompanyBankAccount>
                {
                    new CompanyBankAccount
                    {
                        Number = "9999999999999999999999999",
                        BankNumber = "9999999999",
                        BankCorrespondentNumber = "9999999999999999999999999",
                        BankName = "Test"
                    }
                }
            };

            var createdCompanyId = await _companiesClient.CreateAsync(company).ConfigureAwait(false);

            var createdCompany = await _companiesClient.GetAsync(createdCompanyId).ConfigureAwait(false);

            Assert.NotNull(createdCompany);
            Assert.Equal(createdCompanyId, createdCompany.Id);
            Assert.Equal(company.Type, createdCompany.Type);
            Assert.Equal(company.IndustryType, createdCompany.IndustryType);
            Assert.Equal(company.LeadId, createdCompany.LeadId);
            Assert.True(!createdCompany.CreateUserId.IsEmpty());
            Assert.Equal(company.ResponsibleUserId, createdCompany.ResponsibleUserId);
            Assert.Equal(company.FullName, createdCompany.FullName);
            Assert.Equal(company.ShortName, createdCompany.ShortName);
            Assert.Equal(company.AccountId, createdCompany.AccountId);
            Assert.Equal(company.AccountId, createdCompany.AccountId);
            Assert.Equal(company.AccountId, createdCompany.AccountId);
            Assert.Equal(company.Phone, createdCompany.Phone);
            Assert.Equal(company.Email, createdCompany.Email);
            Assert.Equal(company.TaxNumber, createdCompany.TaxNumber);
            Assert.Equal(company.RegistrationNumber, createdCompany.RegistrationNumber);
            Assert.Equal(company.RegistrationDate, createdCompany.RegistrationDate);
            Assert.Equal(company.EmployeesCount, createdCompany.EmployeesCount);
            Assert.Equal(company.YearlyTurnover, createdCompany.YearlyTurnover);
            Assert.Equal(company.JuridicalPostcode, createdCompany.JuridicalPostcode);
            Assert.Equal(company.JuridicalCountry, createdCompany.JuridicalCountry);
            Assert.Equal(company.JuridicalRegion, createdCompany.JuridicalRegion);
            Assert.Equal(company.JuridicalProvince, createdCompany.JuridicalProvince);
            Assert.Equal(company.JuridicalCity, createdCompany.JuridicalCity);
            Assert.Equal(company.JuridicalStreet, createdCompany.JuridicalStreet);
            Assert.Equal(company.JuridicalHouse, createdCompany.JuridicalHouse);
            Assert.Equal(company.JuridicalApartment, createdCompany.JuridicalApartment);
            Assert.Equal(company.LegalPostcode, createdCompany.LegalPostcode);
            Assert.Equal(company.LegalCountry, createdCompany.LegalCountry);
            Assert.Equal(company.LegalRegion, createdCompany.LegalRegion);
            Assert.Equal(company.LegalProvince, createdCompany.LegalProvince);
            Assert.Equal(company.LegalCity, createdCompany.LegalCity);
            Assert.Equal(company.LegalStreet, createdCompany.LegalStreet);
            Assert.Equal(company.LegalHouse, createdCompany.LegalHouse);
            Assert.Equal(company.LegalApartment, createdCompany.LegalApartment);
            Assert.Equal(company.IsDeleted, createdCompany.IsDeleted);
            Assert.True(createdCompany.CreateDateTime.IsMoreThanMinValue());
            Assert.NotEmpty(createdCompany.AttributeLinks);
            Assert.NotEmpty(createdCompany.BankAccounts);
        }

        [Fact]
        public async Task WhenUpdate_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            var attribute = await _create.CompanyAttribute.WithAccountId(account.Id).BuildAsync().ConfigureAwait(false);
            var company = await _create.Company.WithAccountId(account.Id).BuildAsync().ConfigureAwait(false);

            company.Type = CompanyType.None;
            company.IndustryType = CompanyIndustryType.None;
            company.LeadId = Guid.Empty;
            company.CreateUserId = Guid.Empty;
            company.ResponsibleUserId = Guid.Empty;
            company.FullName = "Test";
            company.ShortName = "Test";
            company.Phone = "9999999999";
            company.Email = "test@test";
            company.TaxNumber = "999999999999";
            company.RegistrationNumber = "999999999999999";
            company.RegistrationDate = new DateTime(2000, 1, 1);
            company.EmployeesCount = 1;
            company.YearlyTurnover = 1000000;
            company.JuridicalPostcode = "000000";
            company.JuridicalCountry = "Test";
            company.JuridicalRegion = "Test";
            company.JuridicalProvince = "Test";
            company.JuridicalCity = "Test";
            company.JuridicalStreet = "Test";
            company.JuridicalHouse = "1";
            company.JuridicalApartment = "1";
            company.LegalPostcode = "000000";
            company.LegalCountry = "Test";
            company.LegalRegion = "Test";
            company.LegalProvince = "Test";
            company.LegalCity = "Test";
            company.LegalStreet = "Test";
            company.LegalHouse = "1";
            company.LegalApartment = "1";
            company.IsDeleted = true;
            company.AttributeLinks.Add(new CompanyAttributeLink {CompanyAttributeId = attribute.Id, Value = "Test"});
            company.BankAccounts.Add(new CompanyBankAccount {Number = "9999999999999999999999999"});
            await _companiesClient.UpdateAsync(company).ConfigureAwait(false);

            var updatedCompany = await _companiesClient.GetAsync(company.Id).ConfigureAwait(false);

            Assert.Equal(company.AccountId, updatedCompany.AccountId);
            Assert.Equal(company.Type, updatedCompany.Type);
            Assert.Equal(company.IndustryType, updatedCompany.IndustryType);
            Assert.Equal(company.LeadId, updatedCompany.LeadId);
            Assert.Equal(company.CreateUserId, updatedCompany.CreateUserId);
            Assert.Equal(company.ResponsibleUserId, updatedCompany.ResponsibleUserId);
            Assert.Equal(company.FullName, updatedCompany.FullName);
            Assert.Equal(company.ShortName, updatedCompany.ShortName);
            Assert.Equal(company.AccountId, updatedCompany.AccountId);
            Assert.Equal(company.AccountId, updatedCompany.AccountId);
            Assert.Equal(company.AccountId, updatedCompany.AccountId);
            Assert.Equal(company.Phone, updatedCompany.Phone);
            Assert.Equal(company.Email, updatedCompany.Email);
            Assert.Equal(company.TaxNumber, updatedCompany.TaxNumber);
            Assert.Equal(company.RegistrationNumber, updatedCompany.RegistrationNumber);
            Assert.Equal(company.RegistrationDate, updatedCompany.RegistrationDate);
            Assert.Equal(company.EmployeesCount, updatedCompany.EmployeesCount);
            Assert.Equal(company.YearlyTurnover, updatedCompany.YearlyTurnover);
            Assert.Equal(company.JuridicalPostcode, updatedCompany.JuridicalPostcode);
            Assert.Equal(company.JuridicalCountry, updatedCompany.JuridicalCountry);
            Assert.Equal(company.JuridicalRegion, updatedCompany.JuridicalRegion);
            Assert.Equal(company.JuridicalProvince, updatedCompany.JuridicalProvince);
            Assert.Equal(company.JuridicalCity, updatedCompany.JuridicalCity);
            Assert.Equal(company.JuridicalStreet, updatedCompany.JuridicalStreet);
            Assert.Equal(company.JuridicalHouse, updatedCompany.JuridicalHouse);
            Assert.Equal(company.JuridicalApartment, updatedCompany.JuridicalApartment);
            Assert.Equal(company.LegalPostcode, updatedCompany.LegalPostcode);
            Assert.Equal(company.LegalCountry, updatedCompany.LegalCountry);
            Assert.Equal(company.LegalRegion, updatedCompany.LegalRegion);
            Assert.Equal(company.LegalProvince, updatedCompany.LegalProvince);
            Assert.Equal(company.LegalCity, updatedCompany.LegalCity);
            Assert.Equal(company.LegalStreet, updatedCompany.LegalStreet);
            Assert.Equal(company.LegalHouse, updatedCompany.LegalHouse);
            Assert.Equal(company.LegalApartment, updatedCompany.LegalApartment);
            Assert.Equal(company.IsDeleted, updatedCompany.IsDeleted);
            Assert.Equal(company.AttributeLinks.Single().CompanyAttributeId,
                updatedCompany.AttributeLinks.Single().CompanyAttributeId);
            Assert.Equal(company.AttributeLinks.Single().Value, updatedCompany.AttributeLinks.Single().Value);
            Assert.Equal(company.BankAccounts.Single().Number, updatedCompany.BankAccounts.Single().Number);
        }

        [Fact]
        public async Task WhenDelete_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            var companyIds = (await Task.WhenAll(
                    _create.Company.WithAccountId(account.Id).BuildAsync(),
                    _create.Company.WithAccountId(account.Id).BuildAsync())
                .ConfigureAwait(false)).Select(x => x.Id).ToList();

            await _companiesClient.DeleteAsync(companyIds).ConfigureAwait(false);

            var companies = await _companiesClient.GetListAsync(companyIds).ConfigureAwait(false);

            Assert.All(companies, x => Assert.True(x.IsDeleted));
        }

        [Fact]
        public async Task WhenRestore_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            var companyIds = (await Task.WhenAll(
                    _create.Company.WithAccountId(account.Id).BuildAsync(),
                    _create.Company.WithAccountId(account.Id).BuildAsync())
                .ConfigureAwait(false)).Select(x => x.Id).ToList();

            await _companiesClient.RestoreAsync(companyIds).ConfigureAwait(false);

            var companies = await _companiesClient.GetListAsync(companyIds).ConfigureAwait(false);

            Assert.All(companies, x => Assert.False(x.IsDeleted));
        }
    }
}