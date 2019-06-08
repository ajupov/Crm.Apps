using Crm.Clients.Products.Settings;
using Crm.Clients.Products.Clients;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Crm.Clients.Products
{
    public static class ProductsClientExtensions
    {
        public static IServiceCollection ConfigureProductsClient(this IServiceCollection services,
            IConfiguration configuration)
        {
            return services
                .AddHttpClient()
                .Configure<ProductsClientSettings>(configuration.GetSection("ProductsClientSettings"))
                .AddSingleton<IProductsClient, ProductsClient>()
                .AddSingleton<IProductChangesClient, ProductChangesClient>()
                .AddSingleton<IProductAttributesClient, ProductAttributesClient>()
                .AddSingleton<IProductAttributeChangesClient, ProductAttributeChangesClient>()
                .AddSingleton<IProductCategoriesClient, ProductCategoriesClient>()
                .AddSingleton<IProductCategoryChangesClient, ProductCategoryChangesClient>()
                .AddSingleton<IProductStatusesClient, ProductStatusesClient>()
                .AddSingleton<IProductStatusChangesClient, ProductStatusChangesClient>();
        }
    }
}