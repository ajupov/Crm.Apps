using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Utils.All.Guid;
using Ajupov.Utils.All.Sorting;
using Ajupov.Utils.All.String;
using Crm.Apps.Companies.Helpers;
using Crm.Apps.Companies.Mappers;
using Crm.Apps.Companies.Storages;
using Crm.Apps.Companies.v1.Models;
using Crm.Apps.Companies.v1.RequestParameters;
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
            return _storage.Companies
                .Include(x => x.BankAccounts)
                .Include(x => x.AttributeLinks)
                .FirstOrDefaultAsync(x => x.Id == id, ct);
        }

        public Task<List<Company>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct)
        {
            return _storage.Companies
                .Where(x => ids.Contains(x.Id))
                .ToListAsync(ct);
        }

        public async Task<List<Company>> GetPagedListAsync(
            CompanyGetPagedListRequestParameter request,
            CancellationToken ct)
        {
            var temp = await _storage.Companies
                .Include(x => x.BankAccounts)
                .Include(x => x.AttributeLinks)
                .Where(x =>
                    (request.AccountId.IsEmpty() || x.AccountId == request.AccountId) &&
                    (request.LeadId.IsEmpty() || x.LeadId == request.LeadId) &&
                    (request.FullName.IsEmpty() || EF.Functions.Like(x.FullName, $"{request.FullName}%")) &&
                    (request.ShortName.IsEmpty() || EF.Functions.Like(x.ShortName, $"{request.ShortName}%")) &&
                    (request.Phone.IsEmpty() || x.Phone == request.Phone) &&
                    (request.Email.IsEmpty() || x.Email == request.Email) &&
                    (request.TaxNumber.IsEmpty() || x.TaxNumber == request.TaxNumber) &&
                    (request.RegistrationNumber.IsEmpty() || x.RegistrationNumber == request.RegistrationNumber) &&
                    (!request.MinRegistrationDate.HasValue || x.RegistrationDate >= request.MinRegistrationDate) &&
                    (!request.MaxRegistrationDate.HasValue || x.RegistrationDate <= request.MaxRegistrationDate) &&
                    (!request.MinEmployeesCount.HasValue || x.EmployeesCount >= request.MinEmployeesCount) &&
                    (!request.MaxEmployeesCount.HasValue || x.EmployeesCount <= request.MaxEmployeesCount) &&
                    (!request.MinYearlyTurnover.HasValue || x.YearlyTurnover >= request.MinYearlyTurnover) &&
                    (!request.MaxYearlyTurnover.HasValue || x.YearlyTurnover <= request.MaxYearlyTurnover) &&
                    (request.JuridicalPostcode.IsEmpty() || x.JuridicalPostcode == request.JuridicalPostcode) &&
                    (request.JuridicalCountry.IsEmpty() ||
                     EF.Functions.Like(x.JuridicalCountry, $"{request.JuridicalCountry}%")) &&
                    (request.JuridicalRegion.IsEmpty() ||
                     EF.Functions.Like(x.JuridicalRegion, $"{request.JuridicalRegion}%")) &&
                    (request.JuridicalProvince.IsEmpty() ||
                     EF.Functions.Like(x.JuridicalProvince, $"{request.JuridicalProvince}%")) &&
                    (request.JuridicalCity.IsEmpty() ||
                     EF.Functions.Like(x.JuridicalCity, $"{request.JuridicalCity}%")) &&
                    (request.JuridicalStreet.IsEmpty() ||
                     EF.Functions.Like(x.JuridicalStreet, $"{request.JuridicalStreet}%")) &&
                    (request.JuridicalHouse.IsEmpty() ||
                     EF.Functions.Like(x.JuridicalHouse, $"{request.JuridicalHouse}%")) &&
                    (request.JuridicalApartment.IsEmpty() || x.JuridicalApartment == request.JuridicalApartment) &&
                    (request.LegalPostcode.IsEmpty() || x.LegalPostcode == request.LegalPostcode) &&
                    (request.LegalCountry.IsEmpty() ||
                     EF.Functions.Like(x.LegalCountry, $"{request.LegalCountry}%")) &&
                    (request.LegalRegion.IsEmpty() ||
                     EF.Functions.Like(x.LegalRegion, $"{request.LegalRegion}%")) &&
                    (request.LegalProvince.IsEmpty() ||
                     EF.Functions.Like(x.LegalProvince, $"{request.LegalProvince}%")) &&
                    (request.LegalCity.IsEmpty() || EF.Functions.Like(x.LegalCity, $"{request.LegalCity}%")) &&
                    (request.LegalStreet.IsEmpty() ||
                     EF.Functions.Like(x.LegalStreet, $"{request.LegalStreet}%")) &&
                    (request.LegalHouse.IsEmpty() || EF.Functions.Like(x.LegalHouse, $"{request.LegalHouse}%")) &&
                    (request.LegalApartment.IsEmpty() || x.LegalApartment == request.LegalApartment) &&
                    (!request.IsDeleted.HasValue || x.IsDeleted == request.IsDeleted) &&
                    (!request.MinCreateDate.HasValue || x.CreateDateTime >= request.MinCreateDate) &&
                    (!request.MaxCreateDate.HasValue || x.CreateDateTime <= request.MaxCreateDate) &&
                    (!request.MinModifyDate.HasValue || x.ModifyDateTime >= request.MinModifyDate) &&
                    (!request.MaxModifyDate.HasValue || x.ModifyDateTime <= request.MaxModifyDate))
                .SortBy(request.SortBy, request.OrderBy)
                .ToListAsync(ct);

            return temp
                .Where(x => x.FilterByAdditional(request))
                .Skip(request.Offset)
                .Take(request.Limit)
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
                x.BankAccounts = company.BankAccounts.Map(x.Id);
                x.AttributeLinks = company.AttributeLinks.Map(x.Id);
            });

            var entry = await _storage.AddAsync(newCompany, ct);
            await _storage.AddAsync(change, ct);
            await _storage.SaveChangesAsync(ct);

            return entry.Entity.Id;
        }

        public async Task UpdateAsync(Guid userId, Company oldCompany, Company newCompany, CancellationToken ct)
        {
            var change = oldCompany.UpdateWithLog(userId, x =>
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
                x.ModifyDateTime = DateTime.UtcNow;
                x.BankAccounts = newCompany.BankAccounts.Map(x.Id);
                x.AttributeLinks = newCompany.AttributeLinks.Map(x.Id);
            });

            _storage.Update(oldCompany);
            await _storage.AddAsync(change, ct);
            await _storage.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<CompanyChange>();

            await _storage.Companies
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(x => changes.Add(x.UpdateWithLog(userId, c =>
                {
                    c.IsDeleted = true;
                    c.ModifyDateTime = DateTime.UtcNow;
                })), ct);

            await _storage.AddRangeAsync(changes, ct);
            await _storage.SaveChangesAsync(ct);
        }

        public async Task RestoreAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<CompanyChange>();

            await _storage.Companies
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(x => changes.Add(x.UpdateWithLog(userId, c =>
                {
                    c.IsDeleted = false;
                    c.ModifyDateTime = DateTime.UtcNow;
                })), ct);

            await _storage.AddRangeAsync(changes, ct);
            await _storage.SaveChangesAsync(ct);
        }
    }
}