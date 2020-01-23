using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.v1.Clients.Contacts.Models;
using Crm.Apps.v1.Clients.Contacts.RequestParameters;
using Crm.Common.All.Types.AttributeType;

namespace Crm.Apps.v1.Clients.Contacts.Clients
{
    public interface IContactAttributesClient
    {
        Task<Dictionary<string, AttributeType>> GetTypesAsync(string accessToken, CancellationToken ct = default);

        Task<ContactAttribute> GetAsync(string accessToken, Guid id, CancellationToken ct = default);

        Task<List<ContactAttribute>> GetListAsync(
            string accessToken,
            IEnumerable<Guid> ids,
            CancellationToken ct = default);

        Task<List<ContactAttribute>> GetPagedListAsync(
            string accessToken,
            ContactAttributeGetPagedListRequestParameter request,
            CancellationToken ct = default);

        Task<Guid> CreateAsync(string accessToken, ContactAttribute attribute, CancellationToken ct = default);

        Task UpdateAsync(string accessToken, ContactAttribute attribute, CancellationToken ct = default);

        Task DeleteAsync(string accessToken, IEnumerable<Guid> ids, CancellationToken ct = default);

        Task RestoreAsync(string accessToken, IEnumerable<Guid> ids, CancellationToken ct = default);
    }
}