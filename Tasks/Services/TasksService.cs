using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Utils.All.Sorting;
using Ajupov.Utils.All.String;
using Crm.Apps.Tasks.Helpers;
using Crm.Apps.Tasks.Mappers;
using Crm.Apps.Tasks.Models;
using Crm.Apps.Tasks.Storages;
using Crm.Apps.Tasks.V1.Requests;
using Crm.Apps.Tasks.V1.Responses;
using Microsoft.EntityFrameworkCore;
using CrmTask = Crm.Apps.Tasks.Models.Task;
using Task = System.Threading.Tasks.Task;

namespace Crm.Apps.Tasks.Services
{
    public class TasksService : ITasksService
    {
        private readonly TasksStorage _storage;

        public TasksService(TasksStorage storage)
        {
            _storage = storage;
        }

        public Task<CrmTask> GetAsync(Guid id, bool isTrackChanges, CancellationToken ct)
        {
            return _storage.Tasks
                .AsTracking(isTrackChanges ? QueryTrackingBehavior.TrackAll : QueryTrackingBehavior.NoTracking)
                .Include(x => x.Type)
                .Include(x => x.Status)
                .Include(x => x.AttributeLinks)
                .FirstOrDefaultAsync(x => x.Id == id, ct);
        }

        public Task<List<CrmTask>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct)
        {
            return _storage.Tasks
                .AsNoTracking()
                .Where(x => ids.Contains(x.Id))
                .ToListAsync(ct);
        }

        public async Task<TaskGetPagedListResponse> GetPagedListAsync(
            Guid accountId,
            TaskGetPagedListRequest request,
            CancellationToken ct)
        {
            var tasks = await _storage.Tasks
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

            return new TaskGetPagedListResponse
            {
                TotalCount = tasks
                    .Count(x => x.FilterByAdditional(request)),
                LastModifyDateTime = tasks
                    .Max(x => x.ModifyDateTime),
                Tasks = tasks
                    .Where(x => x.FilterByAdditional(request))
                    .AsQueryable()
                    .SortBy(request.SortBy, request.OrderBy)
                    .Skip(request.Offset)
                    .Take(request.Limit)
                    .ToList()
            };
        }

        public async Task<Guid> CreateAsync(Guid userId, CrmTask task, CancellationToken ct)
        {
            var newTask = new CrmTask();
            var change = newTask.CreateWithLog(userId, x =>
            {
                x.Id = task.Id;
                x.AccountId = task.AccountId;
                x.TypeId = task.TypeId;
                x.StatusId = task.StatusId;
                x.LeadId = task.LeadId;
                x.CompanyId = task.CompanyId;
                x.ContactId = task.ContactId;
                x.DealId = task.DealId;
                x.CreateUserId = userId;
                x.ResponsibleUserId = task.ResponsibleUserId;
                x.Name = task.Name;
                x.Description = task.Description;
                x.Result = task.Result;
                x.Priority = task.Priority;
                x.StartDateTime = task.StartDateTime;
                x.EndDateTime = task.EndDateTime;
                x.DeadLineDateTime = task.DeadLineDateTime;
                x.IsDeleted = task.IsDeleted;
                x.CreateDateTime = DateTime.UtcNow;
                x.AttributeLinks = task.AttributeLinks.Map(x.Id);
            });

            var entry = await _storage.AddAsync(newTask, ct);
            await _storage.AddAsync(change, ct);
            await _storage.SaveChangesAsync(ct);

            return entry.Entity.Id;
        }

        public async Task UpdateAsync(
            Guid userId,
            CrmTask oldTask,
            CrmTask newTask,
            CancellationToken ct)
        {
            var change = oldTask.UpdateWithLog(userId, x =>
            {
                x.AccountId = newTask.AccountId;
                x.TypeId = newTask.TypeId;
                x.StatusId = newTask.StatusId;
                x.LeadId = newTask.LeadId;
                x.CompanyId = newTask.CompanyId;
                x.ContactId = newTask.ContactId;
                x.DealId = newTask.DealId;
                x.ResponsibleUserId = newTask.ResponsibleUserId;
                x.Name = newTask.Name;
                x.Description = newTask.Description;
                x.Result = newTask.Result;
                x.Priority = newTask.Priority;
                x.StartDateTime = newTask.StartDateTime;
                x.EndDateTime = newTask.EndDateTime;
                x.DeadLineDateTime = newTask.DeadLineDateTime;
                x.ModifyDateTime = DateTime.UtcNow;
                x.IsDeleted = newTask.IsDeleted;
                x.AttributeLinks = newTask.AttributeLinks.Map(x.Id);
            });

            _storage.Update(oldTask);
            await _storage.AddAsync(change, ct);
            await _storage.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<TaskChange>();

            await _storage.Tasks
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
            var changes = new List<TaskChange>();

            await _storage.Tasks
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
