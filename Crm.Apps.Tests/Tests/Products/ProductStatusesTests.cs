using System.Linq;
using System.Threading.Tasks;
using Ajupov.Utils.All.DateTime;
using Crm.Apps.Tests.Creator;
using Crm.Apps.v1.Clients.Products.Clients;
using Crm.Apps.v1.Clients.Products.Models;
using Crm.Apps.v1.Clients.Products.RequestParameters;
using Xunit;

namespace Crm.Apps.Tests.Tests.Products
{
    public class ProductStatusesTests
    {
        private readonly ICreate _create;
        private readonly IProductStatusesClient _productStatusesClient;

        public ProductStatusesTests(ICreate create, IProductStatusesClient productStatusesClient)
        {
            _create = create;
            _productStatusesClient = productStatusesClient;
        }

        [Fact]
        public async Task WhenGet_ThenSuccess()
        {
            var statusId = (await _create.ProductStatus.BuildAsync()).Id;

            var status = await _productStatusesClient.GetAsync(statusId);

            Assert.NotNull(status);
            Assert.Equal(statusId, status.Id);
        }

        [Fact]
        public async Task WhenGetList_ThenSuccess()
        {
            var statusIds = (
                    await Task.WhenAll(
                        _create.ProductStatus
                            .WithName("Test1")
                            .BuildAsync(),
                        _create.ProductStatus
                            .WithName("Test2")
                            .BuildAsync())
                )
                .Select(x => x.Id)
                .ToList();

            var status = await _productStatusesClient.GetListAsync(statusIds);

            Assert.NotEmpty(status);
            Assert.Equal(statusIds.Count, status.Count);
        }

        [Fact]
        public async Task WhenGetPagedList_ThenSuccess()
        {
            await Task.WhenAll(
                _create.ProductStatus
                    .WithName("Test1")
                    .BuildAsync());

            var request = new ProductStatusGetPagedListRequestParameter
            {
                Name = "Test1"
            };

            var status = await _productStatusesClient.GetPagedListAsync(request);

            var results = status
                .Skip(1)
                .Zip(status, (previous, current) => current.CreateDateTime >= previous.CreateDateTime);

            Assert.NotEmpty(status);
            Assert.All(results, Assert.True);
        }

        [Fact]
        public async Task WhenCreate_ThenSuccess()
        {
            var status = new ProductStatus
            {
                Name = "Test",
                IsDeleted = false
            };

            var createdStatusId = await _productStatusesClient.CreateAsync(status);

            var createdStatus = await _productStatusesClient.GetAsync(createdStatusId);

            Assert.NotNull(createdStatus);
            Assert.Equal(createdStatusId, createdStatus.Id);
            Assert.Equal(status.AccountId, createdStatus.AccountId);
            Assert.Equal(status.Name, createdStatus.Name);
            Assert.Equal(status.IsDeleted, createdStatus.IsDeleted);
            Assert.True(createdStatus.CreateDateTime.IsMoreThanMinValue());
        }

        [Fact]
        public async Task WhenUpdate_ThenSuccess()
        {
            var status = await _create.ProductStatus
                .WithName("Test1")
                .BuildAsync();

            status.Name = "Test2";
            status.IsDeleted = true;

            await _productStatusesClient.UpdateAsync(status);

            var updatedStatus = await _productStatusesClient.GetAsync(status.Id);

            Assert.Equal(status.Name, updatedStatus.Name);
            Assert.Equal(status.IsDeleted, updatedStatus.IsDeleted);
        }

        [Fact]
        public async Task WhenDelete_ThenSuccess()
        {
            var statusIds = (
                    await Task.WhenAll(
                        _create.ProductStatus
                            .WithName("Test1")
                            .BuildAsync(),
                        _create.ProductStatus
                            .WithName("Test2")
                            .BuildAsync())
                )
                .Select(x => x.Id)
                .ToList();

            await _productStatusesClient.DeleteAsync(statusIds);

            var statuses = await _productStatusesClient.GetListAsync(statusIds);

            Assert.All(statuses, x => Assert.True(x.IsDeleted));
        }

        [Fact]
        public async Task WhenRestore_ThenSuccess()
        {
            var statusIds = (
                    await Task.WhenAll(
                        _create.ProductStatus
                            .WithName("Test1")
                            .BuildAsync(),
                        _create.ProductStatus
                            .WithName("Test2")
                            .BuildAsync())
                )
                .Select(x => x.Id)
                .ToList();

            await _productStatusesClient.RestoreAsync(statusIds);

            var statuses = await _productStatusesClient.GetListAsync(statusIds);

            Assert.All(statuses, x => Assert.False(x.IsDeleted));
        }
    }
}