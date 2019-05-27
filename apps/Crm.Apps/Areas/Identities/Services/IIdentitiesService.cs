using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Identities.Models;
using Crm.Apps.Areas.Identities.Parameters;

namespace Crm.Apps.Areas.Identities.Services
{
    public interface IIdentitiesService
    {
        Task<Identity> GetAsync(Guid id, CancellationToken ct);

        Task<List<Identity>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct);

        Task<List<Identity>> GetPagedListAsync(IdentityGetPagedListParameter parameter, CancellationToken ct);

        Task<Guid> CreateAsync(Guid userId, Identity identity, CancellationToken ct);

        Task UpdateAsync(Guid userId, Identity oldIdentity, Identity newIdentity, CancellationToken ct);

        Task VerifyAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);

        Task UnverifyAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);

        Task SetAsPrimaryAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);

        Task ResetAsPrimaryAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);
    }
}