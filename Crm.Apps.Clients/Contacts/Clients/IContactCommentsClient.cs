using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Clients.Contacts.Models;

namespace Crm.Apps.Clients.Contacts.Clients
{
    public interface IContactCommentsClient
    {
        Task<List<ContactComment>> GetPagedListAsync(Guid? contactId = default, Guid? commentatorUserId = default,
            string value = default, DateTime? minCreateDate = default, DateTime? maxCreateDate = default,
            int offset = default, int limit = 10, string sortBy = default, string orderBy = default,
            CancellationToken ct = default);

        Task CreateAsync(ContactComment comment, CancellationToken ct = default);
    }
}