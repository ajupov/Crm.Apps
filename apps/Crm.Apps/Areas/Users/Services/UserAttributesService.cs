using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Users.Helpers;
using Crm.Apps.Areas.Users.Models;
using Crm.Apps.Areas.Users.Storages;
using Crm.Common.Types;
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
            return _storage.UserAttributes.Include(x => x.Links).Include(x => x.Changes)
                .FirstOrDefaultAsync(x => x.Id == id, ct);
        }

        public Task<List<UserAttribute>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct)
        {
            return _storage.UserAttributes.Where(x => ids.Contains(x.Id)).ToListAsync(ct);
        }

        public Task<List<UserAttribute>> GetPagedListAsync(Guid? accountId, AttributeType? type, string key,
            bool? isDeleted, DateTime? minCreateDate, DateTime? maxCreateDate, int offset, int limit, string sortBy,
            string orderBy, CancellationToken ct)
        {
            return _storage.UserAttributes.Where(x =>
                    (!accountId.HasValue || x.AccountId == accountId) &&
                    (!type.HasValue || x.Type == type) &&
                    (key.IsEmpty() || EF.Functions.Like(x.Key, $"{key}%")) &&
                    (!isDeleted.HasValue || x.IsDeleted == isDeleted) &&
                    (!minCreateDate.HasValue || x.CreateDateTime >= minCreateDate) &&
                    (!maxCreateDate.HasValue || x.CreateDateTime <= maxCreateDate))
                .Sort(sortBy, orderBy)
                .Skip(offset)
                .Take(limit)
                .ToListAsync(ct);
        }

        public async Task<Guid> CreateAsync(Guid userId, UserAttribute attribute, CancellationToken ct)
        {
            var newAttribute = new UserAttribute().CreateWithLog(userId, x =>
            {
                x.Id = Guid.NewGuid();
                x.Type = attribute.Type;
                x.Key = attribute.Key;
                x.CreateDateTime = DateTime.UtcNow;
            });

            var entry = await _storage.AddAsync(newAttribute, ct).ConfigureAwait(false);

            await _storage.SaveChangesAsync(ct).ConfigureAwait(false);

            return entry.Entity.Id;
        }

        public Task UpdateAsync(Guid userId, UserAttribute oldAttribute, UserAttribute newAttribute,
            CancellationToken ct)
        {
            oldAttribute.UpdateWithLog(userId, x =>
            {
                x.Type = newAttribute.Type;
                x.Key = newAttribute.Key;
                x.IsDeleted = newAttribute.IsDeleted;
            });

            _storage.Update(oldAttribute);

            return _storage.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            await _storage.UserAttributes.Where(x => ids.Contains(x.Id))
                .ForEachAsync(u => u.UpdateWithLog(userId, x => x.IsDeleted = true), ct).ConfigureAwait(false);

            await _storage.SaveChangesAsync(ct).ConfigureAwait(false);
        }

        public async Task RestoreAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            await _storage.UserAttributes.Where(x => ids.Contains(x.Id))
                .ForEachAsync(u => u.UpdateWithLog(userId, x => x.IsDeleted = false), ct).ConfigureAwait(false);

            await _storage.SaveChangesAsync(ct).ConfigureAwait(false);
        }
    }
}