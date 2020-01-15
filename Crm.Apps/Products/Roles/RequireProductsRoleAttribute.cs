using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace Crm.Apps.Products.Roles
{
    public class RequireProductsRoleAttribute : AuthorizeAttribute
    {
        public RequireProductsRoleAttribute()
        {
            // base.Roles = string.Join(",", ProductsRoles.Value.Select(x => x.ToString()));
        }
    }
}