using Ajupov.Infrastructure.All.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace Crm.Apps.UserContext.Attributes.Roles
{
    public class RequireProductsRoleAttribute : AuthorizeAttribute
    {
        public RequireProductsRoleAttribute()
        {
            Roles = Common.All.Roles.Roles.Products;
            AuthenticationSchemes = $"{JwtBearerDefaults.AuthenticationScheme},{JwtDefaults.AuthenticationScheme}";
        }
    }
}