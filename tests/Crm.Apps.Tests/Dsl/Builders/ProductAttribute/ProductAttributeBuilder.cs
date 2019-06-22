using System;
using System.Threading.Tasks;
using Crm.Clients.Products.Clients;
using Crm.Common.Types;
using Crm.Utils.Guid;

namespace Crm.Apps.Tests.Dsl.Builders.ProductAttribute
{
    public class ProductAttributeBuilder : IProductAttributeBuilder
    {
        private readonly Clients.Products.Models.ProductAttribute _productAttribute;
        private readonly IProductAttributesClient _productAttributesClient;

        public ProductAttributeBuilder(IProductAttributesClient productAttributesClient)
        {
            _productAttributesClient = productAttributesClient;
            _productAttribute = new Clients.Products.Models.ProductAttribute
            {
                AccountId = Guid.Empty,
                Type = AttributeType.Text,
                Key = "Test"
            };
        }

        public ProductAttributeBuilder WithAccountId(Guid accountId)
        {
            _productAttribute.AccountId = accountId;

            return this;
        }

        public ProductAttributeBuilder WithType(AttributeType type)
        {
            _productAttribute.Type = type;

            return this;
        }

        public ProductAttributeBuilder WithKey(string key)
        {
            _productAttribute.Key = key;

            return this;
        }

        public async Task<Clients.Products.Models.ProductAttribute> BuildAsync()
        {
            if (_productAttribute.AccountId.IsEmpty())
            {
                throw new InvalidOperationException(nameof(_productAttribute.AccountId));
            }

            var createdId = await _productAttributesClient.CreateAsync(_productAttribute).ConfigureAwait(false);

            return await _productAttributesClient.GetAsync(createdId).ConfigureAwait(false);
        }
    }
}