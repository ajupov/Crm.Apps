using System;
using System.Collections.Generic;
using System.Linq;
using Ajupov.Utils.All.String;
using Crm.Apps.Leads.Models;
using Crm.Apps.Leads.V1.Requests;

namespace Crm.Apps.Leads.Helpers
{
    public static class LeadsFiltersHelper
    {
        public static bool FilterByAdditional(this Lead lead, LeadGetPagedListRequest request)
        {
            return (request.SourceIds == null || !request.SourceIds.Any() ||
                    request.SourceIds.Any(x => SourceIdsPredicate(lead, x))) &&
                   (request.CreateUserIds == null || !request.CreateUserIds.Any() ||
                    request.CreateUserIds.Any(x => CreateUserIdsPredicate(lead, x))) &&
                   (request.ResponsibleUserIds == null || !request.ResponsibleUserIds.Any() ||
                    request.ResponsibleUserIds.Any(x => ResponsibleUserIdsPredicate(lead, x))) &&
                   (request.Attributes == null || !request.Attributes.Any() ||
                    (request.AllAttributes is false
                        ? request.Attributes.Any(x => AttributePredicate(lead, x))
                        : request.Attributes.All(x => AttributePredicate(lead, x))));
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
