using System.Linq;
using Crm.Apps.Companies.Models;

namespace Crm.Apps.Companies.Helpers
{
    public static class CompanyAttributesSortingHelper
    {
        public static IOrderedQueryable<CompanyAttribute> Sort(this IQueryable<CompanyAttribute> queryable,
            string sortBy, string orderBy)
        {
            var isDesc = orderBy == "desc";

            switch (sortBy)
            {
                case nameof(CompanyAttribute.Id):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.Id)
                        : queryable.OrderBy(x => x.Id);
                case nameof(CompanyAttribute.Type):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.Type)
                        : queryable.OrderBy(x => x.Type);
                case nameof(CompanyAttribute.Key):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.Key)
                        : queryable.OrderBy(x => x.Key);
                case nameof(CompanyAttribute.CreateDateTime):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.CreateDateTime)
                        : queryable.OrderBy(x => x.CreateDateTime);
                default:
                    return queryable.OrderByDescending(x => x.CreateDateTime);
            }
        }
    }
}