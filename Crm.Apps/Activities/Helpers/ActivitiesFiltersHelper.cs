using System;
using System.Collections.Generic;
using System.Linq;
using Ajupov.Utils.All.String;
using Crm.Apps.Activities.v1.Models;
using Crm.Apps.Activities.v1.RequestParameters;

namespace Crm.Apps.Activities.Helpers
{
    public static class ActivitiesFiltersHelper
    {
        public static bool FilterByAdditional(this Activity activity, ActivityGetPagedListRequestParameter request)
        {
            return (request.TypeIds == null || !request.TypeIds.Any() ||
                    request.TypeIds.Any(x => TypeIdsPredicate(activity, x))) &&
                   (request.StatusIds == null || !request.StatusIds.Any() ||
                    request.StatusIds.Any(x => StatusIdsPredicate(activity, x))) &&
                   (request.LeadIds == null || !request.LeadIds.Any() ||
                    request.LeadIds.Any(x => LeadIdsPredicate(activity, x))) &&
                   (request.CompanyIds == null || !request.CompanyIds.Any() ||
                    request.CompanyIds.Any(x => CompanyIdsPredicate(activity, x))) &&
                   (request.ContactIds == null || !request.ContactIds.Any() ||
                    request.ContactIds.Any(x => ContactIdsPredicate(activity, x))) &&
                   (request.DealIds == null || !request.DealIds.Any() ||
                    request.DealIds.Any(x => DealIdsPredicate(activity, x))) &&
                   (request.CreateUserIds == null || !request.CreateUserIds.Any() ||
                    request.CreateUserIds.Any(x => CreateUserIdsPredicate(activity, x))) &&
                   (request.ResponsibleUserIds == null || !request.ResponsibleUserIds.Any() ||
                    request.ResponsibleUserIds.Any(x => ResponsibleUserIdsPredicate(activity, x))) &&
                   (request.Attributes == null || !request.Attributes.Any() ||
                    (request.AllAttributes is false
                        ? request.Attributes.Any(x => AttributePredicate(activity, x))
                        : request.Attributes.All(x => AttributePredicate(activity, x)))) &&
                   (request.Priorities == null || !request.Priorities.Any() ||
                    request.Priorities.Any(x => PrioritiesPredicate(activity, x)));
        }

        private static bool TypeIdsPredicate(Activity activity, Guid id)
        {
            return activity.TypeId == id;
        }

        private static bool StatusIdsPredicate(Activity activity, Guid id)
        {
            return activity.StatusId == id;
        }

        private static bool LeadIdsPredicate(Activity activity, Guid id)
        {
            return activity.LeadId == id;
        }

        private static bool CompanyIdsPredicate(Activity activity, Guid id)
        {
            return activity.CompanyId == id;
        }

        private static bool ContactIdsPredicate(Activity activity, Guid id)
        {
            return activity.ContactId == id;
        }

        private static bool DealIdsPredicate(Activity activity, Guid id)
        {
            return activity.DealId == id;
        }

        private static bool CreateUserIdsPredicate(Activity activity, Guid id)
        {
            return activity.CreateUserId == id;
        }

        private static bool ResponsibleUserIdsPredicate(Activity activity, Guid id)
        {
            return activity.ResponsibleUserId == id;
        }

        private static bool AttributePredicate(Activity activity, KeyValuePair<Guid, string> pair)
        {
            var (key, value) = pair;

            return activity.AttributeLinks != null && activity.AttributeLinks.Any(x =>
                       x.ActivityAttributeId == key && (value.IsEmpty() || x.Value == value));
        }

        private static bool PrioritiesPredicate(Activity activity, ActivityPriority priority)
        {
            return activity.Priority == priority;
        }
    }
}