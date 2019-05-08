using System;
using System.Collections.Generic;
using System.Linq;
using Crm.Apps.Areas.Users.Models;
using Crm.Common.UserContext;
using Crm.Utils.String;

namespace Crm.Apps.Areas.Users.Helpers
{
    public static class UsersFiltersHelper
    {
        public static bool FilterByAttributes(this User user, bool? isAll, IDictionary<Guid, string> attributes)
        {
            return isAll is false
                ? attributes.Any(x => AttributePredicate(user, x))
                : attributes.All(x => AttributePredicate(user, x));
        }

        public static bool FilterByPermissions(this User user, bool? isAll, IEnumerable<Permission> permissions)
        {
            return isAll is false
                ? permissions.Any(x => PermissionPredicate(user, x))
                : permissions.All(x => PermissionPredicate(user, x));
        }

        public static bool FilterByGroupIds(this User user, bool? isAll, IEnumerable<Guid> groupIds)
        {
            return isAll is false
                ? groupIds.Any(x => GroupIdPredicate(user, x))
                : groupIds.All(x => GroupIdPredicate(user, x));
        }

        private static bool AttributePredicate(User user, KeyValuePair<Guid, string> pair)
        {
            var (key, value) = pair;

            return user.AttributeLinks.Any(x => x.AttributeId == key && (value.IsEmpty() || x.Value == value));
        }

        private static bool PermissionPredicate(User user, Permission permission)
        {
            return user.Permissions.Any(x => x == permission);
        }

        private static bool GroupIdPredicate(User user, Guid id)
        {
            return user.GroupLinks.Any(x => x.GroupId == id);
        }
    }
}