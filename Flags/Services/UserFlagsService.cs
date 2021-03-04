using System;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Flags.Models;
using Crm.Apps.Flags.Storages;
using Microsoft.EntityFrameworkCore;

namespace Crm.Apps.Flags.Services
{
    public class UserFlagsService : IUserFlagsService
    {
        private readonly FlagsStorage _storage;

        public UserFlagsService(FlagsStorage storage)
        {
            _storage = storage;
        }

        public Task<bool> IsSetAsync(Guid userId, UserFlagType type, CancellationToken ct)
        {
            return _storage.UserFlags
                .AsNoTracking()
                .AnyAsync(x => x.UserId == userId && x.Type == type, cancellationToken: ct);
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
