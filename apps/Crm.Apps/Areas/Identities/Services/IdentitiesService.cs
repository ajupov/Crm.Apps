using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Identities.Helpers;
using Crm.Apps.Areas.Identities.Models;
using Crm.Apps.Areas.Identities.Parameters;
using Crm.Apps.Areas.Identities.Storages;
using Microsoft.EntityFrameworkCore;

namespace Crm.Apps.Areas.Identities.Services
{
    public class IdentitiesService : IIdentitiesService
    {
        private readonly IdentitiesStorage _storage;

        public IdentitiesService(IdentitiesStorage storage)
        {
            _storage = storage;
        }

        public Task<Identity> GetAsync(Guid id, CancellationToken ct)
        {
            return _storage.Identities.FirstOrDefaultAsync(x => x.Id == id, ct);
        }

        public Task<List<Identity>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct)
        {
            return _storage.Identities.Where(x => ids.Contains(x.Id)).ToListAsync(ct);
        }

        public Task<List<Identity>> GetPagedListAsync(IdentityGetPagedListParameter parameter, CancellationToken ct)
        {
            return _storage.Identities.Where(x =>
                    (!parameter.UserId.HasValue || x.UserId == parameter.UserId) &&
                    (parameter.Types == null || !parameter.Types.Any() || parameter.Types.Contains(x.Type)) &&
                    (!parameter.IsPrimary.HasValue || x.IsPrimary == parameter.IsPrimary) &&
                    (!parameter.IsVerified.HasValue || x.IsVerified == parameter.IsVerified) &&
                    (!parameter.MinCreateDate.HasValue || x.CreateDateTime >= parameter.MinCreateDate) &&
                    (!parameter.MaxCreateDate.HasValue || x.CreateDateTime <= parameter.MaxCreateDate))
                .Sort(parameter.SortBy, parameter.OrderBy)
                .Skip(parameter.Offset)
                .Take(parameter.Limit)
                .ToListAsync(ct);
        }

        public async Task<Guid> CreateAsync(Guid userId, Identity identity, CancellationToken ct)
        {
            var newIdentity = new Identity
            {
                Id = Guid.NewGuid(),
                UserId = identity.UserId,
                Type = identity.Type,
                Key = identity.Key,
                PasswordHash = identity.PasswordHash,
                IsPrimary = identity.IsPrimary,
                IsVerified = identity.IsVerified,
                CreateDateTime = DateTime.UtcNow
            };

            var entry = await _storage.AddAsync(newIdentity, ct).ConfigureAwait(false);
            await _storage.SaveChangesAsync(ct).ConfigureAwait(false);

            return entry.Entity.Id;
        }

        public async Task UpdateAsync(Guid userId, Identity oldIdentity, Identity newIdentity, CancellationToken ct)
        {
            oldIdentity.Type = newIdentity.Type;
            oldIdentity.Key = newIdentity.Key;
            oldIdentity.PasswordHash = newIdentity.PasswordHash;
            oldIdentity.IsPrimary = newIdentity.IsPrimary;
            oldIdentity.IsVerified = newIdentity.IsVerified;

            _storage.Update(oldIdentity);
            await _storage.SaveChangesAsync(ct).ConfigureAwait(false);
        }

        public async Task VerifyAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            await _storage.Identities.Where(x => ids.Contains(x.Id)).ForEachAsync(x => x.IsVerified = true, ct)
                .ConfigureAwait(false);

            await _storage.SaveChangesAsync(ct).ConfigureAwait(false);
        }

        public async Task UnverifyAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            await _storage.Identities.Where(x => ids.Contains(x.Id)).ForEachAsync(x => x.IsVerified = false, ct)
                .ConfigureAwait(false);

            await _storage.SaveChangesAsync(ct).ConfigureAwait(false);
        }

        public async Task SetAsPrimaryAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            await _storage.Identities.Where(x => ids.Contains(x.Id)).ForEachAsync(x => x.IsPrimary = true, ct)
                .ConfigureAwait(false);

            await _storage.SaveChangesAsync(ct).ConfigureAwait(false);
        }

        public async Task ResetAsPrimaryAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            await _storage.Identities.Where(x => ids.Contains(x.Id)).ForEachAsync(x => x.IsPrimary = false, ct)
                .ConfigureAwait(false);

            await _storage.SaveChangesAsync(ct).ConfigureAwait(false);
        }
    }
}