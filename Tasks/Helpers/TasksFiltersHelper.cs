using System;
using System.Collections.Generic;
using System.Linq;
using Ajupov.Utils.All.String;
using Crm.Apps.Tasks.Models;
using Crm.Apps.Tasks.V1.Requests;

namespace Crm.Apps.Tasks.Helpers
{
    public static class TasksFiltersHelper
    {
        public static bool FilterByAdditional(this Task task, TaskGetPagedListRequest request)
        {
            return (request.TypeIds == null || !request.TypeIds.Any() ||
                    request.TypeIds.Any(x => TypeIdsPredicate(task, x))) &&
                   (request.StatusIds == null || !request.StatusIds.Any() ||
                    request.StatusIds.Any(x => StatusIdsPredicate(task, x))) &&
                   (request.CustomerIds == null || !request.CustomerIds.Any() ||
                    request.CustomerIds.Any(x => CustomerIdsPredicate(task, x))) &&
                   (request.OrderIds == null || !request.OrderIds.Any() ||
                    request.OrderIds.Any(x => OrderIdsPredicate(task, x))) &&
                   (request.CreateUserIds == null || !request.CreateUserIds.Any() ||
                    request.CreateUserIds.Any(x => CreateUserIdsPredicate(task, x))) &&
                   (request.ResponsibleUserIds == null || !request.ResponsibleUserIds.Any() ||
                    request.ResponsibleUserIds.Any(x => ResponsibleUserIdsPredicate(task, x))) &&
                   (request.Attributes == null || !request.Attributes.Any() ||
                    (request.AllAttributes is false
                        ? request.Attributes.Any(x => AttributePredicate(task, x))
                        : request.Attributes.All(x => AttributePredicate(task, x)))) &&
                   (request.Priorities == null || !request.Priorities.Any() ||
                    request.Priorities.Any(x => PrioritiesPredicate(task, x)));
        }

        private static bool TypeIdsPredicate(Task task, Guid id)
        {
            return task.TypeId == id;
        }

        private static bool StatusIdsPredicate(Task task, Guid id)
        {
            return task.StatusId == id;
        }

        private static bool CustomerIdsPredicate(Task task, Guid id)
        {
            return task.CustomerId == id;
        }

        private static bool OrderIdsPredicate(Task task, Guid id)
        {
            return task.OrderId == id;
        }

        private static bool CreateUserIdsPredicate(Task task, Guid id)
        {
            return task.CreateUserId == id;
        }

        private static bool ResponsibleUserIdsPredicate(Task task, Guid id)
        {
            return task.ResponsibleUserId == id;
        }

        private static bool AttributePredicate(Task task, KeyValuePair<Guid, string> pair)
        {
            var (key, value) = pair;

            return task.AttributeLinks != null && task.AttributeLinks.Any(x =>
                       x.TaskAttributeId == key && (value.IsEmpty() || x.Value == value));
        }

        private static bool PrioritiesPredicate(Task task, TaskPriority priority)
        {
            return task.Priority == priority;
        }
    }
}
