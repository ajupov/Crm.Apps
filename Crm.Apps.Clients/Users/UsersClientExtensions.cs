using Crm.Clients.Users.Clients;
using Crm.Clients.Users.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Crm.Clients.Users
{
    public static class UsersClientExtensions
    {
        public static IServiceCollection ConfigureUsersClient(this IServiceCollection services,
            IConfiguration configuration)
        {
            return services
                .AddHttpClient()
                .Configure<UsersClientSettings>(configuration.GetSection("UsersClientSettings"))
                .AddSingleton<IUsersClient, UsersClient>()
                .AddSingleton<IUserChangesClient, UserChangesClient>()
                .AddSingleton<IUserAttributesClient, UserAttributesClient>()
                .AddSingleton<IUserAttributeChangesClient, UserAttributeChangesClient>()
                .AddSingleton<IUserGroupsClient, UserGroupsClient>()
                .AddSingleton<IUserGroupChangesClient, UserGroupChangesClient>()
                .AddSingleton<IUserSettingsClient, UserSettingsClient>();
        }
    }
}