using Ajupov.Infrastructure.All.Jwt;
using Microsoft.AspNetCore.Authorization;

namespace Crm.Apps.UserContext.Attributes.Roles
{
    public class RequireLeadsRoleAttribute : AuthorizeAttribute
    {
        public RequireLeadsRoleAttribute()
        {
            Roles = Common.All.Roles.Roles.Leads;
            AuthenticationSchemes = JwtDefaults.Scheme;
        }
    }
}