using Microsoft.AspNetCore.Builder;

namespace Crm.Apps.Auth.ExtractAccessToken
{
    public static class ExtractAccessTokenMiddlewareExtensions
    {
        public static IApplicationBuilder UseExtractAccessToken(this IApplicationBuilder builder)
        {
            return builder
                .UseMiddleware<ExtractAccessTokenMiddleware>();
        }
    }
}