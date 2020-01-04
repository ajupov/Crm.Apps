using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Clients.Users.Models;

namespace Crm.Apps.Clients.Users.Clients
{
    public interface IUserChangesClient
    {
        Task<List<UserChange>> GetPagedListAsync(Guid? changerUserId = default, Guid? userId = default,
            DateTime? minCreateDate = default, DateTime? maxCreateDate = default, int offset = default,
            int limit = 10, string sortBy = default, string orderBy = default, CancellationToken ct = default);
    }
}