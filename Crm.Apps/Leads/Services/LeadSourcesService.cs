using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Utils.All.Guid;
using Ajupov.Utils.All.Sorting;
using Ajupov.Utils.All.String;
using Crm.Apps.Leads.Helpers;
using Crm.Apps.Leads.Models;
using Crm.Apps.Leads.RequestParameters;
using Crm.Apps.Leads.Storages;
using Microsoft.EntityFrameworkCore;

namespace Crm.Apps.Leads.Services
{
    public class LeadSourcesService : ILeadSourcesService
    {
        private readonly LeadsStorage _storage;

        public LeadSourcesService(LeadsStorage storage)
        {
            _storage = storage;
        }

        public Task<LeadSource> GetAsync(Guid id, CancellationToken ct)
        {
            return _storage.LeadSources
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id, ct);
        }

        public Task<List<LeadSource>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct)
        {
            return _storage.LeadSources
                .AsNoTracking()
                .Where(x => ids.Contains(x.Id)).ToListAsync(ct);
        }

        public Task<List<LeadSource>> GetPagedListAsync(
            LeadSourceGetPagedListRequestParameter request,
            CancellationToken ct)
        {
            return _storage.LeadSources
                .AsNoTracking()
                .Where(x =>
                    (request.AccountId.IsEmpty() || x.AccountId == request.AccountId) &&
                    (request.Name.IsEmpty() || EF.Functions.Like(x.Name, $"{request.Name}%")) &&
                    (!request.IsDeleted.HasValue || x.IsDeleted == request.IsDeleted) &&
                    (!request.MinCreateDate.HasValue || x.CreateDateTime >= request.MinCreateDate) &&
                    (!request.MaxCreateDate.HasValue || x.CreateDateTime <= request.MaxCreateDate) &&
                    (!request.MinModifyDate.HasValue || x.ModifyDateTime >= request.MinModifyDate) &&
                    (!request.MaxModifyDate.HasValue || x.ModifyDateTime <= request.MaxModifyDate))
                .SortBy(request.SortBy, request.OrderBy)
                .Skip(request.Offset)
                .Take(request.Limit)
                .ToListAsync(ct);
        }

        public async Task<Guid> CreateAsync(Guid userId, LeadSource source, CancellationToken ct)
        {
            var newSource = new LeadSource();
            var change = newSource.WithCreateLog(userId, x =>
            {
                x.Id = Guid.NewGuid();
                x.AccountId = source.AccountId;
                x.Name = source.Name;
                x.IsDeleted = source.IsDeleted;
                x.CreateDateTime = DateTime.UtcNow;
            });

            var entry = await _storage.AddAsync(newSource, ct);
            await _storage.AddAsync(change, ct);
            await _storage.SaveChangesAsync(ct);

            return entry.Entity.Id;
        }

        public async Task UpdateAsync(Guid userId, LeadSource oldSource, LeadSource newSource,
            CancellationToken ct)
        {
            var change = oldSource.WithUpdateLog(userId, x =>
            {
                x.Name = newSource.Name;
                x.IsDeleted = newSource.IsDeleted;
            });

            _storage.Update(oldSource);
            await _storage.AddAsync(change, ct);
            await _storage.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<LeadSourceChange>();

            await _storage.LeadSources
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(u => changes.Add(u.WithUpdateLog(userId, x => x.IsDeleted = true)), ct);

            await _storage.AddRangeAsync(changes, ct);
            await _storage.SaveChangesAsync(ct);
        }

        public async Task RestoreAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<LeadSourceChange>();

            await _storage.LeadSources
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(u => changes.Add(u.WithUpdateLog(userId, x => x.IsDeleted = false)), ct);

            await _storage.AddRangeAsync(changes, ct);
            await _storage.SaveChangesAsync(ct);
        }
    }
}