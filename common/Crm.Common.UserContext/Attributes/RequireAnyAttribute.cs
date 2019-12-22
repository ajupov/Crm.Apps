using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Crm.Common.UserContext.Attributes
{
    public class RequireAnyAttribute : Attribute, IAsyncAuthorizationFilter
    {
        private readonly Role[] _roles;

        public RequireAnyAttribute(params Role[] roles)
        {
            _roles = roles;
        }

        public Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
//            var cache = context.HttpContext.RequestServices.GetService<>();
//            throw new NotImplementedException();

            return Task.CompletedTask;
        }
    }
}