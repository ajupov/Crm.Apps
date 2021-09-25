using System;
using Ajupov.Utils.All.Json;
using Crm.Apps.Tasks.Models;

namespace Crm.Apps.Tasks.Helpers
{
    public static class TaskStatusChangesHelper
    {
        public static TaskStatusChange CreateWithLog(
            this TaskStatus status,
            Guid userId,
            Action<TaskStatus> action)
        {
            action(status);

            return new TaskStatusChange
            {
                StatusId = status.Id,
                ChangerUserId = userId,
                CreateDateTime = DateTime.UtcNow,
                OldValueJson = string.Empty,
                NewValueJson = status.ToJsonString()
            };
        }

        public static TaskStatusChange UpdateWithLog(
            this TaskStatus status,
            Guid userId,
            Action<TaskStatus> action)
        {
            var oldValueJson = status.ToJsonString();

            action(status);

            return new TaskStatusChange
            {
                StatusId = status.Id,
                ChangerUserId = userId,
                CreateDateTime = DateTime.UtcNow,
                OldValueJson = oldValueJson,
                NewValueJson = status.ToJsonString()
            };
        }
    }
}
