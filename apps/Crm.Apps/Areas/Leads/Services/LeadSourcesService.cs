using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Leads.Helpers;
using Crm.Apps.Areas.Leads.Models;
using Crm.Apps.Areas.Leads.Parameters;
using Crm.Apps.Areas.Leads.Storages;
using Microsoft.EntityFrameworkCore;

namespace Crm.Apps.Areas.Leads.Services
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
            return _storage.LeadSources.FirstOrDefaultAsync(x => x.Id == id, ct);
        }

        public Task<List<LeadSource>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct)
        {
            return _storage.LeadSources.Where(x => ids.Contains(x.Id)).ToListAsync(ct);
        }

        public Task<List<LeadSource>> GetPagedListAsync(LeadSourceGetPagedListParameter parameter,
            CancellationToken ct)
        {
            return _storage.LeadSources.Where(x =>
                    (parameter.AccountId.IsEmpty() || x.AccountId == parameter.AccountId) &&
                    (parameter.Name.IsEmpty() || EF.Functions.Like(x.Name, $"{parameter.Name}%")) &&
                    (!parameter.IsDeleted.HasValue || x.IsDeleted == parameter.IsDeleted) &&
                    (!parameter.MinCreateDate.HasValue || x.CreateDateTime >= parameter.MinCreateDate) &&
                    (!parameter.MaxCreateDate.HasValue || x.CreateDateTime <= parameter.MaxCreateDate))
                .Sort(parameter.SortBy, parameter.OrderBy)
                .Skip(parameter.Offset)
                .Take(parameter.Limit)
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

            await _storage.LeadSources.Where(x => ids.Contains(x.Id))
                .ForEachAsync(u => changes.Add(u.WithUpdateLog(userId, x => x.IsDeleted = true)), ct)
                ;

            await _storage.AddRangeAsync(changes, ct);
            await _storage.SaveChangesAsync(ct);
        }

        public async Task RestoreAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<LeadSourceChange>();

            await _storage.LeadSources.Where(x => ids.Contains(x.Id))
                .ForEachAsync(u => changes.Add(u.WithUpdateLog(userId, x => x.IsDeleted = false)), ct)
                ;

            await _storage.AddRangeAsync(changes, ct);
            await _storage.SaveChangesAsync(ct);
        }
    }
}