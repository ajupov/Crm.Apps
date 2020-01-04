using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Utils.All.Guid;
using Ajupov.Utils.All.Sorting;
using Ajupov.Utils.All.String;
using Crm.Apps.Users.Helpers;
using Crm.Apps.Users.Models;
using Crm.Apps.Users.RequestParameters;
using Crm.Apps.Users.Storages;
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
            return _storage.UserGroups
                .AsNoTracking()
                .Include(x => x.Roles)
                .FirstOrDefaultAsync(x => x.Id == id, ct);
        }

        public Task<List<UserGroup>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct)
        {
            return _storage.UserGroups
                .AsNoTracking()
                .Where(x => ids.Contains(x.Id))
                .ToListAsync(ct);
        }

        public Task<List<UserGroup>> GetPagedListAsync(UserGroupGetPagedListRequestParameter request, CancellationToken ct)
        {
            return _storage.UserGroups
                .AsNoTracking()
                .Include(x => x.Roles)
                .Where(x =>
                    (request.AccountId.IsEmpty() || x.AccountId == request.AccountId) &&
                    (request.Name.IsEmpty() || EF.Functions.Like(x.Name, $"{request.Name}%")) &&
                    (!request.IsDeleted.HasValue || x.IsDeleted == request.IsDeleted) &&
                    (!request.MinCreateDate.HasValue || x.CreateDateTime >= request.MinCreateDate) &&
                    (!request.MaxCreateDate.HasValue || x.CreateDateTime <= request.MaxCreateDate) &&
                    (!request.MinModifyDate.HasValue || x.ModifyDateTime >= request.MinModifyDate) &&
                    (!request.MaxModifyDate.HasValue || x.ModifyDateTime <= request.MaxModifyDate))
                .SortBy(request.SortBy, request.OrderBy)
                .Skip(request.Offset)
                .Take(request.Limit)
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
                x.Roles = group.Roles;
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
                x.Roles = newGroup.Roles;
            });

            _storage.Update(oldGroup);
            await _storage.AddAsync(change, ct);
            await _storage.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<UserGroupChange>();

            await _storage.UserGroups
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(u => changes.Add(u.WithUpdateLog(userId, x => x.IsDeleted = true)), ct);

            await _storage.AddRangeAsync(changes, ct);
            await _storage.SaveChangesAsync(ct);
        }

        public async Task RestoreAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<UserGroupChange>();

            await _storage.UserGroups
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(u => changes.Add(u.WithUpdateLog(userId, x => x.IsDeleted = false)), ct);

            await _storage.AddRangeAsync(changes, ct);
            await _storage.SaveChangesAsync(ct);
        }
    }
}