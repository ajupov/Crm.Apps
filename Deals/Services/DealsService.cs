using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Utils.All.Decimal;
using Ajupov.Utils.All.Sorting;
using Ajupov.Utils.All.String;
using Crm.Apps.Deals.Helpers;
using Crm.Apps.Deals.Mappers;
using Crm.Apps.Deals.Models;
using Crm.Apps.Deals.Storages;
using Crm.Apps.Deals.V1.Requests;
using Crm.Apps.Deals.V1.Responses;
using Microsoft.EntityFrameworkCore;

namespace Crm.Apps.Deals.Services
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

        public async Task<DealGetPagedListResponse> GetPagedListAsync(
            Guid accountId,
            DealGetPagedListRequest request,
            CancellationToken ct)
        {
            var deals = await _storage.Deals
                .AsNoTracking()
                .Include(x => x.Type)
                .Include(x => x.Status)
                .Include(x => x.Positions)
                .Include(x => x.AttributeLinks)
                .Where(x =>
                    x.AccountId == accountId &&
                    (request.Name.IsEmpty() || EF.Functions.ILike(x.Name, $"{request.Name}%")) &&
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
                .ToListAsync(ct);

            return new DealGetPagedListResponse
            {
                TotalCount = deals
                    .Count(x => x.FilterByAdditional(request)),
                LastModifyDateTime = deals
                    .Max(x => x.ModifyDateTime),
                Deals = deals
                    .Where(x => x.FilterByAdditional(request))
                    .AsQueryable()
                    .SortBy(request.SortBy, request.OrderBy)
                    .Skip(request.Offset)
                    .Take(request.Limit)
                    .ToList()
            };
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
                x.AttributeLinks = deal.AttributeLinks.Map(x.Id);
                x.Positions = deal.Positions.Map(x.Id);
            });

            var entry = await _storage.AddAsync(newDeal, ct);
            await _storage.AddAsync(change, ct);
            await _storage.SaveChangesAsync(ct);

            return entry.Entity.Id;
        }

        public async Task UpdateAsync(Guid userId, Deal oldDeal, Deal newDeal, CancellationToken ct)
        {
            var change = oldDeal.UpdateWithLog(userId, x =>
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
                x.ModifyDateTime = DateTime.UtcNow;
                x.AttributeLinks = newDeal.AttributeLinks.Map(x.Id);
                x.Positions = newDeal.Positions.Map(x.Id);
            });

            _storage.Update(oldDeal);
            await _storage.AddAsync(change, ct);
            await _storage.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<DealChange>();

            await _storage.Deals
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(x => changes.Add(x.UpdateWithLog(userId, d =>
                {
                    d.IsDeleted = true;
                    d.ModifyDateTime = DateTime.UtcNow;
                })), ct);

            await _storage.AddRangeAsync(changes, ct);
            await _storage.SaveChangesAsync(ct);
        }

        public async Task RestoreAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<DealChange>();

            await _storage.Deals
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(x => changes.Add(x.UpdateWithLog(userId, d =>
                {
                    d.IsDeleted = false;
                    d.ModifyDateTime = DateTime.UtcNow;
                })), ct);

            await _storage.AddRangeAsync(changes, ct);
            await _storage.SaveChangesAsync(ct);
        }
    }
}
