using Crm.Clients.Users.Clients.UserAttributeChanges;
using Crm.Clients.Users.Clients.UserAttributes;
using Crm.Clients.Users.Clients.UserChanges;
using Crm.Clients.Users.Clients.UserGroupChanges;
using Crm.Clients.Users.Clients.UserGroups;
using Crm.Clients.Users.Clients.Users;
using Crm.Clients.Users.Clients.UserSettings;
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