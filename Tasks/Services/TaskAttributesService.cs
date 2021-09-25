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

namespace Crm.Apps.Tasks.Services
{
    public class TaskAttributesService : ITaskAttributesService
    {
        private readonly TasksStorage _storage;

        public TaskAttributesService(TasksStorage storage)
        {
            _storage = storage;
        }

        public Task<TaskAttribute> GetAsync(Guid id, bool isTrackChanges, CancellationToken ct)
        {
            return _storage.TaskAttributes
                .AsTracking(isTrackChanges ? QueryTrackingBehavior.TrackAll : QueryTrackingBehavior.NoTracking)
                .FirstOrDefaultAsync(x => x.Id == id, ct);
        }

        public Task<List<TaskAttribute>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct)
        {
            return _storage.TaskAttributes
                .AsNoTracking()
                .Where(x => ids.Contains(x.Id))
                .ToListAsync(ct);
        }

        public async Task<TaskAttributeGetPagedListResponse> GetPagedListAsync(
            Guid accountId,
            TaskAttributeGetPagedListRequest request,
            CancellationToken ct)
        {
            var attributes = _storage.TaskAttributes
                .AsNoTracking()
                .Where(x =>
                    x.AccountId == accountId &&
                    (request.Types == null || !request.Types.Any() || request.Types.Contains(x.Type)) &&
                    (request.Key.IsEmpty() || EF.Functions.ILike(x.Key, $"{request.Key}%")) &&
                    (!request.IsDeleted.HasValue || x.IsDeleted == request.IsDeleted) &&
                    (!request.MinCreateDate.HasValue || x.CreateDateTime >= request.MinCreateDate) &&
                    (!request.MaxCreateDate.HasValue || x.CreateDateTime <= request.MaxCreateDate) &&
                    (!request.MinModifyDate.HasValue || x.ModifyDateTime >= request.MinModifyDate) &&
                    (!request.MaxModifyDate.HasValue || x.ModifyDateTime <= request.MaxModifyDate));

            return new TaskAttributeGetPagedListResponse
            {
                TotalCount = await attributes
                    .CountAsync(ct),
                LastModifyDateTime = await attributes
                    .MaxAsync(x => x != null ? x.ModifyDateTime ?? x.CreateDateTime : (DateTime?) null, ct),
                Attributes = await attributes
                    .SortBy(request.SortBy, request.OrderBy)
                    .Skip(request.Offset)
                    .Take(request.Limit)
                    .ToListAsync(ct)
            };
        }

        public async Task<Guid> CreateAsync(Guid userId, TaskAttribute attribute, CancellationToken ct)
        {
            var newAttribute = new TaskAttribute();
            var change = newAttribute.CreateWithLog(userId, x =>
            {
                x.Id = attribute.Id;
                x.AccountId = attribute.AccountId;
                x.Type = attribute.Type;
                x.Key = attribute.Key;
                x.IsDeleted = attribute.IsDeleted;
                x.CreateDateTime = DateTime.UtcNow;
            });

            var entry = await _storage.AddAsync(newAttribute, ct);
            await _storage.AddAsync(change, ct);
            await _storage.SaveChangesAsync(ct);

            return entry.Entity.Id;
        }

        public async Task UpdateAsync(
            Guid userId,
            TaskAttribute oldAttribute,
            TaskAttribute newAttribute,
            CancellationToken ct)
        {
            var change = oldAttribute.UpdateWithLog(userId, x =>
            {
                x.AccountId = newAttribute.AccountId;
                x.Type = newAttribute.Type;
                x.Key = newAttribute.Key;
                x.ModifyDateTime = DateTime.UtcNow;
                x.IsDeleted = newAttribute.IsDeleted;
            });

            _storage.Update(oldAttribute);
            await _storage.AddAsync(change, ct);
            await _storage.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<TaskAttributeChange>();

            await _storage.TaskAttributes
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
            var changes = new List<TaskAttributeChange>();

            await _storage.TaskAttributes
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
