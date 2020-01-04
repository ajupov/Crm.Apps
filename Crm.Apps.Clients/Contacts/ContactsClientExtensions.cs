using Crm.Apps.Clients.Contacts.Clients;
using Crm.Apps.Clients.Contacts.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Crm.Apps.Clients.Contacts
{
    public static class ContactsClientExtensions
    {
        public static IServiceCollection ConfigureContactsClient(this IServiceCollection services,
            IConfiguration configuration)
        {
            return services
                .AddHttpClient()
                .Configure<ContactsClientSettings>(configuration.GetSection("ContactsClientSettings"))
                .AddSingleton<IContactsClient, ContactsClient>()
                .AddSingleton<IContactChangesClient, ContactChangesClient>()
                .AddSingleton<IContactAttributesClient, ContactAttributesClient>()
                .AddSingleton<IContactAttributeChangesClient, ContactAttributeChangesClient>()
                .AddSingleton<IContactCommentsClient, ContactCommentsClient>();
        }
    }
}