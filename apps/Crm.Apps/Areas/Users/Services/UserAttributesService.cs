using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Users.Helpers;
using Crm.Apps.Areas.Users.Models;
using Crm.Apps.Areas.Users.Parameters;
using Crm.Apps.Areas.Users.Storages;
using Crm.Utils.String;
using Microsoft.EntityFrameworkCore;

namespace Crm.Apps.Areas.Users.Services
{
    public class UserAttributesService : IUserAttributesService
    {
        private readonly UsersStorage _storage;

        public UserAttributesService(UsersStorage storage)
        {
            _storage = storage;
        }

        public Task<UserAttribute> GetAsync(Guid id, CancellationToken ct)
        {
            return _storage.UserAttributes.FirstOrDefaultAsync(x => x.Id == id, ct);
        }

        public Task<List<UserAttribute>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct)
        {
            return _storage.UserAttributes.Where(x => ids.Contains(x.Id)).ToListAsync(ct);
        }

        public Task<List<UserAttribute>> GetPagedListAsync(UserAttributeGetPagedListParameter parameter,
            CancellationToken ct)
        {
            return _storage.UserAttributes.Where(x =>
                    (!parameter.AccountId.HasValue || x.AccountId == parameter.AccountId) &&
                    (!parameter.Type.HasValue || x.Type == parameter.Type) &&
                    (parameter.Key.IsEmpty() || EF.Functions.Like(x.Key, $"{parameter.Key}%")) &&
                    (!parameter.IsDeleted.HasValue || x.IsDeleted == parameter.IsDeleted) &&
                    (!parameter.MinCreateDate.HasValue || x.CreateDateTime >= parameter.MinCreateDate) &&
                    (!parameter.MaxCreateDate.HasValue || x.CreateDateTime <= parameter.MaxCreateDate))
                .Sort(parameter.SortBy, parameter.OrderBy)
                .Skip(parameter.Offset)
                .Take(parameter.Limit)
                .ToListAsync(ct);
        }

        public async Task<Guid> CreateAsync(Guid userId, UserAttribute attribute, CancellationToken ct)
        {
            var newAttribute = new UserAttribute();
            var change = newAttribute.WithCreateLog(userId, x =>
            {
                x.Id = Guid.NewGuid();
                x.AccountId = attribute.AccountId;
                x.Type = attribute.Type;
                x.Key = attribute.Key;
                x.IsDeleted = attribute.IsDeleted;
                x.CreateDateTime = DateTime.UtcNow;
            });

            var entry = await _storage.AddAsync(newAttribute, ct).ConfigureAwait(false);
            await _storage.AddAsync(change, ct).ConfigureAwait(false);
            await _storage.SaveChangesAsync(ct).ConfigureAwait(false);

            return entry.Entity.Id;
        }

        public async Task UpdateAsync(Guid userId, UserAttribute oldAttribute, UserAttribute newAttribute,
            CancellationToken ct)
        {
            var change = oldAttribute.WithUpdateLog(userId, x =>
            {
                x.Type = newAttribute.Type;
                x.Key = newAttribute.Key;
                x.IsDeleted = newAttribute.IsDeleted;
            });

            _storage.Update(oldAttribute);
            await _storage.AddAsync(change, ct).ConfigureAwait(false);
            await _storage.SaveChangesAsync(ct).ConfigureAwait(false);
        }

        public async Task DeleteAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<UserAttributeChange>();

            await _storage.UserAttributes.Where(x => ids.Contains(x.Id))
                .ForEachAsync(u => changes.Add(u.WithUpdateLog(userId, x => x.IsDeleted = true)), ct)
                .ConfigureAwait(false);

            await _storage.AddRangeAsync(changes, ct).ConfigureAwait(false);
            await _storage.SaveChangesAsync(ct).ConfigureAwait(false);
        }

        public async Task RestoreAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<UserAttributeChange>();

            await _storage.UserAttributes.Where(x => ids.Contains(x.Id))
                .ForEachAsync(u => changes.Add(u.WithUpdateLog(userId, x => x.IsDeleted = false)), ct)
                .ConfigureAwait(false);

            await _storage.AddRangeAsync(changes, ct).ConfigureAwait(false);
            await _storage.SaveChangesAsync(ct).ConfigureAwait(false);
        }
    }
}