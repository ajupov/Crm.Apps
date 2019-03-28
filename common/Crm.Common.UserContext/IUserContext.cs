using System;
using System.Collections.Generic;

namespace Crm.Common.UserContext
{
    public interface IUserContext
    {
        Guid UserId { get; }

        Guid AccountId { get; }

        string Name { get; }

        string AvatarUrl { get; }

        ICollection<Permission> Permissions { get; }
    }
}