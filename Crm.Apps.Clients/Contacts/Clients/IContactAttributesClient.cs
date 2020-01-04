using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Clients.Contacts.Models;

namespace Crm.Apps.Clients.Contacts.Clients
{
    public interface IContactAttributesClient
    {
        Task<List<AttributeType>> GetTypesAsync(CancellationToken ct = default);

        Task<ContactAttribute> GetAsync(Guid id, CancellationToken ct = default);

        Task<List<ContactAttribute>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct = default);

        Task<List<ContactAttribute>> GetPagedListAsync(Guid? accountId = default, List<AttributeType> types = default,
            string key = default, bool? isDeleted = default, DateTime? minCreateDate = default,
            DateTime? maxCreateDate = default, int offset = default, int limit = 10, string sortBy = default,
            string orderBy = default, CancellationToken ct = default);

        Task<Guid> CreateAsync(ContactAttribute attribute, CancellationToken ct = default);

        Task UpdateAsync(ContactAttribute attribute, CancellationToken ct = default);

        Task DeleteAsync(IEnumerable<Guid> ids, CancellationToken ct = default);

        Task RestoreAsync(IEnumerable<Guid> ids, CancellationToken ct = default);
    }
}