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
    public class UsersService : IUsersService
    {
        private readonly UsersStorage _storage;

        public UsersService(UsersStorage storage)
        {
            _storage = storage;
        }

        public Task<User> GetAsync(Guid id, CancellationToken ct)
        {
            return _storage.Users
                .AsNoTracking()
                .Include(x => x.AttributeLinks)
                .Include(x => x.Roles)
                .Include(x => x.GroupLinks)
                .Include(x => x.Settings)
                .FirstOrDefaultAsync(x => x.Id == id, ct);
        }

        public Task<List<User>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct)
        {
            return _storage.Users
                .AsNoTracking()
                .Where(x => ids.Contains(x.Id))
                .ToListAsync(ct);
        }

        public async Task<List<User>> GetPagedListAsync(UserGetPagedListRequestParameter request, CancellationToken ct)
        {
            var temp = await _storage.Users
                .AsNoTracking()
                .Include(x => x.AttributeLinks)
                .Include(x => x.Roles)
                .Include(x => x.GroupLinks)
                .Include(x => x.Settings)
                .Where(x =>
                    (request.AccountId.IsEmpty() || x.AccountId == request.AccountId) &&
                    (request.Surname.IsEmpty() || EF.Functions.Like(x.Surname, $"{request.Surname}%")) &&
                    (request.Name.IsEmpty() || EF.Functions.Like(x.Name, $"{request.Name}%")) &&
                    (request.Patronymic.IsEmpty() || EF.Functions.Like(x.Patronymic, $"{request.Patronymic}%")) &&
                    (!request.MinBirthDate.HasValue || x.BirthDate >= request.MinBirthDate) &&
                    (!request.MaxBirthDate.HasValue || x.BirthDate <= request.MaxBirthDate) &&
                    (!request.Gender.HasValue || x.Gender == request.Gender) &&
                    (!request.IsLocked.HasValue || x.IsLocked == request.IsLocked) &&
                    (!request.IsDeleted.HasValue || x.IsDeleted == request.IsDeleted) &&
                    (!request.MinCreateDate.HasValue || x.CreateDateTime >= request.MinCreateDate) &&
                    (!request.MaxCreateDate.HasValue || x.CreateDateTime <= request.MaxCreateDate) &&
                    (!request.MinModifyDate.HasValue || x.ModifyDateTime >= request.MinModifyDate) &&
                    (!request.MaxModifyDate.HasValue || x.ModifyDateTime <= request.MaxModifyDate))
                .SortBy(request.SortBy, request.OrderBy)
                .ToListAsync(ct);

            return temp.Where(x => x.FilterByAdditional(request)).Skip(request.Offset).Take(request.Limit)
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
                x.Roles = user.Roles;
                x.GroupLinks = user.GroupLinks;
                x.Settings = user.Settings;
            });

            var entry = await _storage.AddAsync(newUser, ct);
            await _storage.AddAsync(change, ct);
            await _storage.SaveChangesAsync(ct);

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
                x.Roles = newUser.Roles;
                x.GroupLinks = newUser.GroupLinks;
                x.Settings = newUser.Settings;
            });

            _storage.Update(oldUser);
            await _storage.AddAsync(change, ct);
            await _storage.SaveChangesAsync(ct);
        }

        public async Task LockAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<UserChange>();

            await _storage.Users
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(u => changes.Add(u.UpdateWithLog(userId, x => x.IsLocked = true)), ct);

            await _storage.AddRangeAsync(changes, ct);
            await _storage.SaveChangesAsync(ct);
        }

        public async Task UnlockAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<UserChange>();

            await _storage.Users
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(u => changes.Add(u.UpdateWithLog(userId, x => x.IsLocked = false)), ct);

            await _storage.AddRangeAsync(changes, ct);
            await _storage.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<UserChange>();

            await _storage.Users
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(u => changes.Add(u.UpdateWithLog(userId, x => x.IsDeleted = true)), ct);

            await _storage.AddRangeAsync(changes, ct);
            await _storage.SaveChangesAsync(ct);
        }

        public async Task RestoreAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<UserChange>();

            await _storage.Users
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(u => changes.Add(u.UpdateWithLog(userId, x => x.IsDeleted = false)), ct);

            await _storage.AddRangeAsync(changes, ct);
            await _storage.SaveChangesAsync(ct);
        }
    }
}