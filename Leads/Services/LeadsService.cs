﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Utils.All.Decimal;
using Ajupov.Utils.All.Guid;
using Ajupov.Utils.All.Sorting;
using Ajupov.Utils.All.String;
using Crm.Apps.Leads.Helpers;
using Crm.Apps.Leads.Mappers;
using Crm.Apps.Leads.Models;
using Crm.Apps.Leads.Storages;
using Crm.Apps.Leads.V1.Requests;
using Crm.Apps.Leads.V1.Responses;
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

        public Task<Lead> GetAsync(Guid id, bool isTrackChanges, CancellationToken ct)
        {
            return _storage.Leads
                .AsTracking(isTrackChanges ? QueryTrackingBehavior.TrackAll : QueryTrackingBehavior.NoTracking)
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

        public async Task<LeadGetPagedListResponse> GetPagedListAsync(
            Guid accountId,
            LeadGetPagedListRequest request,
            CancellationToken ct)
        {
            var leads = await _storage.Leads
                .AsNoTracking()
                .Include(x => x.Source)
                .Include(x => x.AttributeLinks)
                .Where(x =>
                    x.AccountId == accountId &&
                    (request.Surname.IsEmpty() || EF.Functions.ILike(x.Surname, $"{request.Surname}%")) &&
                    (request.Name.IsEmpty() || EF.Functions.ILike(x.Name, $"{request.Name}%")) &&
                    (request.Patronymic.IsEmpty() || EF.Functions.ILike(x.Patronymic, $"{request.Patronymic}%")) &&
                    (request.Phone.IsEmpty() || x.Phone == request.Phone) &&
                    (request.Email.IsEmpty() || x.Email == request.Email) &&
                    (request.CompanyName.IsEmpty() ||
                     EF.Functions.ILike(x.CompanyName, $"{request.CompanyName}%")) &&
                    (request.Post.IsEmpty() || EF.Functions.ILike(x.Post, $"{request.Post}%")) &&
                    (request.Postcode.IsEmpty() || x.Postcode == request.Postcode) &&
                    (request.Country.IsEmpty() || EF.Functions.ILike(x.Country, $"{request.Country}%")) &&
                    (request.Region.IsEmpty() || EF.Functions.ILike(x.Region, $"{request.Region}%")) &&
                    (request.Province.IsEmpty() || EF.Functions.ILike(x.Province, $"{request.Province}%")) &&
                    (request.City.IsEmpty() || EF.Functions.ILike(x.City, $"{request.City}%")) &&
                    (request.Street.IsEmpty() || EF.Functions.ILike(x.Street, $"{request.Street}%")) &&
                    (request.House.IsEmpty() || EF.Functions.ILike(x.House, $"{request.House}%")) &&
                    (request.Apartment.IsEmpty() || x.Apartment == request.Apartment) &&
                    (request.MinOpportunitySum.IsEmpty() || x.OpportunitySum >= request.MinOpportunitySum) &&
                    (request.MaxOpportunitySum.IsEmpty() || x.OpportunitySum <= request.MaxOpportunitySum) &&
                    (!request.IsDeleted.HasValue || x.IsDeleted == request.IsDeleted) &&
                    (!request.MinCreateDate.HasValue || x.CreateDateTime >= request.MinCreateDate) &&
                    (!request.MaxCreateDate.HasValue || x.CreateDateTime <= request.MaxCreateDate) &&
                    (!request.MinModifyDate.HasValue || x.ModifyDateTime >= request.MinModifyDate) &&
                    (!request.MaxModifyDate.HasValue || x.ModifyDateTime <= request.MaxModifyDate))
                .ToListAsync(ct);

            return new LeadGetPagedListResponse
            {
                TotalCount = leads
                    .Count(x => x.FilterByAdditional(request)),
                LastModifyDateTime = leads
                    .Max(x => x.ModifyDateTime),
                Leads = leads
                    .Where(x => x.FilterByAdditional(request))
                    .AsQueryable()
                    .SortBy(request.SortBy, request.OrderBy)
                    .Skip(request.Offset)
                    .Take(request.Limit)
                    .ToList()
            };
        }

        public async Task<Guid> CreateAsync(Guid userId, Lead lead, CancellationToken ct)
        {
            var newLead = new Lead();
            var change = newLead.CreateWithLog(userId, x =>
            {
                x.Id = !lead.Id.IsEmpty() ? lead.Id : Guid.NewGuid();
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
                x.AttributeLinks = lead.AttributeLinks.Map(x.Id);
            });

            var entry = await _storage.AddAsync(newLead, ct);
            await _storage.AddAsync(change, ct);
            await _storage.SaveChangesAsync(ct);

            return entry.Entity.Id;
        }

        public async Task UpdateAsync(Guid userId, Lead oldLead, Lead newLead, CancellationToken ct)
        {
            var change = oldLead.UpdateWithLog(userId, x =>
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
                x.ModifyDateTime = DateTime.UtcNow;
                x.AttributeLinks = newLead.AttributeLinks.Map(x.Id);
            });

            _storage.Update(oldLead);
            await _storage.AddAsync(change, ct);
            await _storage.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<LeadChange>();

            await _storage.Leads
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(x => changes.Add(x.UpdateWithLog(userId, l =>
                {
                    l.IsDeleted = true;
                    l.ModifyDateTime = DateTime.UtcNow;
                })), ct);

            await _storage.AddRangeAsync(changes, ct);
            await _storage.SaveChangesAsync(ct);
        }

        public async Task RestoreAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<LeadChange>();

            await _storage.Leads
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(x => changes.Add(x.UpdateWithLog(userId, l =>
                {
                    l.IsDeleted = false;
                    l.ModifyDateTime = DateTime.UtcNow;
                })), ct);

            await _storage.AddRangeAsync(changes, ct);
            await _storage.SaveChangesAsync(ct);
        }
    }
}
