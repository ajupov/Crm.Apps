using System.Linq;
using Crm.Apps.Areas.Deals.Models;

namespace Crm.Apps.Areas.Deals.Helpers
{
    public static class DealAttributesSortingHelper
    {
        public static IOrderedQueryable<DealAttribute> Sort(this IQueryable<DealAttribute> queryable,
            string sortBy, string orderBy)
        {
            var isDesc = orderBy == "desc";

            switch (sortBy)
            {
                case nameof(DealAttribute.Id):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.Id)
                        : queryable.OrderBy(x => x.Id);
                case nameof(DealAttribute.Type):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.Type)
                        : queryable.OrderBy(x => x.Type);
                case nameof(DealAttribute.Key):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.Key)
                        : queryable.OrderBy(x => x.Key);
                case nameof(DealAttribute.CreateDateTime):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.CreateDateTime)
                        : queryable.OrderBy(x => x.CreateDateTime);
                default:
                    return queryable.OrderByDescending(x => x.CreateDateTime);
            }
        }
    }
}