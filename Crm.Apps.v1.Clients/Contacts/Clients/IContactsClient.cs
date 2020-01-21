using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.v1.Clients.Contacts.Models;
using Crm.Apps.v1.Clients.Contacts.RequestParameters;

namespace Crm.Apps.v1.Clients.Contacts.Clients
{
    public interface IContactsClient
    {
        Task<Contact> GetAsync(Guid id, CancellationToken ct = default);

        Task<List<Contact>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct = default);

        Task<List<Contact>> GetPagedListAsync(
            ContactGetPagedListRequestParameter request,
            CancellationToken ct = default);

        Task<Guid> CreateAsync(Contact contact, CancellationToken ct = default);

        Task UpdateAsync(Contact contact, CancellationToken ct = default);

        Task DeleteAsync(IEnumerable<Guid> ids, CancellationToken ct = default);

        Task RestoreAsync(IEnumerable<Guid> ids, CancellationToken ct = default);
    }
}