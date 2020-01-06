using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Clients.Users.Models;
using Crm.Apps.Clients.Users.RequestParameters;
using Crm.Common.All.Types.AttributeType;

namespace Crm.Apps.Clients.Users.Clients
{
    public interface IUserAttributesClient
    {
        Task<Dictionary<string, AttributeType>> GetTypesAsync(CancellationToken ct = default);

        Task<UserAttribute> GetAsync(Guid id, CancellationToken ct = default);

        Task<List<UserAttribute>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct = default);

        Task<List<UserAttribute>> GetPagedListAsync(
            UserAttributeGetPagedListRequestParameter request,
            CancellationToken ct = default);

        Task<Guid> CreateAsync(UserAttribute attribute, CancellationToken ct = default);

        Task UpdateAsync(UserAttribute attribute, CancellationToken ct = default);

        Task DeleteAsync(IEnumerable<Guid> ids, CancellationToken ct = default);

        Task RestoreAsync(IEnumerable<Guid> ids, CancellationToken ct = default);
    }
}