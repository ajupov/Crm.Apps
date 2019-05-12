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

        bool HasAny(params Permission[] permissions);

        bool HasAll(params Permission[] permissions);

        bool Belongs(IEnumerable<Guid> accountIds);
        
        bool Belongs(params Guid[] accountIds);
    }
}