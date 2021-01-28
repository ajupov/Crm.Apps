using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Contacts.Models;
using Crm.Apps.Contacts.V1.Requests;
using Crm.Apps.Contacts.V1.Responses;

namespace Crm.Apps.Contacts.Services
{
    public interface IContactsService
    {
        Task<Contact> GetAsync(Guid id, bool isTrackChanges, CancellationToken ct);

        Task<List<Contact>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct);

        Task<ContactGetPagedListResponse> GetPagedListAsync(
            Guid accountId,
            ContactGetPagedListRequest request,
            CancellationToken ct);

        Task<Guid> CreateAsync(Guid userId, Contact contact, CancellationToken ct);

        Task UpdateAsync(Guid userId, Contact oldContact, Contact newContact, CancellationToken ct);

        Task DeleteAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);

        Task RestoreAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);
    }
}
