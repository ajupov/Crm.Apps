using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Companies.Helpers;
using Crm.Apps.Companies.Models;
using Crm.Apps.Companies.Parameters;
using Crm.Apps.Companies.Storages;
using Crm.Utils.Decimal;
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
                    
                    
                    
                    (parameter.Surname.IsEmpty() || EF.Functions.Like(x.Surname, $"{parameter.Surname}%")) &&
                    (parameter.Name.IsEmpty() || EF.Functions.Like(x.Name, $"{parameter.Name}%")) &&
                    (parameter.Patronymic.IsEmpty() || EF.Functions.Like(x.Patronymic, $"{parameter.Patronymic}%")) &&
                    (parameter.Phone.IsEmpty() || x.Phone == parameter.Phone) &&
                    (parameter.Email.IsEmpty() || x.Phone == parameter.Email) &&
                    (parameter.CompanyName.IsEmpty() ||
                     EF.Functions.Like(x.CompanyName, $"{parameter.CompanyName}%")) &&
                    (parameter.Post.IsEmpty() || EF.Functions.Like(x.Post, $"{parameter.Post}%")) &&
                    (parameter.Postcode.IsEmpty() || x.Postcode == parameter.Postcode) &&
                    (parameter.Country.IsEmpty() || EF.Functions.Like(x.Country, $"{parameter.Country}%")) &&
                    (parameter.Region.IsEmpty() || EF.Functions.Like(x.Region, $"{parameter.Region}%")) &&
                    (parameter.Province.IsEmpty() || EF.Functions.Like(x.Province, $"{parameter.Province}%")) &&
                    (parameter.City.IsEmpty() || EF.Functions.Like(x.City, $"{parameter.City}%")) &&
                    (parameter.Street.IsEmpty() || EF.Functions.Like(x.Street, $"{parameter.Street}%")) &&
                    (parameter.House.IsEmpty() || EF.Functions.Like(x.House, $"{parameter.House}%")) &&
                    (parameter.Apartment.IsEmpty() || x.Apartment == parameter.Apartment) &&
                    (parameter.MinOpportunitySum.IsEmpty() || x.OpportunitySum >= parameter.MinOpportunitySum.Value) &&
                    (parameter.MaxOpportunitySum.IsEmpty() || x.OpportunitySum <= parameter.MaxOpportunitySum) &&
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
                x.SourceId = company.SourceId;
                x.CreateUserId = userId;
                x.ResponsibleUserId = company.ResponsibleUserId;
                x.Surname = company.Surname;
                x.Name = company.Name;
                x.Patronymic = company.Patronymic;
                x.Phone = company.Phone;
                x.Email = company.Email;
                x.CompanyName = company.CompanyName;
                x.Post = company.Post;
                x.Postcode = company.Postcode;
                x.Country = company.Country;
                x.Region = company.Region;
                x.Province = company.Province;
                x.City = company.City;
                x.Street = company.Street;
                x.House = company.House;
                x.Apartment = company.Apartment;
                x.OpportunitySum = company.OpportunitySum;
                x.IsDeleted = company.IsDeleted;
                x.CreateDateTime = DateTime.UtcNow;
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
                x.AccountId = newCompany.AccountId;
                x.SourceId = newCompany.SourceId;
                x.ResponsibleUserId = newCompany.ResponsibleUserId;
                x.Surname = newCompany.Surname;
                x.Name = newCompany.Name;
                x.Patronymic = newCompany.Patronymic;
                x.Phone = newCompany.Phone;
                x.Email = newCompany.Email;
                x.CompanyName = newCompany.CompanyName;
                x.Post = newCompany.Post;
                x.Postcode = newCompany.Postcode;
                x.Country = newCompany.Country;
                x.Region = newCompany.Region;
                x.Province = newCompany.Province;
                x.City = newCompany.City;
                x.Street = newCompany.Street;
                x.House = newCompany.House;
                x.Apartment = newCompany.Apartment;
                x.OpportunitySum = newCompany.OpportunitySum;
                x.IsDeleted = newCompany.IsDeleted;
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