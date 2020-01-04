using System;
using System.Threading.Tasks;
using Crm.Clients.Products.Clients;
using Crm.Clients.Products.Models;
using Crm.Utils.Guid;

namespace Crm.Apps.Tests.Builders.Products
{
    public class ProductCategoryBuilder : IProductCategoryBuilder
    {
        private readonly IProductCategoriesClient _productCategoriesClient;
        private readonly ProductCategory _productCategory;

        public ProductCategoryBuilder(IProductCategoriesClient productCategoriesClient)
        {
            _productCategoriesClient = productCategoriesClient;
            _productCategory = new ProductCategory
            {
                AccountId = Guid.Empty,
                Name = "Test"
            };
        }

        public ProductCategoryBuilder WithAccountId(Guid accountId)
        {
            _productCategory.AccountId = accountId;

            return this;
        }

        public ProductCategoryBuilder WithName(string name)
        {
            _productCategory.Name = name;

            return this;
        }

        public async Task<ProductCategory> BuildAsync()
        {
            if (_productCategory.AccountId.IsEmpty())
            {
                throw new InvalidOperationException(nameof(_productCategory.AccountId));
            }

            var createdId = await _productCategoriesClient.CreateAsync(_productCategory);

            return await _productCategoriesClient.GetAsync(createdId);
        }
    }
}