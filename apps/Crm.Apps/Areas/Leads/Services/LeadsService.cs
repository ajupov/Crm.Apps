using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Leads.Helpers;
using Crm.Apps.Areas.Leads.Models;
using Crm.Apps.Areas.Leads.Parameters;
using Crm.Apps.Areas.Leads.Storages;
using Microsoft.EntityFrameworkCore;

namespace Crm.Apps.Areas.Leads.Services
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
            return _storage.Leads.Include(x => x.Source).Include(x => x.AttributeLinks)
                .FirstOrDefaultAsync(x => x.Id == id, ct);
        }

        public Task<List<Lead>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct)
        {
            return _storage.Leads.Where(x => ids.Contains(x.Id)).ToListAsync(ct);
        }

        public async Task<List<Lead>> GetPagedListAsync(LeadGetPagedListParameter parameter, CancellationToken ct)
        {
            var temp = await _storage.Leads.Include(x => x.Source).Include(x => x.AttributeLinks).Where(x =>
                    (parameter.AccountId.IsEmpty() || x.AccountId == parameter.AccountId) &&
                    (parameter.Surname.IsEmpty() || EF.Functions.Like(x.Surname, $"{parameter.Surname}%")) &&
                    (parameter.Name.IsEmpty() || EF.Functions.Like(x.Name, $"{parameter.Name}%")) &&
                    (parameter.Patronymic.IsEmpty() || EF.Functions.Like(x.Patronymic, $"{parameter.Patronymic}%")) &&
                    (parameter.Phone.IsEmpty() || x.Phone == parameter.Phone) &&
                    (parameter.Email.IsEmpty() || x.Email == parameter.Email) &&
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
                .ToListAsync(ct);

            return temp.Where(x => x.FilterByAdditional(parameter)).Skip(parameter.Offset).Take(parameter.Limit)
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

            await _storage.Leads.Where(x => ids.Contains(x.Id))
                .ForEachAsync(u => changes.Add(u.UpdateWithLog(leadId, x => x.IsDeleted = true)), ct)
                ;

            await _storage.AddRangeAsync(changes, ct);
            await _storage.SaveChangesAsync(ct);
        }

        public async Task RestoreAsync(Guid leadId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<LeadChange>();

            await _storage.Leads.Where(x => ids.Contains(x.Id))
                .ForEachAsync(u => changes.Add(u.UpdateWithLog(leadId, x => x.IsDeleted = false)), ct)
                ;

            await _storage.AddRangeAsync(changes, ct);
            await _storage.SaveChangesAsync(ct);
        }
    }
}