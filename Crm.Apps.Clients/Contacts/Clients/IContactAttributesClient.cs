using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Clients.Contacts.Models;
using Crm.Apps.Clients.Contacts.RequestParameters;
using Crm.Common.All.Types.AttributeType;

namespace Crm.Apps.Clients.Contacts.Clients
{
    public interface IContactAttributesClient
    {
        Task<Dictionary<string, AttributeType>> GetTypesAsync(CancellationToken ct = default);

        Task<ContactAttribute> GetAsync(Guid id, CancellationToken ct = default);

        Task<List<ContactAttribute>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct = default);

        Task<List<ContactAttribute>> GetPagedListAsync(
            ContactAttributeGetPagedListRequestParameter request,
            CancellationToken ct = default);

        Task<Guid> CreateAsync(ContactAttribute attribute, CancellationToken ct = default);

        Task UpdateAsync(ContactAttribute attribute, CancellationToken ct = default);

        Task DeleteAsync(IEnumerable<Guid> ids, CancellationToken ct = default);

        Task RestoreAsync(IEnumerable<Guid> ids, CancellationToken ct = default);
    }
}