using System;
using System.Threading.Tasks;
using Crm.Apps.Clients.Products.Clients;
using Crm.Apps.Clients.Products.Models;

namespace Crm.Apps.Tests.Builders.Products
{
    public class ProductCategoryBuilder : IProductCategoryBuilder
    {
        private readonly IProductCategoriesClient _productCategoriesClient;
        private readonly ProductCategory _category;

        public ProductCategoryBuilder(IProductCategoriesClient productCategoriesClient)
        {
            _productCategoriesClient = productCategoriesClient;
            _category = new ProductCategory
            {
                AccountId = Guid.Empty,
                Name = "Test",
                IsDeleted = false
            };
        }

        public ProductCategoryBuilder WithName(string name)
        {
            _category.Name = name;

            return this;
        }

        public ProductCategoryBuilder IsDeleted()
        {
            _category.IsDeleted = true;

            return this;
        }

        public async Task<ProductCategory> BuildAsync()
        {
            var id = await _productCategoriesClient.CreateAsync(_category);

            return await _productCategoriesClient.GetAsync(id);
        }
    }
}