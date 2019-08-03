using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Identity.Identities.Models;
using Identity.Identities.Parameters;

namespace Identity.Identities.Services
{
    public interface IIdentitiesService
    {
        Task<Models.Identity> GetAsync(Guid id, CancellationToken ct);

        Task<Models.Identity[]> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct);

        Task<Models.Identity> GetByKeyAndTypesAsync(string key, IdentityType[] types, CancellationToken ct);

        Task<Models.Identity[]> GetPagedListAsync(IdentityGetPagedListParameter parameter, CancellationToken ct);

        Task<Guid> CreateAsync(Models.Identity identity, CancellationToken ct);

        Task UpdateAsync(Models.Identity oldIdentity, Models.Identity identity, CancellationToken ct);

        Task SetPasswordAsync(Models.Identity identity, string password, CancellationToken ct);

        Task VerifyAsync(IEnumerable<Guid> ids, CancellationToken ct);

        Task UnverifyAsync(IEnumerable<Guid> ids, CancellationToken ct);

        Task SetAsPrimaryAsync(IEnumerable<Guid> ids, CancellationToken ct);

        Task ResetAsPrimaryAsync(IEnumerable<Guid> ids, CancellationToken ct);

        bool IsPasswordCorrect(Models.Identity identity, string password);
    }
}