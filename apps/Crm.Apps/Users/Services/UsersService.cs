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
                .Include(x => x.Settings).FirstOrDefaultAsync(x => x.Id == id, ct);
        }

        public Task<List<User>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct)
        {
            return _storage.Users.Where(x => ids.Contains(x.Id)).ToListAsync(ct);
        }

        public async Task<List<User>> GetPagedListAsync(UserGetPagedListParameter parameter, CancellationToken ct)
        {
            var temp = await _storage.Users.Include(x => x.AttributeLinks).Include(x => x.Permissions)
                .Include(x => x.GroupLinks).Include(x => x.Settings).Where(x =>
                    (parameter.AccountId.IsEmpty() || x.AccountId == parameter.AccountId) &&
                    (parameter.Surname.IsEmpty() || EF.Functions.Like(x.Surname, $"{parameter.Surname}%")) &&
                    (parameter.Name.IsEmpty() || EF.Functions.Like(x.Name, $"{parameter.Name}%")) &&
                    (parameter.Patronymic.IsEmpty() || EF.Functions.Like(x.Patronymic, $"{parameter.Patronymic}%")) &&
                    (!parameter.MinBirthDate.HasValue || x.BirthDate >= parameter.MinBirthDate) &&
                    (!parameter.MaxBirthDate.HasValue || x.BirthDate <= parameter.MaxBirthDate) &&
                    (!parameter.Gender.HasValue || x.Gender == parameter.Gender) &&
                    (!parameter.IsLocked.HasValue || x.IsLocked == parameter.IsLocked) &&
                    (!parameter.IsDeleted.HasValue || x.IsDeleted == parameter.IsDeleted) &&
                    (!parameter.MinCreateDate.HasValue || x.CreateDateTime >= parameter.MinCreateDate) &&
                    (!parameter.MaxCreateDate.HasValue || x.CreateDateTime <= parameter.MaxCreateDate))
                .Sort(parameter.SortBy, parameter.OrderBy)
                .ToListAsync(ct).ConfigureAwait(false);

            return temp.Where(x => x.FilterByAdditional(parameter)).Skip(parameter.Offset).Take(parameter.Limit)
                .ToList();
        }

        public async Task<Guid> CreateAsync(Guid userId, User user, CancellationToken ct)
        {
            var newUser = new User();
            var change = newUser.CreateWithLog(userId, x =>
            {
                x.Id = Guid.NewGuid();
                x.AccountId = user.AccountId;
                x.Surname = user.Surname;
                x.Name = user.Name;
                x.Patronymic = user.Patronymic;
                x.BirthDate = user.BirthDate;
                x.Gender = user.Gender;
                x.AvatarUrl = user.AvatarUrl;
                x.IsLocked = user.IsLocked;
                x.IsDeleted = user.IsDeleted;
                x.CreateDateTime = DateTime.UtcNow;
                x.AttributeLinks = user.AttributeLinks;
                x.Permissions = user.Permissions;
                x.GroupLinks = user.GroupLinks;
                x.Settings = user.Settings;
            });

            var entry = await _storage.AddAsync(newUser, ct).ConfigureAwait(false);
            await _storage.AddAsync(change, ct).ConfigureAwait(false);
            await _storage.SaveChangesAsync(ct).ConfigureAwait(false);

            return entry.Entity.Id;
        }

        public async Task UpdateAsync(Guid userId, User oldUser, User newUser, CancellationToken ct)
        {
            var change = oldUser.UpdateWithLog(userId, x =>
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
            await _storage.AddAsync(change, ct).ConfigureAwait(false);
            await _storage.SaveChangesAsync(ct).ConfigureAwait(false);
        }

        public async Task LockAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<UserChange>();

            await _storage.Users.Where(x => ids.Contains(x.Id))
                .ForEachAsync(u => changes.Add(u.UpdateWithLog(userId, x => x.IsLocked = true)), ct)
                .ConfigureAwait(false);

            await _storage.AddRangeAsync(changes, ct).ConfigureAwait(false);
            await _storage.SaveChangesAsync(ct).ConfigureAwait(false);
        }

        public async Task UnlockAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<UserChange>();

            await _storage.Users.Where(x => ids.Contains(x.Id))
                .ForEachAsync(u => changes.Add(u.UpdateWithLog(userId, x => x.IsLocked = false)), ct)
                .ConfigureAwait(false);

            await _storage.AddRangeAsync(changes, ct).ConfigureAwait(false);
            await _storage.SaveChangesAsync(ct).ConfigureAwait(false);
        }

        public async Task DeleteAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<UserChange>();

            await _storage.Users.Where(x => ids.Contains(x.Id))
                .ForEachAsync(u => changes.Add(u.UpdateWithLog(userId, x => x.IsDeleted = true)), ct)
                .ConfigureAwait(false);

            await _storage.AddRangeAsync(changes, ct).ConfigureAwait(false);
            await _storage.SaveChangesAsync(ct).ConfigureAwait(false);
        }

        public async Task RestoreAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<UserChange>();

            await _storage.Users.Where(x => ids.Contains(x.Id))
                .ForEachAsync(u => changes.Add(u.UpdateWithLog(userId, x => x.IsDeleted = false)), ct)
                .ConfigureAwait(false);

            await _storage.AddRangeAsync(changes, ct).ConfigureAwait(false);
            await _storage.SaveChangesAsync(ct).ConfigureAwait(false);
        }
    }
}