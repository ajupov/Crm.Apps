using System;
using System.Threading.Tasks;
using Crm.Clients.Companies.Models;

namespace Crm.Apps.Tests.Dsl.Builders.Company
{
    public interface ICompanyBuilder
    {
        CompanyBuilder WithAccountId(Guid accountId);

        CompanyBuilder WithType(CompanyType type);

        CompanyBuilder WithIndustryType(CompanyIndustryType type);

        CompanyBuilder WithLeadId(Guid leadId);

        CompanyBuilder WithCreateUserId(Guid createUserId);

        CompanyBuilder WithResponsibleUserId(Guid responsibleUserId);

        CompanyBuilder WithFullName(string fullName);

        CompanyBuilder WithShortName(string shortName);

        CompanyBuilder WithPhone(string phone);

        CompanyBuilder WithEmail(string email);

        CompanyBuilder WithTaxNumber(string taxNumber);

        CompanyBuilder WithRegistrationNumber(string registrationNumber);

        CompanyBuilder WithRegistrationDate(DateTime registrationDate);

        CompanyBuilder WithEmployeesCount(int employeesCount);

        CompanyBuilder WithYearlyTurnover(int yearlyTurnover);

        CompanyBuilder WithPostcode(string postcode);

        CompanyBuilder WithJuridicalCountry(string juridicalCountry);

        CompanyBuilder WithJuridicalRegion(string juridicalRegion);

        CompanyBuilder WithJuridicalProvince(string juridicalProvince);

        CompanyBuilder WithJuridicalCity(string juridicalCity);

        CompanyBuilder WithJuridicalStreet(string juridicalStreet);

        CompanyBuilder WithJuridicalHouse(string juridicalHouse);

        CompanyBuilder WithJuridicalApartment(string juridicalApartment);

        CompanyBuilder WithLegalCountry(string legalCountry);

        CompanyBuilder WithLegalRegion(string legalRegion);

        CompanyBuilder WithLegalProvince(string legalProvince);

        CompanyBuilder WithLegalCity(string legalCity);

        CompanyBuilder WithLegalStreet(string legalStreet);

        CompanyBuilder WithLegalHouse(string legalHouse);

        CompanyBuilder WithLegalApartment(string legalApartment);

        CompanyBuilder AsDeleted();

        CompanyBuilder WithBankAccount(string number, string bankNumber = default, string bankName = default,
            string bankCorrespondentNumber = default);

        CompanyBuilder WithAttributeLink(Guid attributeId, string value);

        Task<Clients.Companies.Models.Company> BuildAsync();
    }
}