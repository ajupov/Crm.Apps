using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Utils.All.Guid;
using Ajupov.Utils.All.Sorting;
using Ajupov.Utils.All.String;
using Crm.Apps.Tasks.Helpers;
using Crm.Apps.Tasks.Models;
using Crm.Apps.Tasks.Storages;
using Crm.Apps.Tasks.V1.Requests;
using Crm.Apps.Tasks.V1.Responses;
using Microsoft.EntityFrameworkCore;
using Task = System.Threading.Tasks.Task;
using TaskStatus = Crm.Apps.Tasks.Models.TaskStatus;

namespace Crm.Apps.Tasks.Services
{
    public class TaskStatusesService : ITaskStatusesService
    {
        private readonly TasksStorage _storage;

        public TaskStatusesService(TasksStorage storage)
        {
            _storage = storage;
        }

        public Task<TaskStatus> GetAsync(Guid id, bool isTrackChanges, CancellationToken ct)
        {
            return _storage.TaskStatuses
                .AsTracking(isTrackChanges ? QueryTrackingBehavior.TrackAll : QueryTrackingBehavior.NoTracking)
                .FirstOrDefaultAsync(x => x.Id == id, ct);
        }

        public Task<List<TaskStatus>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct)
        {
            return _storage.TaskStatuses
                .AsNoTracking()
                .Where(x => ids.Contains(x.Id))
                .ToListAsync(ct);
        }

        public async Task<TaskStatusGetPagedListResponse> GetPagedListAsync(
            Guid accountId,
            TaskStatusGetPagedListRequest request,
            CancellationToken ct)
        {
            var statuses = _storage.TaskStatuses
                .AsNoTracking()
                .Where(x =>
                    x.AccountId == accountId &&
                    (request.Name.IsEmpty() || EF.Functions.ILike(x.Name, $"{request.Name}%")) &&
                    (!request.IsFinish.HasValue || x.IsFinish == request.IsFinish) &&
                    (!request.IsDeleted.HasValue || x.IsDeleted == request.IsDeleted) &&
                    (!request.MinCreateDate.HasValue || x.CreateDateTime >= request.MinCreateDate) &&
                    (!request.MaxCreateDate.HasValue || x.CreateDateTime <= request.MaxCreateDate) &&
                    (!request.MinModifyDate.HasValue || x.ModifyDateTime >= request.MinModifyDate) &&
                    (!request.MaxModifyDate.HasValue || x.ModifyDateTime <= request.MaxModifyDate));

            return new TaskStatusGetPagedListResponse
            {
                TotalCount = await statuses
                    .CountAsync(ct),
                LastModifyDateTime = await statuses
                    .MaxAsync(x => x != null ? x.ModifyDateTime ?? x.CreateDateTime : (DateTime?) null, ct),
                Statuses = await statuses
                    .SortBy(request.SortBy, request.OrderBy)
                    .Skip(request.Offset)
                    .Take(request.Limit)
                    .ToListAsync(ct)
            };
        }

        public async Task<Guid> CreateAsync(Guid userId, TaskStatus status, CancellationToken ct)
        {
            var newStatus = new TaskStatus();
            var change = newStatus.CreateWithLog(userId, x =>
            {
                x.Id = status.Id;
                x.AccountId = status.AccountId;
                x.Name = status.Name;
                x.IsFinish = status.IsFinish;
                x.IsDeleted = status.IsDeleted;
                x.CreateDateTime = DateTime.UtcNow;
            });

            var entry = await _storage.AddAsync(newStatus, ct);
            await _storage.AddAsync(change, ct);
            await _storage.SaveChangesAsync(ct);

            return entry.Entity.Id;
        }

        public async Task UpdateAsync(
            Guid userId,
            TaskStatus oldStatus,
            TaskStatus newStatus,
            CancellationToken ct)
        {
            var change = oldStatus.UpdateWithLog(userId, x =>
            {
                x.Name = newStatus.Name;
                x.IsFinish = newStatus.IsFinish;
                x.IsDeleted = newStatus.IsDeleted;
                x.ModifyDateTime = DateTime.UtcNow;
            });

            _storage.Update(oldStatus);
            await _storage.AddAsync(change, ct);
            await _storage.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<TaskStatusChange>();

            await _storage.TaskStatuses
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(x => changes.Add(x.UpdateWithLog(userId, s =>
                {
                    s.IsDeleted = true;
                    s.ModifyDateTime = DateTime.UtcNow;
                })), ct);

            await _storage.AddRangeAsync(changes, ct);
            await _storage.SaveChangesAsync(ct);
        }

        public async Task RestoreAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<TaskStatusChange>();

            await _storage.TaskStatuses
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(x => changes.Add(x.UpdateWithLog(userId, s =>
                {
                    s.IsDeleted = false;
                    s.ModifyDateTime = DateTime.UtcNow;
                })), ct);

            await _storage.AddRangeAsync(changes, ct);
            await _storage.SaveChangesAsync(ct);
        }
    }
}
