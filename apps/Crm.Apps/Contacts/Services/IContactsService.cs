using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Contacts.Models;
using Crm.Apps.Contacts.Parameters;

namespace Crm.Apps.Contacts.Services
{
    public interface IContactsService
    {
        Task<Contact> GetAsync(Guid id, CancellationToken ct);

        Task<List<Contact>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct);

        Task<List<Contact>> GetPagedListAsync(ContactGetPagedListParameter parameter, CancellationToken ct);

        Task<Guid> CreateAsync(Guid userId, Contact contact, CancellationToken ct);

        Task UpdateAsync(Guid userId, Contact oldContact, Contact newContact, CancellationToken ct);

        Task DeleteAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);

        Task RestoreAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);
    }
}