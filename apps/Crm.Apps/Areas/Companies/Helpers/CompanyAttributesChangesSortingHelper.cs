using System.Linq;
using Crm.Apps.Areas.Companies.Models;

namespace Crm.Apps.Areas.Companies.Helpers
{
    public static class CompanyAttributesChangesSortingHelper
    {
        public static IOrderedQueryable<CompanyAttributeChange> Sort(this IQueryable<CompanyAttributeChange> queryable,
            string sortBy, string orderBy)
        {
            var isDesc = orderBy == "desc";

            switch (sortBy)
            {
                case nameof(CompanyAttributeChange.Id):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.Id)
                        : queryable.OrderBy(x => x.Id);
                case nameof(CompanyAttributeChange.CreateDateTime):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.CreateDateTime)
                        : queryable.OrderBy(x => x.CreateDateTime);
                default:
                    return queryable.OrderByDescending(x => x.CreateDateTime);
            }
        }
    }
}