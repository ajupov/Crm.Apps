using System.Linq;
using Crm.Apps.Areas.Deals.Models;

namespace Crm.Apps.Areas.Deals.Helpers
{
    public static class DealsSortingHelper
    {
        public static IOrderedQueryable<Deal> Sort(this IQueryable<Deal> queryable, string sortBy, string orderBy)
        {
            var isDesc = orderBy == "desc";

            switch (sortBy)
            {
                case nameof(Deal.Id):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.Id)
                        : queryable.OrderBy(x => x.Id);
                case nameof(Deal.Name):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.Name)
                        : queryable.OrderBy(x => x.Name);
                case nameof(Deal.StartDateTime):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.StartDateTime)
                        : queryable.OrderBy(x => x.StartDateTime);
                case nameof(Deal.EndDateTime):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.EndDateTime)
                        : queryable.OrderBy(x => x.EndDateTime);
                case nameof(Deal.Sum):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.Sum)
                        : queryable.OrderBy(x => x.Sum);
                case nameof(Deal.SumWithoutDiscount):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.SumWithoutDiscount)
                        : queryable.OrderBy(x => x.SumWithoutDiscount);
                case nameof(Deal.FinishProbability):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.FinishProbability)
                        : queryable.OrderBy(x => x.FinishProbability);
                case nameof(Deal.CreateDateTime):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.CreateDateTime)
                        : queryable.OrderBy(x => x.CreateDateTime);
                default:
                    return queryable.OrderByDescending(x => x.CreateDateTime);
            }
        }
    }
}