﻿using System.Linq;
using Crm.Apps.Areas.Users.Models;

namespace Crm.Apps.Areas.Users.Helpers
{
    public static class UserAttributesChangesSortingHelper
    {
        public static IOrderedQueryable<UserAttributeChange> Sort(this IQueryable<UserAttributeChange> queryable,
            string sortBy,
            string orderBy)
        {
            var isDesc = orderBy == "desc";
            sortBy = sortBy.Substring(0, 1).ToUpper() + sortBy.Substring(1);

            switch (sortBy)
            {
                case nameof(UserAttributeChange.Id):
                    return isDesc
                        ? queryable.OrderByDescending(change => change.Id)
                        : queryable.OrderBy(change => change.Id);
                case nameof(UserAttributeChange.CreateDateTime):
                    return isDesc
                        ? queryable.OrderByDescending(change => change.CreateDateTime)
                        : queryable.OrderBy(change => change.CreateDateTime);
                default:
                    return queryable.OrderByDescending(change => change.CreateDateTime);
            }
        }
    }
}