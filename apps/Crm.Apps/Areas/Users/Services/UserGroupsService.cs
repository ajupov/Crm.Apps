using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Users.Helpers;
using Crm.Apps.Areas.Users.Models;
using Crm.Apps.Areas.Users.Storages;
using Crm.Utils.String;
using Microsoft.EntityFrameworkCore;

namespace Crm.Apps.Areas.Users.Services
{
    public class UserGroupsService : IUserGroupsService
    {
        private readonly UsersStorage _storage;

        public UserGroupsService(UsersStorage storage)
        {
            _storage = storage;
        }

        public Task<UserGroup> GetAsync(Guid id, CancellationToken ct)
        {
            return _storage.UserGroups.FirstOrDefaultAsync(x => x.Id == id, ct);
        }

        public Task<List<UserGroup>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct)
        {
            return _storage.UserGroups.Where(x => ids.Contains(x.Id)).ToListAsync(ct);
        }

        public Task<List<UserGroup>> GetPagedListAsync(Guid? accountId, string name, bool? isDeleted,
            DateTime? minCreateDate, DateTime? maxCreateDate, int offset, int limit, string sortBy, string orderBy,
            CancellationToken ct)
        {
            return _storage.UserGroups.Where(x =>
                    (!accountId.HasValue || x.AccountId == accountId) &&
                    (name.IsEmpty() || EF.Functions.Like(x.Name, $"{name}%")) &&
                    (!isDeleted.HasValue || x.IsDeleted == isDeleted) &&
                    (!minCreateDate.HasValue || x.CreateDateTime >= minCreateDate) &&
                    (!maxCreateDate.HasValue || x.CreateDateTime <= maxCreateDate))
                .Sort(sortBy, orderBy)
                .Skip(offset)
                .Take(limit)
                .ToListAsync(ct);
        }

        public async Task<Guid> CreateAsync(Guid userId, UserGroup group, CancellationToken ct)
        {
            var newGroup = new UserGroup();
            var change = newGroup.WithCreateLog(userId, x =>
            {
                x.Id = Guid.NewGuid();
                x.Name = group.Name;
                x.CreateDateTime = DateTime.UtcNow;
            });

            var entry = await _storage.AddAsync(newGroup, ct).ConfigureAwait(false);
            await _storage.AddAsync(change, ct).ConfigureAwait(false);
            await _storage.SaveChangesAsync(ct).ConfigureAwait(false);

            return entry.Entity.Id;
        }

        public async Task UpdateAsync(Guid userId, UserGroup oldGroup, UserGroup newGroup, CancellationToken ct)
        {
            var change = oldGroup.WithUpdateLog(userId, x =>
            {
                x.Name = newGroup.Name;
                x.IsDeleted = newGroup.IsDeleted;
            });

            _storage.Update(oldGroup);
            await _storage.AddAsync(change, ct).ConfigureAwait(false);
            await _storage.SaveChangesAsync(ct).ConfigureAwait(false);
        }

        public async Task DeleteAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<UserGroupChange>();

            await _storage.UserGroups.Where(x => ids.Contains(x.Id))
                .ForEachAsync(u => changes.Add(u.WithUpdateLog(userId, x => x.IsDeleted = true)), ct)
                .ConfigureAwait(false);

            await _storage.AddRangeAsync(changes, ct).ConfigureAwait(false);
            await _storage.SaveChangesAsync(ct).ConfigureAwait(false);
        }

        public async Task RestoreAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<UserGroupChange>();

            await _storage.UserGroups.Where(x => ids.Contains(x.Id))
                .ForEachAsync(u => changes.Add(u.WithUpdateLog(userId, x => x.IsDeleted = false)), ct)
                .ConfigureAwait(false);

            await _storage.AddRangeAsync(changes, ct).ConfigureAwait(false);
            await _storage.SaveChangesAsync(ct).ConfigureAwait(false);
        }
    }
}