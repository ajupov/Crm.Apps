using System.Linq;
using System.Threading.Tasks;
using Ajupov.Utils.All.DateTime;
using Ajupov.Utils.All.Guid;
using Ajupov.Utils.All.Json;
using Ajupov.Utils.All.String;
using Crm.Apps.Tests.Creator;
using Crm.Apps.v1.Clients.Products.Clients;
using Crm.Apps.v1.Clients.Products.Models;
using Crm.Apps.v1.Clients.Products.RequestParameters;
using Crm.Common.All.Types.AttributeType;
using Xunit;

namespace Crm.Apps.Tests.Tests.Products
{
    public class ProductAttributeChangesTests
    {
        private readonly ICreate _create;
        private readonly IProductAttributesClient _productAttributesClient;
        private readonly IProductAttributeChangesClient _attributeChangesClient;

        public ProductAttributeChangesTests(
            ICreate create,
            IProductAttributesClient productAttributesClient,
            IProductAttributeChangesClient attributeChangesClient)
        {
            _create = create;
            _productAttributesClient = productAttributesClient;
            _attributeChangesClient = attributeChangesClient;
        }

        [Fact]
        public async Task WhenGetPagedList_ThenSuccess()
        {
            var attribute = await _create.ProductAttribute.BuildAsync();

            attribute.Type = AttributeType.Link;
            attribute.Key = "TestLink";
            attribute.IsDeleted = true;

            await _productAttributesClient.UpdateAsync(attribute);

            var request = new ProductAttributeChangeGetPagedListRequestParameter
            {
                AttributeId = attribute.Id,
                SortBy = "CreateDateTime",
                OrderBy = "asc"
            };

            var changes = await _attributeChangesClient.GetPagedListAsync(request);

            Assert.NotEmpty(changes);
            Assert.True(changes.All(x => !x.ChangerUserId.IsEmpty()));
            Assert.True(changes.All(x => x.AttributeId == attribute.Id));
            Assert.True(changes.All(x => x.CreateDateTime.IsMoreThanMinValue()));
            Assert.True(changes.First().OldValueJson.IsEmpty());
            Assert.True(!changes.First().NewValueJson.IsEmpty());
            Assert.NotNull(changes.First().NewValueJson.FromJsonString<ProductAttribute>());
            Assert.True(!changes.Last().OldValueJson.IsEmpty());
            Assert.True(!changes.Last().NewValueJson.IsEmpty());
            Assert.False(changes.Last().OldValueJson.FromJsonString<ProductAttribute>().IsDeleted);
            Assert.True(changes.Last().NewValueJson.FromJsonString<ProductAttribute>().IsDeleted);
            Assert.Equal(changes.Last().NewValueJson.FromJsonString<ProductAttribute>().Key, attribute.Key);
            Assert.Equal(changes.Last().NewValueJson.FromJsonString<ProductAttribute>().Type, attribute.Type);
        }
    }
}