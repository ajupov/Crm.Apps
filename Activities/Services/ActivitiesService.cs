using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Utils.All.Sorting;
using Ajupov.Utils.All.String;
using Crm.Apps.Activities.Helpers;
using Crm.Apps.Activities.Mappers;
using Crm.Apps.Activities.Models;
using Crm.Apps.Activities.Storages;
using Crm.Apps.Activities.V1.Requests;
using Crm.Apps.Activities.V1.Responses;
using Microsoft.EntityFrameworkCore;

namespace Crm.Apps.Activities.Services
{
    public class ActivitiesService : IActivitiesService
    {
        private readonly ActivitiesStorage _storage;

        public ActivitiesService(ActivitiesStorage storage)
        {
            _storage = storage;
        }

        public Task<Activity> GetAsync(Guid id, bool isTrackChanges, CancellationToken ct)
        {
            return _storage.Activities
                .AsTracking(isTrackChanges ? QueryTrackingBehavior.TrackAll : QueryTrackingBehavior.NoTracking)
                .Include(x => x.Type)
                .Include(x => x.Status)
                .Include(x => x.AttributeLinks)
                .FirstOrDefaultAsync(x => x.Id == id, ct);
        }

        public Task<List<Activity>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct)
        {
            return _storage.Activities
                .AsNoTracking()
                .Where(x => ids.Contains(x.Id))
                .ToListAsync(ct);
        }

        public async Task<ActivityGetPagedListResponse> GetPagedListAsync(
            Guid accountId,
            ActivityGetPagedListRequest request,
            CancellationToken ct)
        {
            var activities = await _storage.Activities
                .AsNoTracking()
                .Include(x => x.Type)
                .Include(x => x.Status)
                .Include(x => x.AttributeLinks)
                .Where(x =>
                    x.AccountId == accountId &&
                    (request.Name.IsEmpty() || EF.Functions.ILike(x.Name, $"{request.Name}%")) &&
                    (request.Description.IsEmpty() ||
                     EF.Functions.ILike(x.Description, $"%{request.Description}%")) &&
                    (request.Result.IsEmpty() || EF.Functions.ILike(x.Result, $"%{request.Result}%")) &&
                    (!request.MinStartDateTime.HasValue || x.StartDateTime >= request.MinStartDateTime) &&
                    (!request.MaxStartDateTime.HasValue || x.StartDateTime <= request.MaxStartDateTime) &&
                    (!request.MinEndDateTime.HasValue || x.EndDateTime >= request.MinEndDateTime) &&
                    (!request.MaxEndDateTime.HasValue || x.EndDateTime <= request.MaxEndDateTime) &&
                    (!request.MinDeadLineDateTime.HasValue || x.DeadLineDateTime >= request.MinDeadLineDateTime) &&
                    (!request.MaxDeadLineDateTime.HasValue || x.DeadLineDateTime <= request.MaxDeadLineDateTime) &&
                    (!request.IsDeleted.HasValue || x.IsDeleted == request.IsDeleted) &&
                    (!request.MinCreateDate.HasValue || x.CreateDateTime >= request.MinCreateDate) &&
                    (!request.MaxCreateDate.HasValue || x.CreateDateTime <= request.MaxCreateDate) &&
                    (!request.MinModifyDate.HasValue || x.ModifyDateTime >= request.MinModifyDate) &&
                    (!request.MaxModifyDate.HasValue || x.ModifyDateTime <= request.MaxModifyDate))
                .ToListAsync(ct);

            return new ActivityGetPagedListResponse
            {
                TotalCount = activities
                    .Count(x => x.FilterByAdditional(request)),
                LastModifyDateTime = activities
                    .Max(x => x.ModifyDateTime),
                Activities = activities
                    .Where(x => x.FilterByAdditional(request))
                    .AsQueryable()
                    .SortBy(request.SortBy, request.OrderBy)
                    .Skip(request.Offset)
                    .Take(request.Limit)
                    .ToList()
            };
        }

        public async Task<Guid> CreateAsync(Guid userId, Activity activity, CancellationToken ct)
        {
            var newActivity = new Activity();
            var change = newActivity.CreateWithLog(userId, x =>
            {
                x.Id = Guid.NewGuid();
                x.AccountId = activity.AccountId;
                x.TypeId = activity.TypeId;
                x.StatusId = activity.StatusId;
                x.LeadId = activity.LeadId;
                x.CompanyId = activity.CompanyId;
                x.ContactId = activity.ContactId;
                x.DealId = activity.DealId;
                x.CreateUserId = userId;
                x.ResponsibleUserId = activity.ResponsibleUserId;
                x.Name = activity.Name;
                x.Description = activity.Description;
                x.Result = activity.Result;
                x.Priority = activity.Priority;
                x.StartDateTime = activity.StartDateTime;
                x.EndDateTime = activity.EndDateTime;
                x.DeadLineDateTime = activity.DeadLineDateTime;
                x.IsDeleted = activity.IsDeleted;
                x.CreateDateTime = DateTime.UtcNow;
                x.AttributeLinks = activity.AttributeLinks.Map(x.Id);
            });

            var entry = await _storage.AddAsync(newActivity, ct);
            await _storage.AddAsync(change, ct);
            await _storage.SaveChangesAsync(ct);

            return entry.Entity.Id;
        }

        public async Task UpdateAsync(
            Guid userId,
            Activity oldActivity,
            Activity newActivity,
            CancellationToken ct)
        {
            var change = oldActivity.UpdateWithLog(userId, x =>
            {
                x.AccountId = newActivity.AccountId;
                x.TypeId = newActivity.TypeId;
                x.StatusId = newActivity.StatusId;
                x.LeadId = newActivity.LeadId;
                x.CompanyId = newActivity.CompanyId;
                x.ContactId = newActivity.ContactId;
                x.DealId = newActivity.DealId;
                x.ResponsibleUserId = newActivity.ResponsibleUserId;
                x.Name = newActivity.Name;
                x.Description = newActivity.Description;
                x.Result = newActivity.Result;
                x.Priority = newActivity.Priority;
                x.StartDateTime = newActivity.StartDateTime;
                x.EndDateTime = newActivity.EndDateTime;
                x.DeadLineDateTime = newActivity.DeadLineDateTime;
                x.ModifyDateTime = DateTime.UtcNow;
                x.IsDeleted = newActivity.IsDeleted;
                x.AttributeLinks = newActivity.AttributeLinks.Map(x.Id);
            });

            _storage.Update(oldActivity);
            await _storage.AddAsync(change, ct);
            await _storage.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<ActivityChange>();

            await _storage.Activities
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(x => changes.Add(x.UpdateWithLog(userId, a =>
                {
                    a.IsDeleted = true;
                    a.ModifyDateTime = DateTime.UtcNow;
                })), ct);

            await _storage.AddRangeAsync(changes, ct);
            await _storage.SaveChangesAsync(ct);
        }

        public async Task RestoreAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<ActivityChange>();

            await _storage.Activities
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(x => changes.Add(x.UpdateWithLog(userId, a =>
                {
                    a.IsDeleted = false;
                    a.ModifyDateTime = DateTime.UtcNow;
                })), ct);

            await _storage.AddRangeAsync(changes, ct);
            await _storage.SaveChangesAsync(ct);
        }
    }
}
