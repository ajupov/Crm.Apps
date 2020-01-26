using System.Linq;
using System.Threading.Tasks;
using Crm.Apps.Tests.Extensions;
using Crm.Apps.Tests.Services.AccessTokenGetter;
using Crm.Apps.Tests.Services.Creator;
using Crm.Apps.v1.Clients.Products.Clients;
using Crm.Apps.v1.Clients.Products.Models;
using Xunit;

namespace Crm.Apps.Tests.Tests.MyTest
{
    public class MyTests
    {
        private readonly IAccessTokenGetter _accessTokenGetter;
        private readonly ICreate _create;
        private readonly IProductsClient _productsClient;

        public MyTests(IAccessTokenGetter accessTokenGetter, ICreate create, IProductsClient productsClient)
        {
            _accessTokenGetter = accessTokenGetter;
            _create = create;
            _productsClient = productsClient;
        }

        [Fact]
        public async Task WhenUpdate_ThenSuccess()
        {
            var accessToken = await _accessTokenGetter.GetAsync();

            var status = await _create.ProductStatus.BuildAsync();
            var attribute = await _create.ProductAttribute.BuildAsync();

            var product = await _create.Product
                .WithStatusId(status.Id)
                .WithAttributeLink(attribute.Id, "1")
                .BuildAsync();

            product.AttributeLinks
                .Where(x => x.ProductAttributeId == attribute.Id)
                .ToList()
                .ForEach(x => x.Value = "2");

            await _productsClient.UpdateAsync(accessToken, product);

            var updatedProduct = await _productsClient.GetAsync(accessToken, product.Id);

            Assert.Equal(
                product.AttributeLinks.Single().ProductAttributeId,
                updatedProduct.AttributeLinks.Single().ProductAttributeId);

            Assert.Equal(
                product.AttributeLinks.Single().Value,
                updatedProduct.AttributeLinks.Single().Value);
        }
    }
}