using System.Linq;
using Crm.Apps.Users.Models;

namespace Crm.Apps.Users.Helpers
{
    public static class UsersSortingHelper
    {
        public static IOrderedQueryable<User> Sort(this IQueryable<User> queryable, string sortBy,
            string orderBy)
        {
            var isDesc = orderBy == "desc";

            switch (sortBy)
            {
                case nameof(User.Id):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.Id)
                        : queryable.OrderBy(x => x.Id);
                case nameof(User.Surname):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.Surname)
                        : queryable.OrderBy(x => x.Surname);
                case nameof(User.Name):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.Name)
                        : queryable.OrderBy(x => x.Name);
                case nameof(User.Patronymic):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.Patronymic)
                        : queryable.OrderBy(x => x.Patronymic);
                case nameof(User.BirthDate):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.BirthDate)
                        : queryable.OrderBy(x => x.BirthDate);
                case nameof(User.CreateDateTime):
                    return isDesc
                        ? queryable.OrderByDescending(x => x.CreateDateTime)
                        : queryable.OrderBy(x => x.CreateDateTime);
                default:
                    return queryable.OrderByDescending(x => x.CreateDateTime);
            }
        }
    }
}