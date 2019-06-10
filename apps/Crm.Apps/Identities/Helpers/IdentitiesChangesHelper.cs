using System;
using Crm.Apps.Identities.Models;
using Crm.Utils.Json;

namespace Crm.Apps.Identities.Helpers
{
    public static class IdentitiesChangesHelper
    {
        public static IdentityChange WithCreateLog(this Identity identity, Guid userId, Action<Identity> action)
        {
            action(identity);

            return new IdentityChange
            {
                IdentityId = identity.Id,
                ChangerUserId = userId,
                CreateDateTime = DateTime.UtcNow,
                OldValueJson = string.Empty,
                NewValueJson = identity.ToJsonString()
            };
        }

        public static IdentityChange WithUpdateLog(this Identity identity, Guid userId, Action<Identity> action)
        {
            var oldValueJson = identity.ToJsonString();

            action(identity);

            return new IdentityChange
            {
                IdentityId = identity.Id,
                ChangerUserId = userId,
                CreateDateTime = DateTime.UtcNow,
                OldValueJson = oldValueJson,
                NewValueJson = identity.ToJsonString()
            };
        }
    }
}