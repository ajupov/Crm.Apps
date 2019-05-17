using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Users.Helpers;
using Crm.Apps.Areas.Users.Models;
using Crm.Apps.Areas.Users.Storages;
using Microsoft.EntityFrameworkCore;

namespace Crm.Apps.Areas.Users.Services
{
    public class UserChangesService : IUserChangesService
    {
        private readonly UsersStorage _storage;

        public UserChangesService(UsersStorage storage)
        {
            _storage = storage;
        }

        public Task<List<UserChange>> GetPagedListAsync(Guid? changerUserId, Guid? userId,
            DateTime? minCreateDate, DateTime? maxCreateDate, int offset, int limit, string sortBy, string orderBy,
            CancellationToken ct)
        {
            return _storage.UserChanges.Where(x =>
                    (!changerUserId.HasValue || x.ChangerUserId == changerUserId) &&
                    (!userId.HasValue || x.UserId == userId) &&
                    (!minCreateDate.HasValue || x.CreateDateTime >= minCreateDate) &&
                    (!maxCreateDate.HasValue || x.CreateDateTime <= maxCreateDate))
                .Sort(sortBy, orderBy)
                .Skip(offset)
                .Take(limit)
                .ToListAsync(ct);
        }
    }
}