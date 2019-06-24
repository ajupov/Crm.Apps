using System.Linq;
using System.Threading.Tasks;
using Crm.Apps.Tests.Dsl.Creator;
using Crm.Clients.Products.Clients;
using Crm.Clients.Products.Models;
using Crm.Utils.DateTime;
using Crm.Utils.Guid;
using Crm.Utils.Json;
using Crm.Utils.String;
using Xunit;

namespace Crm.Apps.Tests.Tests.Products
{
    public class ProductChangesTests
    {
        private readonly ICreate _create;
        private readonly IProductsClient _productsClient;
        private readonly IProductChangesClient _productChangesClient;

        public ProductChangesTests(ICreate create, IProductsClient productsClient,
            IProductChangesClient productChangesClient)
        {
            _create = create;
            _productsClient = productsClient;
            _productChangesClient = productChangesClient;
        }

        [Fact]
        public async Task WhenGetPagedList_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            var status = await _create.ProductStatus.WithAccountId(account.Id).BuildAsync().ConfigureAwait(false);
            var product = await _create.Product.WithAccountId(account.Id).WithStatusId(status.Id).BuildAsync()
                .ConfigureAwait(false);
            product.IsHidden = true;
            await _productsClient.UpdateAsync(product).ConfigureAwait(false);

            var changes = await _productChangesClient
                .GetPagedListAsync(productId: product.Id, sortBy: "CreateDateTime", orderBy: "asc")
                .ConfigureAwait(false);

            Assert.NotEmpty(changes);
            Assert.True(changes.All(x => !x.ChangerUserId.IsEmpty()));
            Assert.True(changes.All(x => x.ProductId == product.Id));
            Assert.True(changes.All(x => x.CreateDateTime.IsMoreThanMinValue()));
            Assert.True(changes.First().OldValueJson.IsEmpty());
            Assert.True(!changes.First().NewValueJson.IsEmpty());
            Assert.NotNull(changes.First().NewValueJson.FromJsonString<Product>());
            Assert.True(!changes.Last().OldValueJson.IsEmpty());
            Assert.True(!changes.Last().NewValueJson.IsEmpty());
            Assert.False(changes.Last().OldValueJson.FromJsonString<Product>().IsHidden);
            Assert.True(changes.Last().NewValueJson.FromJsonString<Product>().IsHidden);
        }
    }
}