using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;

namespace Crm.Apps.Infrastructure.TokensProtector
{
    public static class TokensProtectorExtensions
    {
        public static IServiceCollection AddTokensProtection(this IServiceCollection services)
        {
            var path = Path.Join(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "data-protection");

            services
                .AddDataProtection()
                .PersistKeysToFileSystem(new DirectoryInfo(path));

            return services;
        }
    }
}