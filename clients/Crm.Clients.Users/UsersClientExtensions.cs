using Crm.Clients.Users.Clients.UserAttributeLinks;
using Crm.Clients.Users.Clients.UserAttributes;
using Crm.Clients.Users.Clients.UserGroupLinks;
using Crm.Clients.Users.Clients.UserGroups;
using Crm.Clients.Users.Clients.UserPostLinks;
using Crm.Clients.Users.Clients.UserPosts;
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
        public static IServiceCollection ConfigureAccountsClient(this IServiceCollection services,
            IConfiguration configuration)
        {
            return services
                .AddHttpClient()
                .Configure<UsersClientSettings>(configuration.GetSection("UsersClientSettings"))
                .AddSingleton<IUsersDefaultClient, UsersDefaultClient>()
                .AddSingleton<IUsersClient, UsersClient>()
                .AddSingleton<IUserAttributesClient, UserAttributesClient>()
                .AddSingleton<IUserAttributeLinksClient, IUserAttributeLinksClient>()
                .AddSingleton<IUserGroupsClient, UserGroupsClient>()
                .AddSingleton<IUserGroupLinksClient, IUserGroupLinksClient>()
                .AddSingleton<IUserPostsClient, UserPostsClient>()
                .AddSingleton<IUserPostLinksClient, IUserPostLinksClient>()
                .AddSingleton<IUsersSettingsClient, UsersSettingsClient>();
        }
    }
}