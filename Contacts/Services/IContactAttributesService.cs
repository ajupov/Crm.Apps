using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Contacts.v1.Models;
using Crm.Apps.Contacts.v1.RequestParameters;

namespace Crm.Apps.Contacts.Services
{
    public interface IContactAttributesService
    {
        Task<ContactAttribute> GetAsync(Guid id, CancellationToken ct);

        Task<List<ContactAttribute>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct);

        Task<List<ContactAttribute>> GetPagedListAsync(
            ContactAttributeGetPagedListRequestParameter request,
            CancellationToken ct);

        Task<Guid> CreateAsync(Guid userId, ContactAttribute attribute, CancellationToken ct);

        Task UpdateAsync(
            Guid userId,
            ContactAttribute oldAttribute,
            ContactAttribute newAttribute,
            CancellationToken ct);

        Task DeleteAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);

        Task RestoreAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);
    }
}