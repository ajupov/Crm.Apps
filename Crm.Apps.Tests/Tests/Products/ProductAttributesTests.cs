using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ajupov.Utils.All.DateTime;
using Crm.Apps.Tests.Creator;
using Crm.Apps.v1.Clients.Products.Clients;
using Crm.Apps.v1.Clients.Products.Models;
using Crm.Apps.v1.Clients.Products.RequestParameters;
using Crm.Common.All.Types.AttributeType;
using Xunit;

namespace Crm.Apps.Tests.Tests.Products
{
    public class ProductAttributesTests
    {
        private readonly ICreate _create;
        private readonly IProductAttributesClient _productAttributesClient;

        public ProductAttributesTests(ICreate create, IProductAttributesClient productAttributesClient)
        {
            _create = create;
            _productAttributesClient = productAttributesClient;
        }

        [Fact]
        public async Task WhenGetTypes_ThenSuccess()
        {
            var types = await _productAttributesClient.GetTypesAsync();

            Assert.NotEmpty(types);
        }

        [Fact]
        public async Task WhenGet_ThenSuccess()
        {
            var attributeId = (await _create.ProductAttribute.BuildAsync()).Id;

            var attribute = await _productAttributesClient.GetAsync(attributeId);

            Assert.NotNull(attribute);
            Assert.Equal(attributeId, attribute.Id);
        }

        [Fact]
        public async Task WhenGetList_ThenSuccess()
        {
            var attributeIds = (
                    await Task.WhenAll(
                        _create.ProductAttribute
                            .WithKey("Test1")
                            .BuildAsync(),
                        _create.ProductAttribute
                            .WithKey("Test2")
                            .BuildAsync())
                )
                .Select(x => x.Id)
                .ToList();

            var attributes = await _productAttributesClient.GetListAsync(attributeIds);

            Assert.NotEmpty(attributes);
            Assert.Equal(attributeIds.Count, attributes.Count);
        }

        [Fact]
        public async Task WhenGetPagedList_ThenSuccess()
        {
            await Task.WhenAll(
                _create.ProductAttribute
                    .WithType(AttributeType.Text)
                    .WithKey("Test1")
                    .BuildAsync());
            var filterTypes = new List<AttributeType> {AttributeType.Text};

            var request = new ProductAttributeGetPagedListRequestParameter
            {
                Key = "Test1",
                Types = filterTypes,
            };

            var attributes = await _productAttributesClient.GetPagedListAsync(request);

            var results = attributes
                .Skip(1)
                .Zip(attributes, (previous, current) => current.CreateDateTime >= previous.CreateDateTime);

            Assert.NotEmpty(attributes);
            Assert.All(results, Assert.True);
        }

        [Fact]
        public async Task WhenCreate_ThenSuccess()
        {
            var attribute = new ProductAttribute
            {
                Type = AttributeType.Text,
                Key = "Test",
                IsDeleted = false
            };

            var createdAttributeId = await _productAttributesClient.CreateAsync(attribute);

            var createdAttribute = await _productAttributesClient.GetAsync(createdAttributeId);

            Assert.NotNull(createdAttribute);
            Assert.Equal(createdAttributeId, createdAttribute.Id);
            Assert.Equal(attribute.AccountId, createdAttribute.AccountId);
            Assert.Equal(attribute.Type, createdAttribute.Type);
            Assert.Equal(attribute.Key, createdAttribute.Key);
            Assert.Equal(attribute.IsDeleted, createdAttribute.IsDeleted);
            Assert.True(createdAttribute.CreateDateTime.IsMoreThanMinValue());
        }

        [Fact]
        public async Task WhenUpdate_ThenSuccess()
        {
            var attribute = await _create.ProductAttribute
                .WithType(AttributeType.Text)
                .WithKey("Test")
                .BuildAsync();

            attribute.Type = AttributeType.Link;
            attribute.Key = "test.com";
            attribute.IsDeleted = true;

            await _productAttributesClient.UpdateAsync(attribute);

            var updatedAttribute = await _productAttributesClient.GetAsync(attribute.Id);

            Assert.Equal(attribute.Type, updatedAttribute.Type);
            Assert.Equal(attribute.Key, updatedAttribute.Key);
            Assert.Equal(attribute.IsDeleted, updatedAttribute.IsDeleted);
        }

        [Fact]
        public async Task WhenDelete_ThenSuccess()
        {
            var attributeIds = (
                    await Task.WhenAll(
                        _create.ProductAttribute
                            .WithKey("Test1")
                            .BuildAsync(),
                        _create.ProductAttribute
                            .WithKey("Test2")
                            .BuildAsync())
                )
                .Select(x => x.Id)
                .ToList();

            await _productAttributesClient.DeleteAsync(attributeIds);

            var attributes = await _productAttributesClient.GetListAsync(attributeIds);

            Assert.All(attributes, x => Assert.True(x.IsDeleted));
        }

        [Fact]
        public async Task WhenRestore_ThenSuccess()
        {
            var attributeIds = (
                    await Task.WhenAll(
                        _create.ProductAttribute
                            .WithKey("Test1")
                            .BuildAsync(),
                        _create.ProductAttribute
                            .WithKey("Test2")
                            .BuildAsync())
                )
                .Select(x => x.Id)
                .ToList();

            await _productAttributesClient.RestoreAsync(attributeIds);

            var attributes = await _productAttributesClient.GetListAsync(attributeIds);

            Assert.All(attributes, x => Assert.False(x.IsDeleted));
        }
    }
}