using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Deals.Helpers;
using Crm.Apps.Areas.Deals.Models;
using Crm.Apps.Areas.Deals.Parameters;
using Crm.Apps.Areas.Deals.Storages;
using Microsoft.EntityFrameworkCore;

namespace Crm.Apps.Areas.Deals.Services
{
    public class DealsService : IDealsService
    {
        private readonly DealsStorage _storage;

        public DealsService(DealsStorage storage)
        {
            _storage = storage;
        }

        public Task<Deal> GetAsync(Guid id, CancellationToken ct)
        {
            return _storage.Deals.Include(x => x.Type).Include(x => x.Status).Include(x => x.Positions)
                .Include(x => x.AttributeLinks).FirstOrDefaultAsync(x => x.Id == id, ct);
        }

        public Task<List<Deal>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct)
        {
            return _storage.Deals.Where(x => ids.Contains(x.Id)).ToListAsync(ct);
        }

        public async Task<List<Deal>> GetPagedListAsync(DealGetPagedListParameter parameter, CancellationToken ct)
        {
            var temp = await _storage.Deals.Include(x => x.Type).Include(x => x.Status).Include(x => x.Positions)
                .Include(x => x.AttributeLinks).Where(x =>
                    (parameter.AccountId.IsEmpty() || x.AccountId == parameter.AccountId) &&
                    (parameter.Name.IsEmpty() || EF.Functions.Like(x.Name, $"{parameter.Name}%")) &&
                    (!parameter.MinStartDateTime.HasValue || x.StartDateTime >= parameter.MinStartDateTime) &&
                    (!parameter.MaxStartDateTime.HasValue || x.StartDateTime <= parameter.MaxStartDateTime) &&
                    (!parameter.MinEndDateTime.HasValue || x.EndDateTime >= parameter.MinEndDateTime) &&
                    (!parameter.MaxEndDateTime.HasValue || x.EndDateTime <= parameter.MaxEndDateTime) &&
                    (parameter.MinSum.IsEmpty() || x.Sum >= parameter.MinSum) &&
                    (parameter.MaxSum.IsEmpty() || x.Sum <= parameter.MaxSum) &&
                    (parameter.MinSumWithoutDiscount.IsEmpty() || x.SumWithoutDiscount >= parameter.MinSumWithoutDiscount) &&
                    (parameter.MaxSumWithoutDiscount.IsEmpty() || x.SumWithoutDiscount <= parameter.MaxSumWithoutDiscount) &&
                    (parameter.MinFinishProbability == null || parameter.MinFinishProbability.Value == 0 ||
                     x.FinishProbability >= parameter.MinFinishProbability.Value) &&
                    (parameter.MaxFinishProbability == null || parameter.MaxFinishProbability.Value == 0 ||
                     x.FinishProbability <= parameter.MaxFinishProbability) &&
                    (!parameter.IsDeleted.HasValue || x.IsDeleted == parameter.IsDeleted) &&
                    (!parameter.MinCreateDate.HasValue || x.CreateDateTime >= parameter.MinCreateDate) &&
                    (!parameter.MaxCreateDate.HasValue || x.CreateDateTime <= parameter.MaxCreateDate))
                .Sort(parameter.SortBy, parameter.OrderBy)
                .ToListAsync(ct);

            return temp.Where(x => x.FilterByAdditional(parameter)).Skip(parameter.Offset).Take(parameter.Limit)
                .ToList();
        }

        public async Task<Guid> CreateAsync(Guid userId, Deal deal, CancellationToken ct)
        {
            var newDeal = new Deal();
            var change = newDeal.CreateWithLog(userId, x =>
            {
                x.Id = Guid.NewGuid();
                x.AccountId = deal.AccountId;
                x.TypeId = deal.TypeId;
                x.StatusId = deal.StatusId;
                x.CompanyId = deal.CompanyId;
                x.ContactId = deal.ContactId;
                x.CreateUserId = userId;
                x.ResponsibleUserId = deal.ResponsibleUserId;
                x.Name = deal.Name;
                x.StartDateTime = deal.StartDateTime;
                x.EndDateTime = deal.EndDateTime;
                x.Sum = deal.Sum;
                x.SumWithoutDiscount = deal.SumWithoutDiscount;
                x.FinishProbability = deal.FinishProbability;
                x.IsDeleted = deal.IsDeleted;
                x.CreateDateTime = DateTime.UtcNow;
                x.AttributeLinks = deal.AttributeLinks;
                x.Positions = deal.Positions;
            });

            var entry = await _storage.AddAsync(newDeal, ct);
            await _storage.AddAsync(change, ct);
            await _storage.SaveChangesAsync(ct);

            return entry.Entity.Id;
        }

        public async Task UpdateAsync(Guid dealId, Deal oldDeal, Deal newDeal, CancellationToken ct)
        {
            var change = oldDeal.UpdateWithLog(dealId, x =>
            {
                x.AccountId = newDeal.AccountId;
                x.TypeId = newDeal.TypeId;
                x.StatusId = newDeal.StatusId;
                x.CompanyId = newDeal.CompanyId;
                x.ContactId = newDeal.ContactId;
                x.ResponsibleUserId = newDeal.ResponsibleUserId;
                x.Name = newDeal.Name;
                x.StartDateTime = newDeal.StartDateTime;
                x.EndDateTime = newDeal.EndDateTime;
                x.Sum = newDeal.Sum;
                x.SumWithoutDiscount = newDeal.SumWithoutDiscount;
                x.FinishProbability = newDeal.FinishProbability;
                x.IsDeleted = newDeal.IsDeleted;
                x.AttributeLinks = newDeal.AttributeLinks;
                x.Positions = newDeal.Positions;
            });

            _storage.Update(oldDeal);
            await _storage.AddAsync(change, ct);
            await _storage.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(Guid dealId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<DealChange>();

            await _storage.Deals.Where(x => ids.Contains(x.Id))
                .ForEachAsync(u => changes.Add(u.UpdateWithLog(dealId, x => x.IsDeleted = true)), ct)
                ;

            await _storage.AddRangeAsync(changes, ct);
            await _storage.SaveChangesAsync(ct);
        }

        public async Task RestoreAsync(Guid dealId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<DealChange>();

            await _storage.Deals.Where(x => ids.Contains(x.Id))
                .ForEachAsync(u => changes.Add(u.UpdateWithLog(dealId, x => x.IsDeleted = false)), ct)
                ;

            await _storage.AddRangeAsync(changes, ct);
            await _storage.SaveChangesAsync(ct);
        }
    }
}