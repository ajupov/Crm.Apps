using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Crm.Common.UserContext
{
    public class UserContext : IUserContext
    {
        public UserContext(IHttpContextAccessor httpContextAccessor)
        {
            var claims = httpContextAccessor.HttpContext.User.Claims;
            var headers = httpContextAccessor.HttpContext.Request.Headers;

            UserId = Guid.NewGuid();
            AccountId = Guid.NewGuid();
            Name = Guid.NewGuid().ToString();
            AvatarUrl = Guid.NewGuid().ToString();
            Roles = new List<Role>
            {
                Role.Administration
            };
        }

        public Guid UserId { get; }

        public Guid AccountId { get; }

        public string Name { get; }

        public string AvatarUrl { get; }

        public List<Role> Roles { get; }

        public bool HasAny(params Role[] permissions)
        {
            return permissions.Intersect(Roles).Any();
        }

        public bool HasAll(params Role[] permissions)
        {
            return !permissions.Except(Roles).Any();
        }

        public bool Belongs(IEnumerable<Guid> accountIds)
        {
            return accountIds.All(x => x == AccountId);
        }
        
        public bool Belongs(params Guid[] accountIds)
        {
            return accountIds.All(x => x == AccountId);
        }
    }
}