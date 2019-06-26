using System.Linq;
using Crm.Apps.Companies.Models;

namespace Crm.Apps.Companies.Helpers
{
    public static class CompanyCommentsSortingHelper
    {
        public static IOrderedQueryable<CompanyComment> Sort(this IQueryable<CompanyComment> queryable, string sortBy,
            string orderBy)
        {
            var isDesc = orderBy == "desc";

            switch (sortBy)
            {
                case nameof(CompanyComment.Id):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.Id)
                        : queryable.OrderBy(x => x.Id);
                case nameof(CompanyComment.CreateDateTime):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.CreateDateTime)
                        : queryable.OrderBy(x => x.CreateDateTime);
                default:
                    return queryable.OrderByDescending(x => x.CreateDateTime);
            }
        }
    }
}