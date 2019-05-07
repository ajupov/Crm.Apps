using Crm.Clients.Users.Clients.UserAttributeLinks;
using Crm.Clients.Users.Clients.UserAttributes;
using Crm.Clients.Users.Clients.UserGroupLinks;
using Crm.Clients.Users.Clients.UserGroups;
using Crm.Clients.Users.Clients.Users;
using Crm.Clients.Users.Clients.UsersDefault;
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
                .AddSingleton<IUsersDefaultClient, UsersDefaultClient>()
                .AddSingleton<IUsersClient, UsersClient>()
                .AddSingleton<IUserAttributesClient, UserAttributesClient>()
                .AddSingleton<IUserAttributeLinksClient, UserAttributeLinksClient>()
                .AddSingleton<IUserGroupsClient, UserGroupsClient>()
                .AddSingleton<IUserGroupLinksClient, UserGroupLinksClient>()
                .AddSingleton<IUsersSettingsClient, UsersSettingsClient>();
        }
    }
}