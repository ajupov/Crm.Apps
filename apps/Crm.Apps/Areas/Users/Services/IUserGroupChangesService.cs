using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Users.Models;

namespace Crm.Apps.Areas.Users.Services
{
    public interface IUserGroupChangesService
    {
        Task<List<UserGroupChange>> GetPagedListAsync(Guid? changerUserId, Guid? groupId, DateTime? minCreateDate,
            DateTime? maxCreateDate, int offset, int limit, string sortBy, string orderBy, CancellationToken ct);
    }
}