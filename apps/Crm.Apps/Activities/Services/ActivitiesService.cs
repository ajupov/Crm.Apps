using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Activities.Helpers;
using Crm.Apps.Activities.Models;
using Crm.Apps.Activities.Parameters;
using Crm.Apps.Activities.Storages;
using Crm.Utils.Guid;
using Crm.Utils.String;
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

        public Task<Activity> GetAsync(Guid id, CancellationToken ct)
        {
            return _storage.Activities.Include(x => x.Type).Include(x => x.Status).Include(x => x.AttributeLinks)
                .FirstOrDefaultAsync(x => x.Id == id, ct);
        }

        public Task<List<Activity>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct)
        {
            return _storage.Activities.Where(x => ids.Contains(x.Id)).ToListAsync(ct);
        }

        public async Task<List<Activity>> GetPagedListAsync(ActivityGetPagedListParameter parameter,
            CancellationToken ct)
        {
            var temp = await _storage.Activities.Include(x => x.Type).Include(x => x.Status)
                .Include(x => x.AttributeLinks).Where(x =>
                    (parameter.AccountId.IsEmpty() || x.AccountId == parameter.AccountId) &&
                    (parameter.Name.IsEmpty() || EF.Functions.Like(x.Name, $"{parameter.Name}%")) &&
                    (parameter.Description.IsEmpty() ||
                     EF.Functions.Like(x.Description, $"%{parameter.Description}%")) &&
                    (parameter.Result.IsEmpty() || EF.Functions.Like(x.Result, $"%{parameter.Result}%")) &&
                    (!parameter.MinStartDateTime.HasValue || x.StartDateTime >= parameter.MinStartDateTime) &&
                    (!parameter.MaxStartDateTime.HasValue || x.StartDateTime <= parameter.MaxStartDateTime) &&
                    (!parameter.MinEndDateTime.HasValue || x.EndDateTime >= parameter.MinEndDateTime) &&
                    (!parameter.MaxEndDateTime.HasValue || x.EndDateTime <= parameter.MaxEndDateTime) &&
                    (!parameter.MinDeadLineDateTime.HasValue || x.DeadLineDateTime >= parameter.MinDeadLineDateTime) &&
                    (!parameter.MaxDeadLineDateTime.HasValue || x.DeadLineDateTime <= parameter.MaxDeadLineDateTime) &&
                    (!parameter.IsDeleted.HasValue || x.IsDeleted == parameter.IsDeleted) &&
                    (!parameter.MinCreateDate.HasValue || x.CreateDateTime >= parameter.MinCreateDate) &&
                    (!parameter.MaxCreateDate.HasValue || x.CreateDateTime <= parameter.MaxCreateDate))
                .Sort(parameter.SortBy, parameter.OrderBy)
                .ToListAsync(ct).ConfigureAwait(false);

            return temp.Where(x => x.FilterByAdditional(parameter)).Skip(parameter.Offset).Take(parameter.Limit)
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
                x.AttributeLinks = activity.AttributeLinks;
            });

            var entry = await _storage.AddAsync(newActivity, ct).ConfigureAwait(false);
            await _storage.AddAsync(change, ct).ConfigureAwait(false);
            await _storage.SaveChangesAsync(ct).ConfigureAwait(false);

            return entry.Entity.Id;
        }

        public async Task UpdateAsync(Guid activityId, Activity oldActivity, Activity newActivity, CancellationToken ct)
        {
            var change = oldActivity.UpdateWithLog(activityId, x =>
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
                x.IsDeleted = newActivity.IsDeleted;
                x.AttributeLinks = newActivity.AttributeLinks;
            });

            _storage.Update(oldActivity);
            await _storage.AddAsync(change, ct).ConfigureAwait(false);
            await _storage.SaveChangesAsync(ct).ConfigureAwait(false);
        }

        public async Task DeleteAsync(Guid activityId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<ActivityChange>();

            await _storage.Activities.Where(x => ids.Contains(x.Id))
                .ForEachAsync(u => changes.Add(u.UpdateWithLog(activityId, x => x.IsDeleted = true)), ct)
                .ConfigureAwait(false);

            await _storage.AddRangeAsync(changes, ct).ConfigureAwait(false);
            await _storage.SaveChangesAsync(ct).ConfigureAwait(false);
        }

        public async Task RestoreAsync(Guid activityId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<ActivityChange>();

            await _storage.Activities.Where(x => ids.Contains(x.Id))
                .ForEachAsync(u => changes.Add(u.UpdateWithLog(activityId, x => x.IsDeleted = false)), ct)
                .ConfigureAwait(false);

            await _storage.AddRangeAsync(changes, ct).ConfigureAwait(false);
            await _storage.SaveChangesAsync(ct).ConfigureAwait(false);
        }
    }
}