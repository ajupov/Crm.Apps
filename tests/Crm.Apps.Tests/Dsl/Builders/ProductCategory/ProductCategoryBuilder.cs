using System;
using System.Threading.Tasks;
using Crm.Clients.Products.Clients;
using Crm.Utils.Guid;

namespace Crm.Apps.Tests.Dsl.Builders.ProductCategory
{
    public class ProductCategoryBuilder : IProductCategoryBuilder
    {
        private readonly Clients.Products.Models.ProductCategory _productCategory;
        private readonly IProductCategoriesClient _productCategoriesClient;

        public ProductCategoryBuilder(IProductCategoriesClient productCategoriesClient)
        {
            _productCategoriesClient = productCategoriesClient;
            _productCategory = new Clients.Products.Models.ProductCategory
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

        public async Task<Clients.Products.Models.ProductCategory> BuildAsync()
        {
            if (_productCategory.AccountId.IsEmpty())
            {
                throw new InvalidOperationException(nameof(_productCategory.AccountId));
            }

            var createdId = await _productCategoriesClient.CreateAsync(_productCategory).ConfigureAwait(false);

            return await _productCategoriesClient.GetAsync(createdId).ConfigureAwait(false);
        }
    }
}