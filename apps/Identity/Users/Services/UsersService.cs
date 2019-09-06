using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Utils.Sorting;
using Crm.Utils.String;
using Identity.Users.Models;
using Identity.Users.Parameters;
using Identity.Users.Storages;
using Microsoft.EntityFrameworkCore;

namespace Identity.Users.Services
{
    public class UsersService : IUsersService
    {
        private readonly UsersStorage _usersStorage;

        public UsersService(UsersStorage usersStorage)
        {
            _usersStorage = usersStorage;
        }

        public Task<User> GetAsync(Guid id, CancellationToken ct)
        {
            return _usersStorage.Users
                .FirstOrDefaultAsync(x => x.Id == id, ct);
        }

        public Task<User[]> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct)
        {
            return _usersStorage.Users
                .Where(x => ids.Contains(x.Id))
                .ToArrayAsync(ct);
        }

        public Task<User[]> GetPagedListAsync(UserGetPagedListParameter parameter, CancellationToken ct)
        {
            return _usersStorage.Users
                .Where(x =>
                    (parameter.Surname.IsEmpty() || EF.Functions.Like(x.Surname, $"{parameter.Surname}%")) &&
                    (parameter.Name.IsEmpty() || EF.Functions.Like(x.Name, $"{parameter.Name}%")) &&
                    (!parameter.MinBirthDate.HasValue || x.BirthDate >= parameter.MinBirthDate) &&
                    (!parameter.MaxBirthDate.HasValue || x.BirthDate <= parameter.MaxBirthDate) &&
                    (!parameter.Gender.HasValue || x.Gender == parameter.Gender) &&
                    (!parameter.IsLocked.HasValue || x.IsLocked == parameter.IsLocked) &&
                    (!parameter.IsDeleted.HasValue || x.IsDeleted == parameter.IsDeleted) &&
                    (!parameter.MinCreateDate.HasValue || x.CreateDateTime >= parameter.MinCreateDate) &&
                    (!parameter.MaxCreateDate.HasValue || x.CreateDateTime <= parameter.MaxCreateDate))
                .SortBy(parameter.SortBy, parameter.OrderBy)
                .Skip(parameter.Offset)
                .Take(parameter.Limit)
                .ToArrayAsync(ct);
        }

        public async Task<Guid> CreateAsync(User user, CancellationToken ct)
        {
            var newUser = new User(user.Surname, user.Name, user.BirthDate, user.Gender, user.AvatarUrl);

            var entry = await _usersStorage.AddAsync(newUser, ct);
            await _usersStorage.SaveChangesAsync(ct);

            return entry.Entity.Id;
        }

        public Task UpdateAsync(User oldUser, User user, CancellationToken ct)
        {
            oldUser.Surname = user.Surname;
            oldUser.Name = user.Name;
            oldUser.BirthDate = user.BirthDate;
            oldUser.Gender = user.Gender;
            oldUser.AvatarUrl = user.AvatarUrl;
            oldUser.IsLocked = user.IsLocked;
            oldUser.IsDeleted = user.IsDeleted;

            _usersStorage.Update(oldUser);

            return _usersStorage.SaveChangesAsync(ct);
        }

        public async Task LockAsync(IEnumerable<Guid> ids, CancellationToken ct)
        {
            await _usersStorage.Users
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(x => x.IsLocked = true, ct);

            await _usersStorage.SaveChangesAsync(ct);
        }

        public async Task UnlockAsync(IEnumerable<Guid> ids, CancellationToken ct)
        {
            await _usersStorage.Users
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(x => x.IsLocked = false, ct);

            await _usersStorage.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(IEnumerable<Guid> ids, CancellationToken ct)
        {
            await _usersStorage.Users
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(x => x.IsDeleted = true, ct);

            await _usersStorage.SaveChangesAsync(ct);
        }

        public async Task RestoreAsync(IEnumerable<Guid> ids, CancellationToken ct)
        {
            await _usersStorage.Users
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(x => x.IsDeleted = false, ct);

            await _usersStorage.SaveChangesAsync(ct);
        }
    }
}