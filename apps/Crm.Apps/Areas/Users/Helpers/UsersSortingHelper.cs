using System.Linq;
using Crm.Apps.Areas.Users.Models;

namespace Crm.Apps.Areas.Users.Helpers
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
                        ? queryable.OrderByDescending(user => user.Id)
                        : queryable.OrderBy(user => user.Id);
                case nameof(User.Surname):
                    return isDesc
                        ? queryable.OrderByDescending(user => user.Surname)
                        : queryable.OrderBy(user => user.Surname);
                case nameof(User.Name):
                    return isDesc
                        ? queryable.OrderByDescending(user => user.Name)
                        : queryable.OrderBy(user => user.Name);
                case nameof(User.Patronymic):
                    return isDesc
                        ? queryable.OrderByDescending(user => user.Patronymic)
                        : queryable.OrderBy(user => user.Patronymic);
                case nameof(User.BirthDate):
                    return isDesc
                        ? queryable.OrderByDescending(user => user.BirthDate)
                        : queryable.OrderBy(user => user.BirthDate);
                case nameof(User.CreateDateTime):
                    return isDesc
                        ? queryable.OrderByDescending(user => user.CreateDateTime)
                        : queryable.OrderBy(user => user.CreateDateTime);
                default:
                    return queryable.OrderByDescending(user => user.CreateDateTime);
            }
        }
    }
}