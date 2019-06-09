using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Clients.Identities.Models;

namespace Crm.Clients.Identities.Clients
{
    public interface IIdentitiesClient
    {
        Task<List<IdentityType>> GetTypesAsync(CancellationToken ct = default);

        Task<Identity> GetAsync(Guid id, CancellationToken ct = default);

        Task<List<Identity>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct = default);

        Task<List<Identity>> GetPagedListAsync(Guid? userId = default, List<IdentityType> types = default,
            string key = default, bool? isPrimary = default, bool? isVerified = default,
            DateTime? minCreateDate = default, DateTime? maxCreateDate = default, int offset = default,
            int limit = 10, string sortBy = default, string orderBy = default, CancellationToken ct = default);

        Task<Guid> CreateAsync(Identity identity, CancellationToken ct = default);

        Task UpdateAsync(Identity identity, CancellationToken ct = default);

        Task SetPasswordAsync(Guid id, string password, CancellationToken ct = default);

        Task<bool> IsPasswordCorrectAsync(Guid id, string password, CancellationToken ct = default);

        Task VerifyAsync(IEnumerable<Guid> ids, CancellationToken ct = default);

        Task UnverifyAsync(IEnumerable<Guid> ids, CancellationToken ct = default);

        Task SetAsPrimaryAsync(IEnumerable<Guid> ids, CancellationToken ct = default);

        Task ResetAsPrimaryAsync(IEnumerable<Guid> ids, CancellationToken ct = default);
    }
}