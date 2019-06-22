using System.Linq;
using Crm.Apps.Leads.Models;

namespace Crm.Apps.Leads.Helpers
{
    public static class LeadSourcesSortingHelper
    {
        public static IOrderedQueryable<LeadSource> Sort(this IQueryable<LeadSource> queryable, string sortBy,
            string orderBy)
        {
            var isDesc = orderBy == "desc";

            switch (sortBy)
            {
                case nameof(LeadSource.Id):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.Id)
                        : queryable.OrderBy(x => x.Id);
                case nameof(LeadSource.Name):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.Name)
                        : queryable.OrderBy(x => x.Name);
                case nameof(LeadSource.CreateDateTime):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.CreateDateTime)
                        : queryable.OrderBy(x => x.CreateDateTime);
                default:
                    return queryable.OrderByDescending(x => x.CreateDateTime);
            }
        }
    }
}