using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Crm.Common.UserContext.Attributes
{
    public class RequireAnyAttribute : Attribute, IAsyncAuthorizationFilter
    {
        private readonly Permission[] _permissions;

        public RequireAnyAttribute(params Permission[] permissions)
        {
            _permissions = permissions;
        }

        public Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
//            var cache = context.HttpContext.RequestServices.GetService<>();
//            throw new NotImplementedException();

            return Task.CompletedTask;
        }
    }
}