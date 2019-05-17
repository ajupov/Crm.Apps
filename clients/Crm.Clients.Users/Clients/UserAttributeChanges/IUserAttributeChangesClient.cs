using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Clients.Users.Models;

namespace Crm.Clients.Users.Clients.UserAttributeChanges
{
    public interface IUserAttributeChangesClient
    {
        Task<List<UserAttributeChange>> GetPagedListAsync(Guid? changerUserId = default, Guid? attributeId = default,
            DateTime? minCreateDate = default, DateTime? maxCreateDate = default, int offset = default,
            int limit = 10, string sortBy = default, string orderBy = default, CancellationToken ct = default);
    }
}