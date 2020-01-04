using Crm.Clients.Contacts.Clients;
using Crm.Clients.Contacts.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Crm.Clients.Contacts
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