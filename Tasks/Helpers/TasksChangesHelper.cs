using System;
using Ajupov.Utils.All.Json;
using Crm.Apps.Tasks.Models;

namespace Crm.Apps.Tasks.Helpers
{
    public static class TasksChangesHelper
    {
        public static TaskChange CreateWithLog(this Task task, Guid userId, Action<Task> action)
        {
            action(task);

            return new TaskChange
            {
                TaskId = task.Id,
                ChangerUserId = userId,
                CreateDateTime = DateTime.UtcNow,
                OldValueJson = string.Empty,
                NewValueJson = task.ToJsonString()
            };
        }

        public static TaskChange UpdateWithLog(this Task task, Guid userId, Action<Task> action)
        {
            var oldValueJson = task.ToJsonString();

            action(task);

            return new TaskChange
            {
                TaskId = task.Id,
                ChangerUserId = userId,
                CreateDateTime = DateTime.UtcNow,
                OldValueJson = oldValueJson,
                NewValueJson = task.ToJsonString()
            };
        }
    }
}
