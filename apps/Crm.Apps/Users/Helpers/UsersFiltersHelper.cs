using System;
using System.Collections.Generic;
using System.Linq;
using Ajupov.Utils.All.String;
using Crm.Apps.Users.Models;
using Crm.Apps.Users.RequestParameters;
using Crm.Common.UserContext;

namespace Crm.Apps.Users.Helpers
{
    public static class UsersFiltersHelper
    {
        public static bool FilterByAdditional(this User user, UserGetPagedListRequestParameter request)
        {
            return (request.Attributes == null || !request.Attributes.Any() ||
                    (request.AllAttributes is false
                        ? request.Attributes.Any(x => AttributePredicate(user, x))
                        : request.Attributes.All(x => AttributePredicate(user, x)))) &&
                   (request.Permissions == null || !request.Permissions.Any() ||
                    (request.AllPermissions is false
                        ? request.Permissions.Any(x => PermissionPredicate(user, x))
                        : request.Permissions.All(x => PermissionPredicate(user, x)))) &&
                   (request.GroupIds == null || !request.GroupIds.Any() ||
                    (request.AllGroupIds is false
                        ? request.GroupIds.Any(x => GroupIdPredicate(user, x))
                        : request.GroupIds.All(x => GroupIdPredicate(user, x))));
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