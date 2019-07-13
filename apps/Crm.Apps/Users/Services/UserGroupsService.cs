using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Users.Helpers;
using Crm.Apps.Users.Models;
using Crm.Apps.Users.Parameters;
using Crm.Apps.Users.Storages;
using Crm.Utils.Guid;
using Crm.Utils.String;
using Microsoft.EntityFrameworkCore;

namespace Crm.Apps.Users.Services
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
            return _storage.UserGroups.Include(x => x.Permissions).FirstOrDefaultAsync(x => x.Id == id, ct);
        }

        public Task<List<UserGroup>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct)
        {
            return _storage.UserGroups.Where(x => ids.Contains(x.Id)).ToListAsync(ct);
        }

        public Task<List<UserGroup>> GetPagedListAsync(UserGroupGetPagedListParameter parameter, CancellationToken ct)
        {
            return _storage.UserGroups.Include(x => x.Permissions).Where(x =>
                    (parameter.AccountId.IsEmpty() || x.AccountId == parameter.AccountId) &&
                    (parameter.Name.IsEmpty() || EF.Functions.Like(x.Name, $"{parameter.Name}%")) &&
                    (!parameter.IsDeleted.HasValue || x.IsDeleted == parameter.IsDeleted) &&
                    (!parameter.MinCreateDate.HasValue || x.CreateDateTime >= parameter.MinCreateDate) &&
                    (!parameter.MaxCreateDate.HasValue || x.CreateDateTime <= parameter.MaxCreateDate))
                .Sort(parameter.SortBy, parameter.OrderBy)
                .Skip(parameter.Offset)
                .Take(parameter.Limit)
                .ToListAsync(ct);
        }

        public async Task<Guid> CreateAsync(Guid userId, UserGroup group, CancellationToken ct)
        {
            var newGroup = new UserGroup();
            var change = newGroup.WithCreateLog(userId, x =>
            {
                x.Id = Guid.NewGuid();
                x.AccountId = group.AccountId;
                x.Name = group.Name;
                x.IsDeleted = group.IsDeleted;
                x.CreateDateTime = DateTime.UtcNow;
                x.Permissions = group.Permissions;
            });

            var entry = await _storage.AddAsync(newGroup, ct);
            await _storage.AddAsync(change, ct);
            await _storage.SaveChangesAsync(ct);

            return entry.Entity.Id;
        }

        public async Task UpdateAsync(Guid userId, UserGroup oldGroup, UserGroup newGroup, CancellationToken ct)
        {
            var change = oldGroup.WithUpdateLog(userId, x =>
            {
                x.Name = newGroup.Name;
                x.IsDeleted = newGroup.IsDeleted;
                x.Permissions = newGroup.Permissions;
            });

            _storage.Update(oldGroup);
            await _storage.AddAsync(change, ct);
            await _storage.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<UserGroupChange>();

            await _storage.UserGroups.Where(x => ids.Contains(x.Id))
                .ForEachAsync(u => changes.Add(u.WithUpdateLog(userId, x => x.IsDeleted = true)), ct)
                ;

            await _storage.AddRangeAsync(changes, ct);
            await _storage.SaveChangesAsync(ct);
        }

        public async Task RestoreAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<UserGroupChange>();

            await _storage.UserGroups.Where(x => ids.Contains(x.Id))
                .ForEachAsync(u => changes.Add(u.WithUpdateLog(userId, x => x.IsDeleted = false)), ct)
                ;

            await _storage.AddRangeAsync(changes, ct);
            await _storage.SaveChangesAsync(ct);
        }
    }
}