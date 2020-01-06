using System;
using System.Threading.Tasks;
using Ajupov.Utils.All.Guid;
using Crm.Apps.Clients.Products.Clients;
using Crm.Apps.Clients.Products.Models;
using Crm.Common.All.Types.AttributeType;

namespace Crm.Apps.Tests.Builders.Products
{
    public class ProductAttributeBuilder : IProductAttributeBuilder
    {
        private readonly IProductAttributesClient _productAttributesClient;
        private readonly ProductAttribute _attribute;

        public ProductAttributeBuilder(IProductAttributesClient productAttributesClient)
        {
            _productAttributesClient = productAttributesClient;
            _attribute = new ProductAttribute
            {
                AccountId = Guid.Empty,
                Type = AttributeType.Text,
                Key = "Test",
                IsDeleted = false
            };
        }

        public ProductAttributeBuilder WithAccountId(Guid accountId)
        {
            _attribute.AccountId = accountId;

            return this;
        }

        public ProductAttributeBuilder WithType(AttributeType type)
        {
            _attribute.Type = type;

            return this;
        }

        public ProductAttributeBuilder WithKey(string key)
        {
            _attribute.Key = key;

            return this;
        }

        public ProductAttributeBuilder AsDeleted()
        {
            _attribute.IsDeleted = true;

            return this;
        }

        public async Task<ProductAttribute> BuildAsync()
        {
            var id = await _productAttributesClient.CreateAsync(_attribute);

            return await _productAttributesClient.GetAsync(id);
        }
    }
}