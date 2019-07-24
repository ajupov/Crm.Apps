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

        Task<Identity[]> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct);

        Task<Identity[]> GetPagedListAsync(IdentityGetPagedListParameter parameter, CancellationToken ct);

        Task<Guid> CreateAsync(Guid userId, Identity identity, CancellationToken ct);

        Task UpdateAsync(Guid userId, Identity oldIdentity, Identity newIdentity, CancellationToken ct);

        Task SetPasswordAsync(Guid userId, Identity identity, string password, CancellationToken ct);

        Task<bool> IsPasswordCorrectAsync(Guid userId, Identity identity, string password, CancellationToken ct);

        Task VerifyAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);

        Task UnverifyAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);

        Task SetAsPrimaryAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);

        Task ResetAsPrimaryAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);
    }
}