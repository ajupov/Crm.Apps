using System.Linq;
using Crm.Apps.Leads.Models;

namespace Crm.Apps.Leads.Helpers
{
    public static class LeadCommentsSortingHelper
    {
        public static IOrderedQueryable<LeadComment> Sort(this IQueryable<LeadComment> queryable, string sortBy,
            string orderBy)
        {
            var isDesc = orderBy == "desc";

            switch (sortBy)
            {
                case nameof(LeadComment.Id):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.Id)
                        : queryable.OrderBy(x => x.Id);
                case nameof(LeadComment.CreateDateTime):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.CreateDateTime)
                        : queryable.OrderBy(x => x.CreateDateTime);
                default:
                    return queryable.OrderByDescending(x => x.CreateDateTime);
            }
        }
    }
}