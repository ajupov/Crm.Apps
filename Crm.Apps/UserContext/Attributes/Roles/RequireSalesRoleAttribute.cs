using Ajupov.Infrastructure.All.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace Crm.Apps.UserContext.Attributes.Roles
{
    public class RequireSalesRoleAttribute : AuthorizeAttribute
    {
        public RequireSalesRoleAttribute()
        {
            Roles = Common.All.Roles.Roles.Sales;
            AuthenticationSchemes = $"{JwtBearerDefaults.AuthenticationScheme},{JwtDefaults.Scheme}";
        }
    }
}