using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Clients.Users.Models;

namespace Crm.Clients.Users.Clients.UserGroupChanges
{
    public interface IUserGroupChangesClient
    {
        Task<List<UserGroupChange>> GetPagedListAsync(Guid? changerUserId = default, Guid? groupId = default,
            DateTime? minCreateDate = default, DateTime? maxCreateDate = default, int offset = 10, int limit = default,
            string sortBy = default, string orderBy = default, CancellationToken ct = default);
    }
}