using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Identities.Helpers;
using Crm.Apps.Identities.Models;
using Crm.Apps.Identities.Parameters;
using Crm.Apps.Identities.Storages;
using Crm.Utils.Guid;
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

        public Task<Identity> GetAsync(Guid id, CancellationToken ct)
        {
            return _storage.Identities.FirstOrDefaultAsync(x => x.Id == id, ct);
        }

        public async Task<List<Identity>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct)
        {
            var result = await _storage.Identities.Where(x => ids.Contains(x.Id)).ToListAsync(ct);

            result.ForEach(x => x.PasswordHash = string.Empty);

            return result;
        }

        public async Task<List<Identity>> GetPagedListAsync(IdentityGetPagedListParameter parameter,
            CancellationToken ct)
        {
            var result = await _storage.Identities.Where(x =>
                    (parameter.UserId.IsEmpty() || x.UserId == parameter.UserId) &&
                    (parameter.Types == null || !parameter.Types.Any() || parameter.Types.Contains(x.Type)) &&
                    (!parameter.IsPrimary.HasValue || x.IsPrimary == parameter.IsPrimary) &&
                    (!parameter.IsVerified.HasValue || x.IsVerified == parameter.IsVerified) &&
                    (!parameter.MinCreateDate.HasValue || x.CreateDateTime >= parameter.MinCreateDate) &&
                    (!parameter.MaxCreateDate.HasValue || x.CreateDateTime <= parameter.MaxCreateDate))
                .Sort(parameter.SortBy, parameter.OrderBy)
                .Skip(parameter.Offset)
                .Take(parameter.Limit)
                .ToListAsync(ct)
                ;

            result.ForEach(x => x.PasswordHash = string.Empty);

            return result;
        }

        public async Task<Guid> CreateAsync(Guid userId, Identity identity, CancellationToken ct)
        {
            var newIdentity = new Identity();
            var change = newIdentity.WithCreateLog(userId, x =>
            {
                x.Id = Guid.NewGuid();
                x.UserId = identity.UserId;
                x.Type = identity.Type;
                x.Key = identity.Key;
                x.PasswordHash = string.Empty;
                x.IsPrimary = identity.IsPrimary;
                x.IsVerified = identity.IsVerified;
                x.CreateDateTime = DateTime.UtcNow;
            });

            var entry = await _storage.AddAsync(newIdentity, ct);
            await _storage.AddAsync(change, ct);
            await _storage.SaveChangesAsync(ct);

            return entry.Entity.Id;
        }

        public async Task UpdateAsync(Guid userId, Identity oldIdentity, Identity newIdentity, CancellationToken ct)
        {
            var change = oldIdentity.WithUpdateLog(userId, x =>
            {
                x.Type = newIdentity.Type;
                x.Key = newIdentity.Key;
                x.IsPrimary = newIdentity.IsPrimary;
                x.IsVerified = newIdentity.IsVerified;
            });

            _storage.Update(oldIdentity);
            await _storage.AddAsync(change, ct);
            await _storage.SaveChangesAsync(ct);
        }

        public async Task SetPasswordAsync(Guid userId, Identity identity, string password, CancellationToken ct)
        {
            var change = identity.WithUpdateLog(userId, x => { x.PasswordHash = Password.ToPasswordHash(password); });

            _storage.Update(identity);
            await _storage.AddAsync(change, ct);
            await _storage.SaveChangesAsync(ct);
        }

        public Task<bool> IsPasswordCorrectAsync(Guid userId, Identity identity, string password, CancellationToken ct)
        {
            if (ct.IsCancellationRequested)
            {
                throw new TaskCanceledException();
            }

            return Task.FromResult(Password.IsVerifiedPassword(password, identity.PasswordHash));
        }

        public async Task VerifyAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<IdentityChange>();

            await _storage.Identities.Where(x => ids.Contains(x.Id))
                .ForEachAsync(a => changes.Add(a.WithUpdateLog(userId, x => x.IsVerified = true)), ct)
                ;

            await _storage.AddRangeAsync(changes, ct);
            await _storage.SaveChangesAsync(ct);
        }

        public async Task UnverifyAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<IdentityChange>();

            await _storage.Identities.Where(x => ids.Contains(x.Id))
                .ForEachAsync(a => changes.Add(a.WithUpdateLog(userId, x => x.IsVerified = false)), ct)
                ;

            await _storage.AddRangeAsync(changes, ct);
            await _storage.SaveChangesAsync(ct);
        }

        public async Task SetAsPrimaryAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<IdentityChange>();

            await _storage.Identities.Where(x => ids.Contains(x.Id))
                .ForEachAsync(a => changes.Add(a.WithUpdateLog(userId, x => x.IsPrimary = true)), ct)
                ;

            await _storage.AddRangeAsync(changes, ct);
            await _storage.SaveChangesAsync(ct);
        }

        public async Task ResetAsPrimaryAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<IdentityChange>();

            await _storage.Identities.Where(x => ids.Contains(x.Id))
                .ForEachAsync(a => changes.Add(a.WithUpdateLog(userId, x => x.IsPrimary = false)), ct)
                ;

            await _storage.AddRangeAsync(changes, ct);
            await _storage.SaveChangesAsync(ct);
        }
    }
}