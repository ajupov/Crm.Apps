using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Contacts.Helpers;
using Crm.Apps.Areas.Contacts.Models;
using Crm.Apps.Areas.Contacts.Parameters;
using Crm.Apps.Areas.Contacts.Storages;
using Microsoft.EntityFrameworkCore;

namespace Crm.Apps.Areas.Contacts.Services
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
            return _storage.ContactAttributes.FirstOrDefaultAsync(x => x.Id == id, ct);
        }

        public Task<List<ContactAttribute>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct)
        {
            return _storage.ContactAttributes.Where(x => ids.Contains(x.Id)).ToListAsync(ct);
        }

        public Task<List<ContactAttribute>> GetPagedListAsync(ContactAttributeGetPagedListParameter parameter,
            CancellationToken ct)
        {
            return _storage.ContactAttributes.Where(x =>
                    (parameter.AccountId.IsEmpty() || x.AccountId == parameter.AccountId) &&
                    (parameter.Types == null || !parameter.Types.Any() || parameter.Types.Contains(x.Type)) &&
                    (parameter.Key.IsEmpty() || EF.Functions.Like(x.Key, $"{parameter.Key}%")) &&
                    (!parameter.IsDeleted.HasValue || x.IsDeleted == parameter.IsDeleted) &&
                    (!parameter.MinCreateDate.HasValue || x.CreateDateTime >= parameter.MinCreateDate) &&
                    (!parameter.MaxCreateDate.HasValue || x.CreateDateTime <= parameter.MaxCreateDate))
                .Sort(parameter.SortBy, parameter.OrderBy)
                .Skip(parameter.Offset)
                .Take(parameter.Limit)
                .ToListAsync(ct);
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

        public async Task UpdateAsync(Guid userId, ContactAttribute oldAttribute, ContactAttribute newAttribute,
            CancellationToken ct)
        {
            var change = oldAttribute.WithUpdateLog(userId, x =>
            {
                x.Type = newAttribute.Type;
                x.Key = newAttribute.Key;
                x.IsDeleted = newAttribute.IsDeleted;
            });

            _storage.Update(oldAttribute);
            await _storage.AddAsync(change, ct);
            await _storage.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<ContactAttributeChange>();

            await _storage.ContactAttributes.Where(x => ids.Contains(x.Id))
                .ForEachAsync(u => changes.Add(u.WithUpdateLog(userId, x => x.IsDeleted = true)), ct)
                ;

            await _storage.AddRangeAsync(changes, ct);
            await _storage.SaveChangesAsync(ct);
        }

        public async Task RestoreAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<ContactAttributeChange>();

            await _storage.ContactAttributes.Where(x => ids.Contains(x.Id))
                .ForEachAsync(u => changes.Add(u.WithUpdateLog(userId, x => x.IsDeleted = false)), ct)
                ;

            await _storage.AddRangeAsync(changes, ct);
            await _storage.SaveChangesAsync(ct);
        }
    }
}