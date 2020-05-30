using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Contacts.Models;
using Crm.Apps.Contacts.V1.Requests;
using Crm.Apps.Contacts.V1.Responses;

namespace Crm.Apps.Contacts.Services
{
    public interface IContactAttributesService
    {
        Task<ContactAttribute> GetAsync(Guid id, CancellationToken ct);

        Task<List<ContactAttribute>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct);

        Task<ContactAttributeGetPagedListResponse> GetPagedListAsync(
            Guid accountId,
            ContactAttributeGetPagedListRequest request,
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
