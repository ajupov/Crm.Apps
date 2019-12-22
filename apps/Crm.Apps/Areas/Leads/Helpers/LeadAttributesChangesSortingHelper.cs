using System.Linq;
using Crm.Apps.Areas.Leads.Models;

namespace Crm.Apps.Areas.Leads.Helpers
{
    public static class LeadAttributesChangesSortingHelper
    {
        public static IOrderedQueryable<LeadAttributeChange> Sort(this IQueryable<LeadAttributeChange> queryable,
            string sortBy, string orderBy)
        {
            var isDesc = orderBy == "desc";

            switch (sortBy)
            {
                case nameof(LeadAttributeChange.Id):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.Id)
                        : queryable.OrderBy(x => x.Id);
                case nameof(LeadAttributeChange.CreateDateTime):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.CreateDateTime)
                        : queryable.OrderBy(x => x.CreateDateTime);
                default:
                    return queryable.OrderByDescending(x => x.CreateDateTime);
            }
        }
    }
}