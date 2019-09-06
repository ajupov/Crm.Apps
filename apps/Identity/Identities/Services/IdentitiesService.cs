using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Utils.Guid;
using Crm.Utils.Password;
using Crm.Utils.Sorting;
using Identity.Identities.Models;
using Identity.Identities.Parameters;
using Identity.Identities.Storages;
using Microsoft.EntityFrameworkCore;

namespace Identity.Identities.Services
{
    public class IdentitiesService : IIdentitiesService
    {
        private readonly IdentitiesStorage _identitiesStorage;

        public IdentitiesService(IdentitiesStorage identitiesStorage)
        {
            _identitiesStorage = identitiesStorage;
        }

        public Task<Models.Identity> GetAsync(Guid id, CancellationToken ct)
        {
            return _identitiesStorage.Identities
                .FirstOrDefaultAsync(x => x.Id == id, ct);
        }

        public Task<Models.Identity[]> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct)
        {
            return _identitiesStorage.Identities
                .Where(x => ids.Contains(x.Id))
                .ToArrayAsync(ct);
        }

        public Task<Models.Identity> GetByKeyAndTypesAsync(string key, IdentityType[] types, CancellationToken ct)
        {
            return _identitiesStorage.Identities
                .FirstOrDefaultAsync(x => x.Key == key && types.Contains(x.Type), ct);
        }

        public Task<Models.Identity[]> GetPagedListAsync(
            IdentityGetPagedListParameter parameter,
            CancellationToken ct)
        {
            return _identitiesStorage.Identities
                .Where(x =>
                    (parameter.UserId.IsEmpty() || x.UserId == parameter.UserId) &&
                    (parameter.Types == null || !parameter.Types.Any() || parameter.Types.Contains(x.Type)) &&
                    (!parameter.IsPrimary.HasValue || x.IsPrimary == parameter.IsPrimary) &&
                    (!parameter.IsVerified.HasValue || x.IsVerified == parameter.IsVerified) &&
                    (!parameter.MinCreateDateTime.HasValue || x.CreateDateTime >= parameter.MinCreateDateTime) &&
                    (!parameter.MaxCreateDateTime.HasValue || x.CreateDateTime <= parameter.MaxCreateDateTime))
                .SortBy(parameter.SortBy, parameter.OrderBy)
                .Skip(parameter.Offset)
                .Take(parameter.Limit)
                .ToArrayAsync(ct);
        }

        public async Task<Guid> CreateAsync(Models.Identity identity, CancellationToken ct)
        {
            var newIdentity = new Models.Identity(identity.UserId, identity.Type, identity.Key, identity.IsPrimary);

            var entry = await _identitiesStorage.AddAsync(newIdentity, ct);
            await _identitiesStorage.SaveChangesAsync(ct);

            return entry.Entity.Id;
        }

        public Task UpdateAsync(Models.Identity oldIdentity, Models.Identity identity, CancellationToken ct)
        {
            oldIdentity.IsVerified = oldIdentity.Key != identity.Key;
            oldIdentity.Key = identity.Key;
            oldIdentity.IsPrimary = identity.IsPrimary;

            _identitiesStorage.Update(oldIdentity);

            return _identitiesStorage.SaveChangesAsync(ct);
        }

        public Task SetPasswordAsync(Models.Identity identity, string password, CancellationToken ct)
        {
            identity.PasswordHash = Password.ToPasswordHash(password);

            _identitiesStorage.Update(identity);

            return _identitiesStorage.SaveChangesAsync(ct);
        }

        public async Task VerifyAsync(IEnumerable<Guid> ids, CancellationToken ct)
        {
            await _identitiesStorage.Identities
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(x => x.IsVerified = true, ct);

            await _identitiesStorage.SaveChangesAsync(ct);
        }

        public async Task UnverifyAsync(IEnumerable<Guid> ids, CancellationToken ct)
        {
            await _identitiesStorage.Identities
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(x => x.IsVerified = false, ct);

            await _identitiesStorage.SaveChangesAsync(ct);
        }

        public async Task SetAsPrimaryAsync(IEnumerable<Guid> ids, CancellationToken ct)
        {
            await _identitiesStorage.Identities
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(x => x.IsPrimary = true, ct);

            await _identitiesStorage.SaveChangesAsync(ct);
        }

        public async Task ResetAsPrimaryAsync(IEnumerable<Guid> ids, CancellationToken ct)
        {
            await _identitiesStorage.Identities
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(x => x.IsPrimary = false, ct);

            await _identitiesStorage.SaveChangesAsync(ct);
        }

        public bool IsPasswordCorrect(Models.Identity identity, string password)
        {
            return Password.IsVerifiedPassword(password, identity.PasswordHash);
        }
    }
}