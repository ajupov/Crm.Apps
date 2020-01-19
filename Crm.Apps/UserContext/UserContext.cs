using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Ajupov.Infrastructure.All.Jwt;
using Crm.Common.All.UserContext;
using Microsoft.AspNetCore.Http;

namespace Crm.Apps.UserContext
{
    public class UserContext : IUserContext
    {
        public UserContext(IHttpContextAccessor httpContextAccessor)
        {
            var id = httpContextAccessor.HttpContext.User.Claims
                .FirstOrDefault(x => x.Type == JwtDefaults.IdentifierClaimType || x.Type == ClaimTypes.NameIdentifier)?
                .Value;

            if (Guid.TryParse(id, out var parsedId))
            {
                UserId = parsedId;
            }

            Roles = httpContextAccessor.HttpContext.User.Claims?
                .Where(x => x.Type == ClaimTypes.Role)
                .Select(x => x.Value)
                .ToList();
        }

        public Guid UserId { get; }

        public Guid AccountId => UserId;

        public List<string> Roles { get; }
    }
}