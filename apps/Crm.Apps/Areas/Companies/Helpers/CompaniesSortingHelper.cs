using System.Linq;
using Crm.Apps.Areas.Companies.Models;

namespace Crm.Apps.Areas.Companies.Helpers
{
    public static class CompaniesSortingHelper
    {
        public static IOrderedQueryable<Company> Sort(this IQueryable<Company> queryable, string sortBy, string orderBy)
        {
            var isDesc = orderBy == "desc";

            switch (sortBy)
            {
                case nameof(Company.Id):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.Id)
                        : queryable.OrderBy(x => x.Id);
                case nameof(Company.Type):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.Type)
                        : queryable.OrderBy(x => x.Type);
                case nameof(Company.IndustryType):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.IndustryType)
                        : queryable.OrderBy(x => x.IndustryType);
                case nameof(Company.FullName):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.FullName)
                        : queryable.OrderBy(x => x.FullName);
                case nameof(Company.ShortName):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.ShortName)
                        : queryable.OrderBy(x => x.ShortName);
                case nameof(Company.Phone):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.Phone)
                        : queryable.OrderBy(x => x.Phone);
                case nameof(Company.Email):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.Email)
                        : queryable.OrderBy(x => x.Email);
                case nameof(Company.TaxNumber):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.TaxNumber)
                        : queryable.OrderBy(x => x.TaxNumber);
                case nameof(Company.RegistrationNumber):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.RegistrationNumber)
                        : queryable.OrderBy(x => x.RegistrationNumber);
                case nameof(Company.RegistrationDate):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.RegistrationDate)
                        : queryable.OrderBy(x => x.RegistrationDate);
                case nameof(Company.EmployeesCount):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.EmployeesCount)
                        : queryable.OrderBy(x => x.EmployeesCount);
                case nameof(Company.YearlyTurnover):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.YearlyTurnover)
                        : queryable.OrderBy(x => x.YearlyTurnover);
                case nameof(Company.JuridicalPostcode):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.JuridicalPostcode)
                        : queryable.OrderBy(x => x.JuridicalPostcode);
                case nameof(Company.JuridicalCountry):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.JuridicalCountry)
                        : queryable.OrderBy(x => x.JuridicalCountry);
                case nameof(Company.JuridicalRegion):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.JuridicalRegion)
                        : queryable.OrderBy(x => x.JuridicalRegion);
                case nameof(Company.JuridicalProvince):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.JuridicalProvince)
                        : queryable.OrderBy(x => x.JuridicalProvince);
                case nameof(Company.JuridicalCity):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.JuridicalCity)
                        : queryable.OrderBy(x => x.JuridicalCity);
                case nameof(Company.JuridicalStreet):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.JuridicalStreet)
                        : queryable.OrderBy(x => x.JuridicalStreet);
                case nameof(Company.JuridicalHouse):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.JuridicalHouse)
                        : queryable.OrderBy(x => x.JuridicalHouse);
                case nameof(Company.JuridicalApartment):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.JuridicalApartment)
                        : queryable.OrderBy(x => x.JuridicalApartment);
                case nameof(Company.LegalPostcode):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.LegalPostcode)
                        : queryable.OrderBy(x => x.LegalPostcode);
                case nameof(Company.LegalCountry):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.LegalCountry)
                        : queryable.OrderBy(x => x.LegalCountry);
                case nameof(Company.LegalRegion):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.LegalRegion)
                        : queryable.OrderBy(x => x.LegalRegion);
                case nameof(Company.LegalProvince):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.LegalProvince)
                        : queryable.OrderBy(x => x.LegalProvince);
                case nameof(Company.LegalCity):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.LegalCity)
                        : queryable.OrderBy(x => x.LegalCity);
                case nameof(Company.LegalStreet):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.LegalStreet)
                        : queryable.OrderBy(x => x.LegalStreet);
                case nameof(Company.LegalHouse):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.LegalHouse)
                        : queryable.OrderBy(x => x.LegalHouse);
                case nameof(Company.LegalApartment):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.LegalApartment)
                        : queryable.OrderBy(x => x.LegalApartment);
                case nameof(Company.CreateDateTime):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.CreateDateTime)
                        : queryable.OrderBy(x => x.CreateDateTime);
                default:
                    return queryable.OrderByDescending(x => x.CreateDateTime);
            }
        }
    }
}