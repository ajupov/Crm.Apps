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
    public class UserAttributeChangesService : IUserAttributeChangesService
    {
        private readonly UsersStorage _storage;

        public UserAttributeChangesService(UsersStorage storage)
        {
            _storage = storage;
        }

        public Task<List<UserAttributeChange>> GetPagedListAsync(Guid? changerUserId, Guid? attributeId,
            DateTime? minCreateDate, DateTime? maxCreateDate, int offset, int limit, string sortBy, string orderBy,
            CancellationToken ct)
        {
            return _storage.UserAttributeChanges.Where(x =>
                    (!changerUserId.HasValue || x.ChangerUserId == changerUserId) &&
                    (!attributeId.HasValue || x.AttributeId == attributeId) &&
                    (!minCreateDate.HasValue || x.CreateDateTime >= minCreateDate) &&
                    (!maxCreateDate.HasValue || x.CreateDateTime <= maxCreateDate))
                .Sort(sortBy, orderBy)
                .Skip(offset)
                .Take(limit)
                .ToListAsync(ct);
        }
    }
}