using System;
using System.Collections.Generic;
using System.Linq;
using Crm.Apps.Activities.Models;
using Crm.Apps.Activities.Parameters;
using Crm.Utils.String;

namespace Crm.Apps.Activities.Helpers
{
    public static class ActivitiesFiltersHelper
    {
        public static bool FilterByAdditional(this Activity activity, ActivityGetPagedListParameter parameter)
        {
            return
                (parameter.TypeIds == null || !parameter.TypeIds.Any() ||
                 parameter.TypeIds.Any(x => TypeIdsPredicate(activity, x))) &&
                (parameter.StatusIds == null || !parameter.StatusIds.Any() ||
                 parameter.StatusIds.Any(x => StatusIdsPredicate(activity, x))) &&
                (parameter.LeadIds == null || !parameter.LeadIds.Any() ||
                 parameter.LeadIds.Any(x => LeadIdsPredicate(activity, x))) &&
                (parameter.CompanyIds == null || !parameter.CompanyIds.Any() ||
                 parameter.CompanyIds.Any(x => CompanyIdsPredicate(activity, x))) &&
                (parameter.ContactIds == null || !parameter.ContactIds.Any() ||
                 parameter.ContactIds.Any(x => ContactIdsPredicate(activity, x))) &&
                (parameter.DealIds == null || !parameter.DealIds.Any() ||
                 parameter.DealIds.Any(x => DealIdsPredicate(activity, x))) &&
                (parameter.CreateUserIds == null || !parameter.CreateUserIds.Any() ||
                 parameter.CreateUserIds.Any(x => CreateUserIdsPredicate(activity, x))) &&
                (parameter.ResponsibleUserIds == null || !parameter.ResponsibleUserIds.Any() ||
                 parameter.ResponsibleUserIds.Any(x => ResponsibleUserIdsPredicate(activity, x))) &&
                (parameter.Attributes == null || !parameter.Attributes.Any() ||
                 (parameter.AllAttributes is false
                     ? parameter.Attributes.Any(x => AttributePredicate(activity, x))
                     : parameter.Attributes.All(x => AttributePredicate(activity, x)))) &&
                (parameter.Priorities == null || !parameter.Priorities.Any() ||
                 parameter.Priorities.Any(x => PrioritiesPredicate(activity, x)));
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