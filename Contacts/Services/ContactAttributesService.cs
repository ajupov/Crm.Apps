using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Utils.All.Sorting;
using Ajupov.Utils.All.String;
using Crm.Apps.Contacts.Helpers;
using Crm.Apps.Contacts.Models;
using Crm.Apps.Contacts.Storages;
using Crm.Apps.Contacts.v1.Requests;
using Crm.Apps.Contacts.v1.Responses;
using Microsoft.EntityFrameworkCore;

namespace Crm.Apps.Contacts.Services
{
    public class ContactAttributesService : IContactAttributesService
    {
        private readonly ContactsStorage _storage;

        public ContactAttributesService(ContactsStorage storage)
        {
            _storage = storage;
        }

        public Task<ContactAttribute> GetAsync(Guid id, CancellationToken ct)
        {
            return _storage.ContactAttributes
                .FirstOrDefaultAsync(x => x.Id == id, ct);
        }

        public Task<List<ContactAttribute>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct)
        {
            return _storage.ContactAttributes
                .Where(x => ids.Contains(x.Id))
                .ToListAsync(ct);
        }

        public async Task<ContactAttributeGetPagedListResponse> GetPagedListAsync(
            Guid accountId,
            ContactAttributeGetPagedListRequest request,
            CancellationToken ct)
        {
            var attributes = _storage.ContactAttributes
                .Where(x =>
                    x.AccountId == accountId &&
                    (request.Types == null || !request.Types.Any() || request.Types.Contains(x.Type)) &&
                    (request.Key.IsEmpty() || EF.Functions.ILike(x.Key, $"{request.Key}%")) &&
                    (!request.IsDeleted.HasValue || x.IsDeleted == request.IsDeleted) &&
                    (!request.MinCreateDate.HasValue || x.CreateDateTime >= request.MinCreateDate) &&
                    (!request.MaxCreateDate.HasValue || x.CreateDateTime <= request.MaxCreateDate) &&
                    (!request.MinModifyDate.HasValue || x.ModifyDateTime >= request.MinModifyDate) &&
                    (!request.MaxModifyDate.HasValue || x.ModifyDateTime <= request.MaxModifyDate));

            return new ContactAttributeGetPagedListResponse
            {
                TotalCount = await attributes
                    .CountAsync(ct),
                LastModifyDateTime = await attributes
                    .MaxAsync(x => x.ModifyDateTime, ct),
                Attributes = await attributes
                    .SortBy(request.SortBy, request.OrderBy)
                    .Skip(request.Offset)
                    .Take(request.Limit)
                    .ToListAsync(ct)
            };
        }

        public async Task<Guid> CreateAsync(Guid userId, ContactAttribute attribute, CancellationToken ct)
        {
            var newAttribute = new ContactAttribute();
            var change = newAttribute.WithCreateLog(userId, x =>
            {
                x.Id = Guid.NewGuid();
                x.AccountId = attribute.AccountId;
                x.Type = attribute.Type;
                x.Key = attribute.Key;
                x.IsDeleted = attribute.IsDeleted;
                x.CreateDateTime = DateTime.UtcNow;
            });

            var entry = await _storage.AddAsync(newAttribute, ct);
            await _storage.AddAsync(change, ct);
            await _storage.SaveChangesAsync(ct);

            return entry.Entity.Id;
        }

        public async Task UpdateAsync(
            Guid userId,
            ContactAttribute oldAttribute,
            ContactAttribute newAttribute,
            CancellationToken ct)
        {
            var change = oldAttribute.WithUpdateLog(userId, x =>
            {
                x.Type = newAttribute.Type;
                x.Key = newAttribute.Key;
                x.IsDeleted = newAttribute.IsDeleted;
                x.ModifyDateTime = DateTime.UtcNow;
            });

            _storage.Update(oldAttribute);
            await _storage.AddAsync(change, ct);
            await _storage.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<ContactAttributeChange>();

            await _storage.ContactAttributes
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(x => changes.Add(x.WithUpdateLog(userId, a =>
                {
                    a.IsDeleted = true;
                    a.ModifyDateTime = DateTime.UtcNow;
                })), ct);

            await _storage.AddRangeAsync(changes, ct);
            await _storage.SaveChangesAsync(ct);
        }

        public async Task RestoreAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<ContactAttributeChange>();

            await _storage.ContactAttributes
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(x => changes.Add(x.WithUpdateLog(userId, a =>
                {
                    a.IsDeleted = false;
                    a.ModifyDateTime = DateTime.UtcNow;
                })), ct);

            await _storage.AddRangeAsync(changes, ct);
            await _storage.SaveChangesAsync(ct);
        }
    }
}