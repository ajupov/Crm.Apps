using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Utils.All.Decimal;
using Ajupov.Utils.All.Guid;
using Ajupov.Utils.All.String;
using Crm.Apps.Areas.Deals.Helpers;
using Crm.Apps.Areas.Deals.Models;
using Crm.Apps.Areas.Deals.RequestParameters;
using Crm.Apps.Areas.Deals.Storages;
using Crm.Apps.Utils;
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
            return _storage.Deals
                .AsNoTracking()
                .Include(x => x.Type)
                .Include(x => x.Status)
                .Include(x => x.Positions)
                .Include(x => x.AttributeLinks)
                .FirstOrDefaultAsync(x => x.Id == id, ct);
        }

        public Task<List<Deal>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct)
        {
            return _storage.Deals
                .AsNoTracking()
                .Where(x => ids.Contains(x.Id))
                .ToListAsync(ct);
        }

        public async Task<List<Deal>> GetPagedListAsync(DealGetPagedListRequestParameter request, CancellationToken ct)
        {
            var temp = await _storage.Deals
                .AsNoTracking()
                .Include(x => x.Type)
                .Include(x => x.Status)
                .Include(x => x.Positions)
                .Include(x => x.AttributeLinks)
                .Where(x =>
                    (request.AccountId.IsEmpty() || x.AccountId == request.AccountId) &&
                    (request.Name.IsEmpty() || EF.Functions.Like(x.Name, $"{request.Name}%")) &&
                    (!request.MinStartDateTime.HasValue || x.StartDateTime >= request.MinStartDateTime) &&
                    (!request.MaxStartDateTime.HasValue || x.StartDateTime <= request.MaxStartDateTime) &&
                    (!request.MinEndDateTime.HasValue || x.EndDateTime >= request.MinEndDateTime) &&
                    (!request.MaxEndDateTime.HasValue || x.EndDateTime <= request.MaxEndDateTime) &&
                    (request.MinSum.IsEmpty() || x.Sum >= request.MinSum) &&
                    (request.MaxSum.IsEmpty() || x.Sum <= request.MaxSum) &&
                    (request.MinSumWithoutDiscount.IsEmpty() ||
                     x.SumWithoutDiscount >= request.MinSumWithoutDiscount) &&
                    (request.MaxSumWithoutDiscount.IsEmpty() ||
                     x.SumWithoutDiscount <= request.MaxSumWithoutDiscount) &&
                    (request.MinFinishProbability == null || request.MinFinishProbability.Value == 0 ||
                     x.FinishProbability >= request.MinFinishProbability.Value) &&
                    (request.MaxFinishProbability == null || request.MaxFinishProbability.Value == 0 ||
                     x.FinishProbability <= request.MaxFinishProbability) &&
                    (!request.IsDeleted.HasValue || x.IsDeleted == request.IsDeleted) &&
                    (!request.MinCreateDate.HasValue || x.CreateDateTime >= request.MinCreateDate) &&
                    (!request.MaxCreateDate.HasValue || x.CreateDateTime <= request.MaxCreateDate) &&
                    (!request.MinModifyDate.HasValue || x.ModifyDateTime >= request.MinModifyDate) &&
                    (!request.MaxModifyDate.HasValue || x.ModifyDateTime <= request.MaxModifyDate))
                .SortBy(request.SortBy, request.OrderBy)
                .ToListAsync(ct);

            return temp
                .Where(x => x.FilterByAdditional(request))
                .Skip(request.Offset)
                .Take(request.Limit)
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

            await _storage.Deals
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(u => changes.Add(u.UpdateWithLog(dealId, x => x.IsDeleted = true)), ct);

            await _storage.AddRangeAsync(changes, ct);
            await _storage.SaveChangesAsync(ct);
        }

        public async Task RestoreAsync(Guid dealId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<DealChange>();

            await _storage.Deals
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(u => changes.Add(u.UpdateWithLog(dealId, x => x.IsDeleted = false)), ct);

            await _storage.AddRangeAsync(changes, ct);
            await _storage.SaveChangesAsync(ct);
        }
    }
}