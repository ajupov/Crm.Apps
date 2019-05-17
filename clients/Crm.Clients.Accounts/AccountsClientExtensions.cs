using Crm.Clients.Accounts.Clients.AccountChanges;
using Crm.Clients.Accounts.Clients.Accounts;
using Crm.Clients.Accounts.Clients.AccountSettings;
using Crm.Clients.Accounts.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Crm.Clients.Accounts
{
    public static class AccountsClientExtensions
    {
        public static IServiceCollection ConfigureAccountsClient(this IServiceCollection services,
            IConfiguration configuration)
        {
            return services
                .AddHttpClient()
                .Configure<AccountsClientSettings>(configuration.GetSection("AccountsClientSettings"))
                .AddSingleton<IAccountsClient, AccountsClient>()
                .AddSingleton<IAccountSettingsClient, AccountSettingsClient>()
                .AddSingleton<IAccountChangesClient, AccountChangesClient>();
        }
    }
}