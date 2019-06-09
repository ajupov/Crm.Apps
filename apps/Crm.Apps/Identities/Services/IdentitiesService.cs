using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Identities.Helpers;
using Crm.Apps.Identities.Models;
using Crm.Apps.Identities.Parameters;
using Crm.Apps.Identities.Storages;
using Crm.Utils.Password;
using Microsoft.EntityFrameworkCore;

namespace Crm.Apps.Identities.Services
{
    public class IdentitiesService : IIdentitiesService
    {
        private readonly IdentitiesStorage _storage;

        public IdentitiesService(IdentitiesStorage storage)
        {
            _storage = storage;
        }

        public async Task<Identity> GetAsync(Guid id, CancellationToken ct)
        {
            var result = await _storage.Identities.FirstOrDefaultAsync(x => x.Id == id, ct).ConfigureAwait(false);
            if (result != null)
            {
                result.PasswordHash = string.Empty;
            }

            return result;
        }

        public async Task<List<Identity>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct)
        {
            var result = await _storage.Identities.Where(x => ids.Contains(x.Id)).ToListAsync(ct).ConfigureAwait(false);

            result.ForEach(x => x.PasswordHash = string.Empty);

            return result;
        }

        public async Task<List<Identity>> GetPagedListAsync(IdentityGetPagedListParameter parameter,
            CancellationToken ct)
        {
            var result = await _storage.Identities.Where(x =>
                    (!parameter.UserId.HasValue || x.UserId == parameter.UserId) &&
                    (parameter.Types == null || !parameter.Types.Any() || parameter.Types.Contains(x.Type)) &&
                    (!parameter.IsPrimary.HasValue || x.IsPrimary == parameter.IsPrimary) &&
                    (!parameter.IsVerified.HasValue || x.IsVerified == parameter.IsVerified) &&
                    (!parameter.MinCreateDate.HasValue || x.CreateDateTime >= parameter.MinCreateDate) &&
                    (!parameter.MaxCreateDate.HasValue || x.CreateDateTime <= parameter.MaxCreateDate))
                .Sort(parameter.SortBy, parameter.OrderBy)
                .Skip(parameter.Offset)
                .Take(parameter.Limit)
                .ToListAsync(ct)
                .ConfigureAwait(false);

            result.ForEach(x => x.PasswordHash = string.Empty);

            return result;
        }

        public async Task<Guid> CreateAsync(Identity identity, CancellationToken ct)
        {
            var newIdentity = new Identity
            {
                Id = Guid.NewGuid(),
                UserId = identity.UserId,
                Type = identity.Type,
                Key = identity.Key,
                PasswordHash = string.Empty,
                IsPrimary = identity.IsPrimary,
                IsVerified = identity.IsVerified,
                CreateDateTime = DateTime.UtcNow
            };

            var entry = await _storage.AddAsync(newIdentity, ct).ConfigureAwait(false);
            await _storage.SaveChangesAsync(ct).ConfigureAwait(false);

            return entry.Entity.Id;
        }

        public Task UpdateAsync(Identity oldIdentity, Identity newIdentity, CancellationToken ct)
        {
            oldIdentity.Type = newIdentity.Type;
            oldIdentity.Key = newIdentity.Key;
            oldIdentity.IsPrimary = newIdentity.IsPrimary;
            oldIdentity.IsVerified = newIdentity.IsVerified;

            _storage.Update(oldIdentity);
            return _storage.SaveChangesAsync(ct);
        }

        public Task SetPasswordAsync(Identity identity, string password, CancellationToken ct)
        {
            identity.PasswordHash = Password.ToPasswordHash(password);

            _storage.Update(identity);
            return _storage.SaveChangesAsync(ct);
        }

        public Task<bool> IsPasswordCorrectAsync(Identity identity, string password, CancellationToken ct)
        {
            return Task.FromResult(Password.IsVerifiedPassword(password, identity.PasswordHash));
        }

        public async Task VerifyAsync(IEnumerable<Guid> ids, CancellationToken ct)
        {
            await _storage.Identities.Where(x => ids.Contains(x.Id)).ForEachAsync(x => x.IsVerified = true, ct)
                .ConfigureAwait(false);

            await _storage.SaveChangesAsync(ct).ConfigureAwait(false);
        }

        public async Task UnverifyAsync(IEnumerable<Guid> ids, CancellationToken ct)
        {
            await _storage.Identities.Where(x => ids.Contains(x.Id)).ForEachAsync(x => x.IsVerified = false, ct)
                .ConfigureAwait(false);

            await _storage.SaveChangesAsync(ct).ConfigureAwait(false);
        }

        public async Task SetAsPrimaryAsync(IEnumerable<Guid> ids, CancellationToken ct)
        {
            await _storage.Identities.Where(x => ids.Contains(x.Id)).ForEachAsync(x => x.IsPrimary = true, ct)
                .ConfigureAwait(false);

            await _storage.SaveChangesAsync(ct).ConfigureAwait(false);
        }

        public async Task ResetAsPrimaryAsync(IEnumerable<Guid> ids, CancellationToken ct)
        {
            await _storage.Identities.Where(x => ids.Contains(x.Id)).ForEachAsync(x => x.IsPrimary = false, ct)
                .ConfigureAwait(false);

            await _storage.SaveChangesAsync(ct).ConfigureAwait(false);
        }
    }
}