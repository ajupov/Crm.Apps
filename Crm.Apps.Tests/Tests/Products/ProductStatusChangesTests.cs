using System.Linq;
using System.Threading.Tasks;
using Ajupov.Utils.All.DateTime;
using Ajupov.Utils.All.Json;
using Crm.Apps.Clients.Products.Clients;
using Crm.Apps.Clients.Products.Models;
using Crm.Apps.Tests.Creator;
using Xunit;

namespace Crm.Apps.Tests.Tests.Products
{
    public class ProductStatusChangesTests
    {
        private readonly ICreate _create;
        private readonly IProductStatusesClient _productStatusesClient;
        private readonly IProductStatusChangesClient _productStatusChangesClient;

        public ProductStatusChangesTests(ICreate create, IProductStatusesClient productStatusesClient,
            IProductStatusChangesClient productStatusChangesClient)
        {
            _create = create;
            _productStatusesClient = productStatusesClient;
            _productStatusChangesClient = productStatusChangesClient;
        }

        [Fact]
        public async Task WhenGetPagedList_ThenSuccess()
        {
            
            var status = await _create.ProductStatus.BuildAsync();
            status.Name = "Test2";
            status.IsDeleted = true;
            await _productStatusesClient.UpdateAsync(status);

            var changes = await _productStatusChangesClient
                .GetPagedListAsync(statusId: status.Id, sortBy: "CreateDateTime", orderBy: "asc")
                ;

            Assert.NotEmpty(changes);
            Assert.True(changes.All(x => !x.ChangerUserId.IsEmpty()));
            Assert.True(changes.All(x => x.StatusId == status.Id));
            Assert.True(changes.All(x => x.CreateDateTime.IsMoreThanMinValue()));
            Assert.True(changes.First().OldValueJson.IsEmpty());
            Assert.True(!changes.First().NewValueJson.IsEmpty());
            Assert.NotNull(changes.First().NewValueJson.FromJsonString<ProductStatus>());
            Assert.True(!changes.Last().OldValueJson.IsEmpty());
            Assert.True(!changes.Last().NewValueJson.IsEmpty());
            Assert.False(changes.Last().OldValueJson.FromJsonString<ProductStatus>().IsDeleted);
            Assert.True(changes.Last().NewValueJson.FromJsonString<ProductStatus>().IsDeleted);
            Assert.Equal(changes.Last().NewValueJson.FromJsonString<ProductStatus>().Name, status.Name);
        }
    }
}