using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Utils.All.Guid;
using Ajupov.Utils.All.Sorting;
using Ajupov.Utils.All.String;
using Crm.Apps.Activities.Helpers;
using Crm.Apps.Activities.Storages;
using Crm.Apps.Activities.v1.Models;
using Crm.Apps.Activities.v1.RequestParameters;
using Microsoft.EntityFrameworkCore;

namespace Crm.Apps.Activities.Services
{
    public class ActivitiesService : IActivitiesService
    {
        private readonly ActivitiesStorage _activitiesStorage;

        public ActivitiesService(ActivitiesStorage activitiesStorage)
        {
            _activitiesStorage = activitiesStorage;
        }

        public Task<Activity> GetAsync(Guid id, CancellationToken ct)
        {
            return _activitiesStorage.Activities
                .AsNoTracking()
                .Include(x => x.Type)
                .Include(x => x.Status)
                .Include(x => x.AttributeLinks)
                .FirstOrDefaultAsync(x => x.Id == id, ct);
        }

        public Task<List<Activity>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct)
        {
            return _activitiesStorage.Activities
                .AsNoTracking()
                .Where(x => ids.Contains(x.Id))
                .ToListAsync(ct);
        }

        public async Task<List<Activity>> GetPagedListAsync(
            ActivityGetPagedListRequestParameter request,
            CancellationToken ct)
        {
            var temp = await _activitiesStorage.Activities
                .AsNoTracking()
                .Include(x => x.Type)
                .Include(x => x.Status)
                .Include(x => x.AttributeLinks)
                .Where(x =>
                    (request.AccountId.IsEmpty() || x.AccountId == request.AccountId) &&
                    (request.Name.IsEmpty() || EF.Functions.Like(x.Name, $"{request.Name}%")) &&
                    (request.Description.IsEmpty() ||
                     EF.Functions.Like(x.Description, $"%{request.Description}%")) &&
                    (request.Result.IsEmpty() || EF.Functions.Like(x.Result, $"%{request.Result}%")) &&
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
                .SortBy(request.SortBy, request.OrderBy)
                .ToListAsync(ct);

            return temp
                .Where(x => x.FilterByAdditional(request))
                .Skip(request.Offset)
                .Take(request.Limit)
                .ToList();
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
                x.AttributeLinks = activity.AttributeLinks?
                    .Select(l => new ActivityAttributeLink
                    {
                        ActivityId = x.Id,
                        ActivityAttributeId = l.ActivityAttributeId,
                        Value = l.Value,
                        CreateDateTime = DateTime.UtcNow
                    })
                    .ToList();
            });

            var entry = await _activitiesStorage.AddAsync(newActivity, ct);
            await _activitiesStorage.AddAsync(change, ct);
            await _activitiesStorage.SaveChangesAsync(ct);

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
                x.AttributeLinks = newActivity.AttributeLinks?
                    .Select(l => new ActivityAttributeLink
                    {
                        ActivityId = x.Id,
                        ActivityAttributeId = l.ActivityAttributeId,
                        Value = l.Value,
                        CreateDateTime = DateTime.UtcNow
                    })
                    .ToList();
            });

            _activitiesStorage.Update(oldActivity);
            await _activitiesStorage.AddAsync(change, ct);
            await _activitiesStorage.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(Guid activityId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<ActivityChange>();

            await _activitiesStorage.Activities
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(u => changes.Add(u.UpdateWithLog(activityId, x =>
                {
                    x.IsDeleted = true;
                    x.ModifyDateTime = DateTime.UtcNow;
                })), ct);

            await _activitiesStorage.AddRangeAsync(changes, ct);
            await _activitiesStorage.SaveChangesAsync(ct);
        }

        public async Task RestoreAsync(Guid activityId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<ActivityChange>();

            await _activitiesStorage.Activities
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(u => changes.Add(u.UpdateWithLog(activityId, x =>
                {
                    x.IsDeleted = false;
                    x.ModifyDateTime = DateTime.UtcNow;
                })), ct);

            await _activitiesStorage.AddRangeAsync(changes, ct);
            await _activitiesStorage.SaveChangesAsync(ct);
        }
    }
}