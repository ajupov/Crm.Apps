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

        List<Role> Roles { get; }

        bool HasAny(params Role[] permissions);

        bool HasAll(params Role[] permissions);

        bool Belongs(IEnumerable<Guid> accountIds);
        
        bool Belongs(params Guid[] accountIds);
    }
}