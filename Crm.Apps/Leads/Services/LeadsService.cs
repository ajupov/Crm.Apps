using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Utils.All.Decimal;
using Ajupov.Utils.All.Guid;
using Ajupov.Utils.All.Sorting;
using Ajupov.Utils.All.String;
using Crm.Apps.Leads.Helpers;
using Crm.Apps.Leads.Storages;
using Crm.Apps.Leads.v1.Models;
using Crm.Apps.Leads.v1.RequestParameters;
using Microsoft.EntityFrameworkCore;

namespace Crm.Apps.Leads.Services
{
    public class LeadsService : ILeadsService
    {
        private readonly LeadsStorage _storage;

        public LeadsService(LeadsStorage storage)
        {
            _storage = storage;
        }

        public Task<Lead> GetAsync(Guid id, CancellationToken ct)
        {
            return _storage.Leads
                .AsNoTracking()
                .Include(x => x.Source)
                .Include(x => x.AttributeLinks)
                .FirstOrDefaultAsync(x => x.Id == id, ct);
        }

        public Task<List<Lead>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct)
        {
            return _storage.Leads
                .AsNoTracking()
                .Where(x => ids.Contains(x.Id))
                .ToListAsync(ct);
        }

        public async Task<List<Lead>> GetPagedListAsync(LeadGetPagedListRequestParameter request, CancellationToken ct)
        {
            var temp = await _storage.Leads
                .AsNoTracking()
                .Include(x => x.Source)
                .Include(x => x.AttributeLinks)
                .Where(x =>
                    (request.AccountId.IsEmpty() || x.AccountId == request.AccountId) &&
                    (request.Surname.IsEmpty() || EF.Functions.Like(x.Surname, $"{request.Surname}%")) &&
                    (request.Name.IsEmpty() || EF.Functions.Like(x.Name, $"{request.Name}%")) &&
                    (request.Patronymic.IsEmpty() || EF.Functions.Like(x.Patronymic, $"{request.Patronymic}%")) &&
                    (request.Phone.IsEmpty() || x.Phone == request.Phone) &&
                    (request.Email.IsEmpty() || x.Email == request.Email) &&
                    (request.CompanyName.IsEmpty() ||
                     EF.Functions.Like(x.CompanyName, $"{request.CompanyName}%")) &&
                    (request.Post.IsEmpty() || EF.Functions.Like(x.Post, $"{request.Post}%")) &&
                    (request.Postcode.IsEmpty() || x.Postcode == request.Postcode) &&
                    (request.Country.IsEmpty() || EF.Functions.Like(x.Country, $"{request.Country}%")) &&
                    (request.Region.IsEmpty() || EF.Functions.Like(x.Region, $"{request.Region}%")) &&
                    (request.Province.IsEmpty() || EF.Functions.Like(x.Province, $"{request.Province}%")) &&
                    (request.City.IsEmpty() || EF.Functions.Like(x.City, $"{request.City}%")) &&
                    (request.Street.IsEmpty() || EF.Functions.Like(x.Street, $"{request.Street}%")) &&
                    (request.House.IsEmpty() || EF.Functions.Like(x.House, $"{request.House}%")) &&
                    (request.Apartment.IsEmpty() || x.Apartment == request.Apartment) &&
                    (request.MinOpportunitySum.IsEmpty() || x.OpportunitySum >= request.MinOpportunitySum.Value) &&
                    (request.MaxOpportunitySum.IsEmpty() || x.OpportunitySum <= request.MaxOpportunitySum) &&
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

        public async Task<Guid> CreateAsync(Guid userId, Lead lead, CancellationToken ct)
        {
            var newLead = new Lead();
            var change = newLead.CreateWithLog(userId, x =>
            {
                x.Id = Guid.NewGuid();
                x.AccountId = lead.AccountId;
                x.SourceId = lead.SourceId;
                x.CreateUserId = userId;
                x.ResponsibleUserId = lead.ResponsibleUserId;
                x.Surname = lead.Surname;
                x.Name = lead.Name;
                x.Patronymic = lead.Patronymic;
                x.Phone = lead.Phone;
                x.Email = lead.Email;
                x.CompanyName = lead.CompanyName;
                x.Post = lead.Post;
                x.Postcode = lead.Postcode;
                x.Country = lead.Country;
                x.Region = lead.Region;
                x.Province = lead.Province;
                x.City = lead.City;
                x.Street = lead.Street;
                x.House = lead.House;
                x.Apartment = lead.Apartment;
                x.OpportunitySum = lead.OpportunitySum;
                x.IsDeleted = lead.IsDeleted;
                x.CreateDateTime = DateTime.UtcNow;
                x.AttributeLinks = lead.AttributeLinks;
            });

            var entry = await _storage.AddAsync(newLead, ct);
            await _storage.AddAsync(change, ct);
            await _storage.SaveChangesAsync(ct);

            return entry.Entity.Id;
        }

        public async Task UpdateAsync(Guid leadId, Lead oldLead, Lead newLead, CancellationToken ct)
        {
            var change = oldLead.UpdateWithLog(leadId, x =>
            {
                x.AccountId = newLead.AccountId;
                x.SourceId = newLead.SourceId;
                x.ResponsibleUserId = newLead.ResponsibleUserId;
                x.Surname = newLead.Surname;
                x.Name = newLead.Name;
                x.Patronymic = newLead.Patronymic;
                x.Phone = newLead.Phone;
                x.Email = newLead.Email;
                x.CompanyName = newLead.CompanyName;
                x.Post = newLead.Post;
                x.Postcode = newLead.Postcode;
                x.Country = newLead.Country;
                x.Region = newLead.Region;
                x.Province = newLead.Province;
                x.City = newLead.City;
                x.Street = newLead.Street;
                x.House = newLead.House;
                x.Apartment = newLead.Apartment;
                x.OpportunitySum = newLead.OpportunitySum;
                x.IsDeleted = newLead.IsDeleted;
                x.AttributeLinks = newLead.AttributeLinks;
            });

            _storage.Update(oldLead);
            await _storage.AddAsync(change, ct);
            await _storage.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(Guid leadId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<LeadChange>();

            await _storage.Leads
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(u => changes.Add(u.UpdateWithLog(leadId, x => x.IsDeleted = true)), ct);

            await _storage.AddRangeAsync(changes, ct);
            await _storage.SaveChangesAsync(ct);
        }

        public async Task RestoreAsync(Guid leadId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<LeadChange>();

            await _storage.Leads
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(u => changes.Add(u.UpdateWithLog(leadId, x => x.IsDeleted = false)), ct);

            await _storage.AddRangeAsync(changes, ct);
            await _storage.SaveChangesAsync(ct);
        }
    }
}