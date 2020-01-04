using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Clients.Contacts.Models;

namespace Crm.Clients.Contacts.Clients
{
    public interface IContactChangesClient
    {
        Task<List<ContactChange>> GetPagedListAsync(Guid? changerUserId = default, Guid? contactId = default,
            DateTime? minCreateDate = default, DateTime? maxCreateDate = default, int offset = default,
            int limit = 10, string sortBy = default, string orderBy = default, CancellationToken ct = default);
    }
}