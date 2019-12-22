using System;
using System.Collections.Generic;
using System.Linq;
using Ajupov.Utils.All.String;
using Crm.Apps.Areas.Leads.Models;
using Crm.Apps.Areas.Leads.Parameters;

namespace Crm.Apps.Areas.Leads.Helpers
{
    public static class LeadsFiltersHelper
    {
        public static bool FilterByAdditional(this Lead lead, LeadGetPagedListParameter parameter)
        {
            return (parameter.SourceIds == null || !parameter.SourceIds.Any() ||
                    parameter.SourceIds.Any(x => SourceIdsPredicate(lead, x))) &&
                   (parameter.CreateUserIds == null || !parameter.CreateUserIds.Any() ||
                    parameter.CreateUserIds.Any(x => CreateUserIdsPredicate(lead, x))) &&
                   (parameter.ResponsibleUserIds == null || !parameter.ResponsibleUserIds.Any() ||
                    parameter.ResponsibleUserIds.Any(x => ResponsibleUserIdsPredicate(lead, x))) &&
                   (parameter.Attributes == null || !parameter.Attributes.Any() ||
                    (parameter.AllAttributes is false
                        ? parameter.Attributes.Any(x => AttributePredicate(lead, x))
                        : parameter.Attributes.All(x => AttributePredicate(lead, x))));
        }

        private static bool SourceIdsPredicate(Lead lead, Guid id)
        {
            return lead.SourceId == id;
        }

        private static bool CreateUserIdsPredicate(Lead lead, Guid id)
        {
            return lead.CreateUserId == id;
        }

        private static bool ResponsibleUserIdsPredicate(Lead lead, Guid id)
        {
            return lead.ResponsibleUserId == id;
        }

        private static bool AttributePredicate(Lead lead, KeyValuePair<Guid, string> pair)
        {
            var (key, value) = pair;

            return lead.AttributeLinks != null && lead.AttributeLinks.Any(x =>
                       x.LeadAttributeId == key && (value.IsEmpty() || x.Value == value));
        }
    }
}