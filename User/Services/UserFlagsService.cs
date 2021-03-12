using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Utils.All.Enums;
using Crm.Apps.User.Models;
using Crm.Apps.User.Storages;
using Microsoft.EntityFrameworkCore;

namespace Crm.Apps.User.Services
{
    public class UserFlagsService : IUserFlagsService
    {
        private readonly UserStorage _storage;

        public UserFlagsService(UserStorage storage)
        {
            _storage = storage;
        }

        public Task<bool> IsSetAsync(Guid userId, UserFlagType type, CancellationToken ct)
        {
            return _storage.UserFlags
                .AsNoTracking()
                .AnyAsync(x => x.UserId == userId && x.Type == type, cancellationToken: ct);
        }

        public async Task<IEnumerable<UserFlagType>> GetNotSetListAsync(Guid userId, CancellationToken ct)
        {
            {
                var allFlags = EnumsExtensions.GetValues<UserFlagType>();

                var setFlags = await _storage.UserFlags
                    .AsNoTracking()
                    .Where(x => x.UserId == userId)
                    .Select(x => x.Type)
                    .ToListAsync(ct);

                return allFlags
                    .Except(setFlags)
                    .ToList();
            }
        }

        public async Task SetAsync(Guid userId, UserFlagType type, CancellationToken ct)
        {
            var flag = new UserFlag
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Type = type,
                SetDateTime = DateTime.UtcNow
            };

            await _storage.AddAsync(flag, ct);
            await _storage.SaveChangesAsync(ct);
        }
    }
}
