using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

namespace Crm.Apps.Auth.ExtractAccessToken
{
    public class ExtractAccessTokenMiddleware
    {
        private readonly RequestDelegate _next;

        public ExtractAccessTokenMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Headers.ContainsKey("Authorization"))
            {
                return _next(context);
            }

            if (!context.Request.Cookies.TryGetValue("access_token", out var accessToken))
            {
                return _next(context);
            }

            var authorizationHeaderValue = $"{JwtBearerDefaults.AuthenticationScheme} {accessToken}";
            context.Request.Headers.Add(HeaderNames.Authorization, authorizationHeaderValue);

            return _next(context);
        }
    }
}