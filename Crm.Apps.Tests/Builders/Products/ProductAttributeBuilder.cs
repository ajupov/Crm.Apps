using System;
using System.Threading.Tasks;
using Crm.Apps.Tests.Services.AccessTokenGetter;
using Crm.Apps.v1.Clients.Products.Clients;
using Crm.Apps.v1.Clients.Products.Models;
using Crm.Common.All.Types.AttributeType;

namespace Crm.Apps.Tests.Builders.Products
{
    public class ProductAttributeBuilder : IProductAttributeBuilder
    {
        private readonly IAccessTokenGetter _accessTokenGetter;
        private readonly IProductAttributesClient _productAttributesClient;
        private readonly ProductAttribute _attribute;

        public ProductAttributeBuilder(
            IAccessTokenGetter accessTokenGetter,
            IProductAttributesClient productAttributesClient)
        {
            _productAttributesClient = productAttributesClient;
            _accessTokenGetter = accessTokenGetter;
            _attribute = new ProductAttribute
            {
                AccountId = Guid.Empty,
                Type = AttributeType.Text,
                Key = "Test",
                IsDeleted = false
            };
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
            var accessToken = await _accessTokenGetter.GetAsync();

            var id = await _productAttributesClient.CreateAsync(accessToken, _attribute);

            return await _productAttributesClient.GetAsync(accessToken, id);
        }
    }
}