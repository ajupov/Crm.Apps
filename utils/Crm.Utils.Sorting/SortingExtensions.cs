using System.Linq;
using System.Linq.Expressions;

namespace Crm.Utils.Sorting
{
    public static class SortingHelper
    {
        public static IQueryable<T> Sort<T>(this IQueryable<T> queryable, string sortBy, string orderBy)
        {
            var type = typeof(T);
            var property = type.GetProperty(sortBy);
            var parameter = Expression.Parameter(type, "p");
            var memberAccess = Expression.MakeMemberAccess(parameter, property);
            var expression = Expression.Lambda(memberAccess, parameter);
            var method = orderBy == "desc" ? "OrderByDescending" : "OrderBy";

            var resultExp = Expression.Call(typeof(Queryable), method,
                new[] {type, property.PropertyType}, queryable.Expression, Expression.Quote(expression));

            return queryable.Provider.CreateQuery<T>(resultExp);
        }
    }
}