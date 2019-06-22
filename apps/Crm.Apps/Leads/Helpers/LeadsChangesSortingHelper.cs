using System.Linq;
using Crm.Apps.Leads.Models;

namespace Crm.Apps.Leads.Helpers
{
    public static class LeadsChangesSortingHelper
    {
        public static IOrderedQueryable<LeadChange> Sort(this IQueryable<LeadChange> queryable, string sortBy,
            string orderBy)
        {
            var isDesc = orderBy == "desc";

            switch (sortBy)
            {
                case nameof(LeadChange.Id):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.Id)
                        : queryable.OrderBy(x => x.Id);
                case nameof(LeadChange.CreateDateTime):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.CreateDateTime)
                        : queryable.OrderBy(x => x.CreateDateTime);
                default:
                    return queryable.OrderByDescending(x => x.CreateDateTime);
            }
        }
    }
}