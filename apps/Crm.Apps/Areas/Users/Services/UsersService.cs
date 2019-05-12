using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Users.Helpers;
using Crm.Apps.Areas.Users.Models;
using Crm.Apps.Areas.Users.Storages;
using Crm.Common.UserContext;
using Crm.Utils.String;
using Microsoft.EntityFrameworkCore;

namespace Crm.Apps.Areas.Users.Services
{
    public class UsersService : IUsersService
    {
        private readonly UsersStorage _storage;

        public UsersService(UsersStorage storage)
        {
            _storage = storage;
        }

        public Task<User> GetAsync(Guid id, CancellationToken ct)
        {
            return _storage.Users.Include(x => x.AttributeLinks).Include(x => x.Permissions).Include(x => x.GroupLinks)
                .Include(x => x.Settings).Include(x => x.Changes).FirstOrDefaultAsync(x => x.Id == id, ct);
        }

        public Task<List<User>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct)
        {
            return _storage.Users.Where(x => ids.Contains(x.Id)).ToListAsync(ct);
        }

        public Task<List<User>> GetPagedListAsync(Guid? accountId, string surname, string name, string patronymic,
            DateTime? minBirthDate, DateTime? maxBirthDate, UserGender? gender, bool? isLocked, bool? isDeleted,
            DateTime? minCreateDate, DateTime? maxCreateDate, bool? allAttributes, IDictionary<Guid, string> attributes,
            bool? allPermissions, List<Permission> permissions, bool? allGroupIds, List<Guid> groupIds,
            int offset, int limit, string sortBy, string orderBy, CancellationToken ct)
        {
            return _storage.Users.Where(x =>
                    (!accountId.HasValue || x.AccountId == accountId) &&
                    (surname.IsEmpty() || EF.Functions.Like(x.Surname, $"{surname}%")) &&
                    (name.IsEmpty() || EF.Functions.Like(x.Name, $"{name}%")) &&
                    (patronymic.IsEmpty() || EF.Functions.Like(x.Patronymic, $"{patronymic}%")) &&
                    (!minBirthDate.HasValue || x.BirthDate >= minBirthDate) &&
                    (!maxBirthDate.HasValue || x.BirthDate <= maxBirthDate) &&
                    (!gender.HasValue || x.Gender == gender) &&
                    (!isLocked.HasValue || x.IsLocked == isLocked) &&
                    (!isDeleted.HasValue || x.IsDeleted == isDeleted) &&
                    (!minCreateDate.HasValue || x.CreateDateTime >= minCreateDate) &&
                    (!maxCreateDate.HasValue || x.CreateDateTime <= maxCreateDate) &&
                    (!attributes.Any() || x.FilterByAttributes(allAttributes, attributes)) &&
                    (!permissions.Any() || x.FilterByPermissions(allPermissions, permissions)) &&
                    (!groupIds.Any() || x.FilterByGroupIds(allGroupIds, groupIds)))
                .Sort(sortBy, orderBy)
                .Skip(offset)
                .Take(limit)
                .ToListAsync(ct);
        }

        public async Task<Guid> CreateAsync(Guid userId, User user, CancellationToken ct)
        {
            var newUser = new User().CreateWithLog(userId, x =>
            {
                x.Id = new Guid();
                x.AccountId = user.AccountId;
                x.Surname = user.Surname;
                x.Name = user.Name;
                x.Patronymic = user.Patronymic;
                x.BirthDate = user.BirthDate;
                x.Gender = user.Gender;
                x.AvatarUrl = user.AvatarUrl;
                x.CreateDateTime = DateTime.UtcNow;
            });

            var entry = await _storage.AddAsync(newUser, ct).ConfigureAwait(false);

            await _storage.SaveChangesAsync(ct).ConfigureAwait(false);

            return entry.Entity.Id;
        }

        public Task UpdateAsync(Guid userId, User oldUser, User newUser, CancellationToken ct)
        {
            oldUser.UpdateWithLog(userId, x =>
            {
                x.Surname = newUser.Surname;
                x.Name = newUser.Name;
                x.Patronymic = newUser.Patronymic;
                x.BirthDate = newUser.BirthDate;
                x.Gender = newUser.Gender;
                x.AvatarUrl = newUser.AvatarUrl;
                x.IsLocked = newUser.IsLocked;
                x.IsDeleted = newUser.IsDeleted;
                x.AttributeLinks = newUser.AttributeLinks;
                x.Permissions = newUser.Permissions;
                x.GroupLinks = newUser.GroupLinks;
                x.Settings = newUser.Settings;
            });

            _storage.Update(oldUser);

            return _storage.SaveChangesAsync(ct);
        }

        public async Task LockAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            await _storage.Users.Where(x => ids.Contains(x.Id))
                .ForEachAsync(u => u.UpdateWithLog(userId, x => x.IsLocked = true), ct).ConfigureAwait(false);

            await _storage.SaveChangesAsync(ct).ConfigureAwait(false);
        }

        public async Task UnlockAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            await _storage.Users.Where(x => ids.Contains(x.Id))
                .ForEachAsync(u => u.UpdateWithLog(userId, x => x.IsLocked = false), ct).ConfigureAwait(false);

            await _storage.SaveChangesAsync(ct).ConfigureAwait(false);
        }

        public async Task DeleteAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            await _storage.Users.Where(x => ids.Contains(x.Id))
                .ForEachAsync(u => u.UpdateWithLog(userId, x => x.IsDeleted = true), ct).ConfigureAwait(false);

            await _storage.SaveChangesAsync(ct).ConfigureAwait(false);
        }

        public async Task RestoreAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            await _storage.Users.Where(x => ids.Contains(x.Id))
                .ForEachAsync(u => u.UpdateWithLog(userId, x => x.IsDeleted = false), ct).ConfigureAwait(false);

            await _storage.SaveChangesAsync(ct).ConfigureAwait(false);
        }
    }
}