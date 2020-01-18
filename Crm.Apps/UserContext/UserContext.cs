using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Ajupov.Infrastructure.All.Jwt;
using Ajupov.Infrastructure.All.Jwt.JwtReader;
using Ajupov.Utils.All.String;
using Crm.Common.All.UserContext;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

namespace Crm.Apps.UserContext
{
    public class UserContext : IUserContext
    {
        public UserContext(IHttpContextAccessor httpContextAccessor, IJwtReader jwtReader)
        {
            var header = httpContextAccessor.HttpContext.Request.Headers[HeaderNames.Authorization].FirstOrDefault();
            if (header.IsEmpty())
            {
                return;
            }

            var jwtString = header?.Substring((JwtBearerDefaults.AuthenticationScheme + " ").Length).Trim();
            if (jwtString.IsEmpty())
            {
                return;
            }

            var jwt = jwtReader.ReadAccessToken(jwtString);

            var id = jwt.Claims
                .FirstOrDefault(x => x.Type == JwtDefaults.IdentifierClaimType)?
                .Value;

            if (Guid.TryParse(id, out var parsedId))
            {
                UserId = parsedId;
            }

            Roles = jwt.Claims?
                .Where(x => x.Type == ClaimTypes.Role)
                .Select(x => x.Value)
                .ToList();
        }

        public Guid UserId { get; }

        public Guid AccountId => UserId;

        public List<string> Roles { get; }
    }
}