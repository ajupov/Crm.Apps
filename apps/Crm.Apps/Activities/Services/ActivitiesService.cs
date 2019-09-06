using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Activities.Helpers;
using Crm.Apps.Activities.Models;
using Crm.Apps.Activities.RequestParameters;
using Crm.Apps.Activities.Storages;
using Crm.Utils.Guid;
using Crm.Utils.Sorting;
using Crm.Utils.String;
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
                .Include(x => x.Type)
                .Include(x => x.Status)
                .Include(x => x.AttributeLinks)
                .FirstOrDefaultAsync(x => x.Id == id, ct);
        }

        public Task<Activity[]> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct)
        {
            return _activitiesStorage.Activities
                .Where(x => ids.Contains(x.Id))
                .ToArrayAsync(ct);
        }

        public async Task<Activity[]> GetPagedListAsync(ActivityGetPagedListRequest request, CancellationToken ct)
        {
            var temp = await _activitiesStorage.Activities
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
                    (!request.MaxCreateDate.HasValue || x.CreateDateTime <= request.MaxCreateDate))
                .SortBy(request.SortBy, request.OrderBy)
                .ToListAsync(ct);

            return temp
                .Where(x => x.FilterByAdditional(request))
                .Skip(request.Offset)
                .Take(request.Limit)
                .ToArray();
        }

        public async Task<Guid> CreateAsync(Guid userId, ActivityCreateRequest request, CancellationToken ct)
        {
            var activity = new Activity();
            var change = activity.CreateWithLog(userId, x =>
            {
                x.Id = Guid.NewGuid();
                x.AccountId = request.AccountId;
                x.TypeId = request.TypeId;
                x.StatusId = request.StatusId;
                x.LeadId = request.LeadId;
                x.CompanyId = request.CompanyId;
                x.ContactId = request.ContactId;
                x.DealId = request.DealId;
                x.CreateUserId = userId;
                x.ResponsibleUserId = request.ResponsibleUserId;
                x.Name = request.Name;
                x.Description = request.Description;
                x.Result = request.Result;
                x.Priority = request.Priority;
                x.StartDateTime = request.StartDateTime;
                x.EndDateTime = request.EndDateTime;
                x.DeadLineDateTime = request.DeadLineDateTime;
                x.IsDeleted = request.IsDeleted;
                x.CreateDateTime = DateTime.UtcNow;
                x.AttributeLinks = request.AttributeLinks?
                    .Select(l => new ActivityAttributeLink
                    {
                        ActivityId = x.Id,
                        ActivityAttributeId = l.ActivityAttributeId,
                        Value = l.Value,
                        CreateDateTime = DateTime.UtcNow
                    }).ToList();
            });

            var entry = await _activitiesStorage.AddAsync(activity, ct);
            await _activitiesStorage.AddAsync(change, ct);
            await _activitiesStorage.SaveChangesAsync(ct);

            return entry.Entity.Id;
        }

        public async Task UpdateAsync(Guid userId, Activity activity, ActivityUpdateRequest request,
            CancellationToken ct)
        {
            var change = activity.UpdateWithLog(userId, x =>
            {
                x.AccountId = request.AccountId;
                x.TypeId = request.TypeId;
                x.StatusId = request.StatusId;
                x.LeadId = request.LeadId;
                x.CompanyId = request.CompanyId;
                x.ContactId = request.ContactId;
                x.DealId = request.DealId;
                x.ResponsibleUserId = request.ResponsibleUserId;
                x.Name = request.Name;
                x.Description = request.Description;
                x.Result = request.Result;
                x.Priority = request.Priority;
                x.StartDateTime = request.StartDateTime;
                x.EndDateTime = request.EndDateTime;
                x.DeadLineDateTime = request.DeadLineDateTime;
                x.IsDeleted = request.IsDeleted;
                x.AttributeLinks = request.AttributeLinks?
                    .Select(l => new ActivityAttributeLink
                    {
                        ActivityId = x.Id,
                        ActivityAttributeId = l.ActivityAttributeId,
                        Value = l.Value,
                        CreateDateTime = DateTime.UtcNow
                    }).ToList();
            });

            _activitiesStorage.Update(activity);
            await _activitiesStorage.AddAsync(change, ct);
            await _activitiesStorage.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(Guid activityId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<ActivityChange>();

            await _activitiesStorage.Activities
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(u => changes.Add(u.UpdateWithLog(activityId, x => x.IsDeleted = true)), ct);

            await _activitiesStorage.AddRangeAsync(changes, ct);
            await _activitiesStorage.SaveChangesAsync(ct);
        }

        public async Task RestoreAsync(Guid activityId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<ActivityChange>();

            await _activitiesStorage.Activities
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(u => changes.Add(u.UpdateWithLog(activityId, x => x.IsDeleted = false)), ct);

            await _activitiesStorage.AddRangeAsync(changes, ct);
            await _activitiesStorage.SaveChangesAsync(ct);
        }
    }
}