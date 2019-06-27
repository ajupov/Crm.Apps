using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Companies.Helpers;
using Crm.Apps.Companies.Models;
using Crm.Apps.Companies.Parameters;
using Crm.Apps.Companies.Storages;
using Crm.Utils.Guid;
using Crm.Utils.String;
using Microsoft.EntityFrameworkCore;

namespace Crm.Apps.Companies.Services
{
    public class CompaniesService : ICompaniesService
    {
        private readonly CompaniesStorage _storage;

        public CompaniesService(CompaniesStorage storage)
        {
            _storage = storage;
        }

        public Task<Company> GetAsync(Guid id, CancellationToken ct)
        {
            return _storage.Companies.Include(x => x.BankAccounts).Include(x => x.AttributeLinks)
                .FirstOrDefaultAsync(x => x.Id == id, ct);
        }

        public Task<List<Company>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct)
        {
            return _storage.Companies.Where(x => ids.Contains(x.Id)).ToListAsync(ct);
        }

        public async Task<List<Company>> GetPagedListAsync(CompanyGetPagedListParameter parameter, CancellationToken ct)
        {
            var temp = await _storage.Companies.Include(x => x.BankAccounts).Include(x => x.AttributeLinks)
                .Where(x =>
                    (parameter.AccountId.IsEmpty() || x.AccountId == parameter.AccountId) &&
                    (parameter.LeadId.IsEmpty() || x.LeadId == parameter.LeadId) &&
                    (parameter.FullName.IsEmpty() || EF.Functions.Like(x.FullName, $"{parameter.FullName}%")) &&
                    (parameter.ShortName.IsEmpty() || EF.Functions.Like(x.ShortName, $"{parameter.ShortName}%")) &&
                    (parameter.Phone.IsEmpty() || x.Phone == parameter.Phone) &&
                    (parameter.Email.IsEmpty() || x.Email == parameter.Email) &&
                    (parameter.TaxNumber.IsEmpty() || x.TaxNumber == parameter.TaxNumber) &&
                    (parameter.RegistrationNumber.IsEmpty() || x.RegistrationNumber == parameter.RegistrationNumber) &&
                    (!parameter.MinRegistrationDate.HasValue || x.RegistrationDate >= parameter.MinRegistrationDate) &&
                    (!parameter.MaxRegistrationDate.HasValue || x.RegistrationDate <= parameter.MaxRegistrationDate) &&
                    (!parameter.MinEmployeesCount.HasValue || x.EmployeesCount >= parameter.MinEmployeesCount) &&
                    (!parameter.MaxEmployeesCount.HasValue || x.EmployeesCount <= parameter.MaxEmployeesCount) &&
                    (!parameter.MinYearlyTurnover.HasValue || x.YearlyTurnover >= parameter.MinYearlyTurnover) &&
                    (!parameter.MaxYearlyTurnover.HasValue || x.YearlyTurnover <= parameter.MaxYearlyTurnover) &&
                    (parameter.JuridicalPostcode.IsEmpty() || x.JuridicalPostcode == parameter.JuridicalPostcode) &&
                    (parameter.JuridicalCountry.IsEmpty() ||
                     EF.Functions.Like(x.JuridicalCountry, $"{parameter.JuridicalCountry}%")) &&
                    (parameter.JuridicalRegion.IsEmpty() ||
                     EF.Functions.Like(x.JuridicalRegion, $"{parameter.JuridicalRegion}%")) &&
                    (parameter.JuridicalProvince.IsEmpty() ||
                     EF.Functions.Like(x.JuridicalProvince, $"{parameter.JuridicalProvince}%")) &&
                    (parameter.JuridicalCity.IsEmpty() ||
                     EF.Functions.Like(x.JuridicalCity, $"{parameter.JuridicalCity}%")) &&
                    (parameter.JuridicalStreet.IsEmpty() ||
                     EF.Functions.Like(x.JuridicalStreet, $"{parameter.JuridicalStreet}%")) &&
                    (parameter.JuridicalHouse.IsEmpty() ||
                     EF.Functions.Like(x.JuridicalHouse, $"{parameter.JuridicalHouse}%")) &&
                    (parameter.JuridicalApartment.IsEmpty() || x.JuridicalApartment == parameter.JuridicalApartment) &&
                    (parameter.LegalPostcode.IsEmpty() || x.LegalPostcode == parameter.LegalPostcode) &&
                    (parameter.LegalCountry.IsEmpty() ||
                     EF.Functions.Like(x.LegalCountry, $"{parameter.LegalCountry}%")) &&
                    (parameter.LegalRegion.IsEmpty() ||
                     EF.Functions.Like(x.LegalRegion, $"{parameter.LegalRegion}%")) &&
                    (parameter.LegalProvince.IsEmpty() ||
                     EF.Functions.Like(x.LegalProvince, $"{parameter.LegalProvince}%")) &&
                    (parameter.LegalCity.IsEmpty() || EF.Functions.Like(x.LegalCity, $"{parameter.LegalCity}%")) &&
                    (parameter.LegalStreet.IsEmpty() ||
                     EF.Functions.Like(x.LegalStreet, $"{parameter.LegalStreet}%")) &&
                    (parameter.LegalHouse.IsEmpty() || EF.Functions.Like(x.LegalHouse, $"{parameter.LegalHouse}%")) &&
                    (parameter.LegalApartment.IsEmpty() || x.LegalApartment == parameter.LegalApartment) &&
                    (!parameter.IsDeleted.HasValue || x.IsDeleted == parameter.IsDeleted) &&
                    (!parameter.MinCreateDate.HasValue || x.CreateDateTime >= parameter.MinCreateDate) &&
                    (!parameter.MaxCreateDate.HasValue || x.CreateDateTime <= parameter.MaxCreateDate))
                .Sort(parameter.SortBy, parameter.OrderBy)
                .ToListAsync(ct).ConfigureAwait(false);

            return temp.Where(x => x.FilterByAdditional(parameter)).Skip(parameter.Offset).Take(parameter.Limit)
                .ToList();
        }

        public async Task<Guid> CreateAsync(Guid userId, Company company, CancellationToken ct)
        {
            var newCompany = new Company();
            var change = newCompany.CreateWithLog(userId, x =>
            {
                x.Id = Guid.NewGuid();
                x.AccountId = company.AccountId;
                x.Type = company.Type;
                x.IndustryType = company.IndustryType;
                x.LeadId = company.LeadId;
                x.CreateUserId = userId;
                x.ResponsibleUserId = company.ResponsibleUserId;
                x.FullName = company.FullName;
                x.ShortName = company.ShortName;
                x.Phone = company.Phone;
                x.Email = company.Email;
                x.TaxNumber = company.TaxNumber;
                x.RegistrationNumber = company.RegistrationNumber;
                x.RegistrationDate = company.RegistrationDate;
                x.EmployeesCount = company.EmployeesCount;
                x.YearlyTurnover = company.YearlyTurnover;
                x.JuridicalPostcode = company.JuridicalPostcode;
                x.JuridicalCountry = company.JuridicalCountry;
                x.JuridicalRegion = company.JuridicalRegion;
                x.JuridicalProvince = company.JuridicalProvince;
                x.JuridicalCity = company.JuridicalCity;
                x.JuridicalStreet = company.JuridicalStreet;
                x.JuridicalHouse = company.JuridicalHouse;
                x.JuridicalApartment = company.JuridicalApartment;
                x.LegalPostcode = company.LegalPostcode;
                x.LegalCountry = company.LegalCountry;
                x.LegalRegion = company.LegalRegion;
                x.LegalProvince = company.LegalProvince;
                x.LegalCity = company.LegalCity;
                x.LegalStreet = company.LegalStreet;
                x.LegalHouse = company.LegalHouse;
                x.LegalApartment = company.LegalApartment;
                x.IsDeleted = company.IsDeleted;
                x.CreateDateTime = DateTime.UtcNow;
                x.BankAccounts = company.BankAccounts;
                x.AttributeLinks = company.AttributeLinks;
            });

            var entry = await _storage.AddAsync(newCompany, ct).ConfigureAwait(false);
            await _storage.AddAsync(change, ct).ConfigureAwait(false);
            await _storage.SaveChangesAsync(ct).ConfigureAwait(false);

            return entry.Entity.Id;
        }

        public async Task UpdateAsync(Guid companyId, Company oldCompany, Company newCompany, CancellationToken ct)
        {
            var change = oldCompany.UpdateWithLog(companyId, x =>
            {
                x.Type = newCompany.Type;
                x.IndustryType = newCompany.IndustryType;
                x.LeadId = newCompany.LeadId;
                x.ResponsibleUserId = newCompany.ResponsibleUserId;
                x.FullName = newCompany.FullName;
                x.ShortName = newCompany.ShortName;
                x.Phone = newCompany.Phone;
                x.Email = newCompany.Email;
                x.TaxNumber = newCompany.TaxNumber;
                x.RegistrationNumber = newCompany.RegistrationNumber;
                x.RegistrationDate = newCompany.RegistrationDate;
                x.EmployeesCount = newCompany.EmployeesCount;
                x.YearlyTurnover = newCompany.YearlyTurnover;
                x.JuridicalPostcode = newCompany.JuridicalPostcode;
                x.JuridicalCountry = newCompany.JuridicalCountry;
                x.JuridicalRegion = newCompany.JuridicalRegion;
                x.JuridicalProvince = newCompany.JuridicalProvince;
                x.JuridicalCity = newCompany.JuridicalCity;
                x.JuridicalStreet = newCompany.JuridicalStreet;
                x.JuridicalHouse = newCompany.JuridicalHouse;
                x.JuridicalApartment = newCompany.JuridicalApartment;
                x.LegalPostcode = newCompany.LegalPostcode;
                x.LegalCountry = newCompany.LegalCountry;
                x.LegalRegion = newCompany.LegalRegion;
                x.LegalProvince = newCompany.LegalProvince;
                x.LegalCity = newCompany.LegalCity;
                x.LegalStreet = newCompany.LegalStreet;
                x.LegalHouse = newCompany.LegalHouse;
                x.LegalApartment = newCompany.LegalApartment;
                x.IsDeleted = newCompany.IsDeleted;
                x.BankAccounts = newCompany.BankAccounts;
                x.AttributeLinks = newCompany.AttributeLinks;
            });

            _storage.Update(oldCompany);
            await _storage.AddAsync(change, ct).ConfigureAwait(false);
            await _storage.SaveChangesAsync(ct).ConfigureAwait(false);
        }

        public async Task DeleteAsync(Guid companyId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<CompanyChange>();

            await _storage.Companies.Where(x => ids.Contains(x.Id))
                .ForEachAsync(u => changes.Add(u.UpdateWithLog(companyId, x => x.IsDeleted = true)), ct)
                .ConfigureAwait(false);

            await _storage.AddRangeAsync(changes, ct).ConfigureAwait(false);
            await _storage.SaveChangesAsync(ct).ConfigureAwait(false);
        }

        public async Task RestoreAsync(Guid companyId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<CompanyChange>();

            await _storage.Companies.Where(x => ids.Contains(x.Id))
                .ForEachAsync(u => changes.Add(u.UpdateWithLog(companyId, x => x.IsDeleted = false)), ct)
                .ConfigureAwait(false);

            await _storage.AddRangeAsync(changes, ct).ConfigureAwait(false);
            await _storage.SaveChangesAsync(ct).ConfigureAwait(false);
        }
    }
}