using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Contacts.Models;
using Crm.Apps.Areas.Contacts.RequestParameters;

namespace Crm.Apps.Areas.Contacts.Services
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