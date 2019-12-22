using System;
using System.Collections.Generic;
using System.Linq;
using Ajupov.Utils.All.String;
using Crm.Apps.Areas.Users.Models;
using Crm.Apps.Areas.Users.Parameters;
using Crm.Common.UserContext;

namespace Crm.Apps.Areas.Users.Helpers
{
    public static class UsersFiltersHelper
    {
        public static bool FilterByAdditional(this User user, UserGetPagedListParameter parameter)
        {
            return (parameter.Attributes == null || !parameter.Attributes.Any() ||
                    (parameter.AllAttributes is false
                        ? parameter.Attributes.Any(x => AttributePredicate(user, x))
                        : parameter.Attributes.All(x => AttributePredicate(user, x)))) &&
                   (parameter.Permissions == null || !parameter.Permissions.Any() ||
                    (parameter.AllPermissions is false
                        ? parameter.Permissions.Any(x => PermissionPredicate(user, x))
                        : parameter.Permissions.All(x => PermissionPredicate(user, x)))) &&
                   (parameter.GroupIds == null || !parameter.GroupIds.Any() ||
                    (parameter.AllGroupIds is false
                        ? parameter.GroupIds.Any(x => GroupIdPredicate(user, x))
                        : parameter.GroupIds.All(x => GroupIdPredicate(user, x))));
        }

        private static bool AttributePredicate(User user, KeyValuePair<Guid, string> pair)
        {
            var (key, value) = pair;

            return user.AttributeLinks != null && user.AttributeLinks.Any(x =>
                       x.UserAttributeId == key && (value.IsEmpty() || x.Value == value));
        }

        private static bool PermissionPredicate(User user, Role role)
        {
            return user.Permissions != null && user.Permissions.Any(x => x.Role == role);
        }

        private static bool GroupIdPredicate(User user, Guid id)
        {
            return user.GroupLinks != null && user.GroupLinks.Any(x => x.UserGroupId == id);
        }
    }
}