using Crm.Areas.Accounts.Settings;
using Crm.Areas.Accounts.Storages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Crm.Areas.Accounts.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection ConfigureAccounts(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var section = configuration.GetSection("AccountsStorageSettings");

            services.Configure<AccountsStorageSettings>(section);

            services.AddEntityFrameworkNpgsql()
               .AddDbContext<AccountsStorage>()
               .BuildServiceProvider();

            return services;
        }
    }
}