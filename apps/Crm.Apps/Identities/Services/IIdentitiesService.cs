using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Identities.Models;
using Crm.Apps.Identities.Parameters;

namespace Crm.Apps.Identities.Services
{
    public interface IIdentitiesService
    {
        Task<Identity> GetAsync(Guid id, CancellationToken ct);

        Task<List<Identity>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct);

        Task<List<Identity>> GetPagedListAsync(IdentityGetPagedListParameter parameter, CancellationToken ct);

        Task<Guid> CreateAsync(Identity identity, CancellationToken ct);

        Task UpdateAsync(Identity oldIdentity, Identity newIdentity, CancellationToken ct);

        Task SetPasswordAsync(Identity identity, string password, CancellationToken ct);

        Task<bool> IsPasswordCorrectAsync(Identity identity, string password, CancellationToken ct);

        Task VerifyAsync(IEnumerable<Guid> ids, CancellationToken ct);

        Task UnverifyAsync(IEnumerable<Guid> ids, CancellationToken ct);

        Task SetAsPrimaryAsync(IEnumerable<Guid> ids, CancellationToken ct);

        Task ResetAsPrimaryAsync(IEnumerable<Guid> ids, CancellationToken ct);
    }
}