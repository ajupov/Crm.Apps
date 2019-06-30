using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Crm.Apps.Tests.Creator;
using Crm.Clients.Products.Clients;
using Crm.Clients.Products.Models;
using Crm.Utils.DateTime;
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
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            var statusId = (await _create.ProductStatus.WithAccountId(account.Id).BuildAsync().ConfigureAwait(false))
                .Id;

            var status = await _productStatusesClient.GetAsync(statusId).ConfigureAwait(false);

            Assert.NotNull(status);
            Assert.Equal(statusId, status.Id);
        }

        [Fact]
        public async Task WhenGetList_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            var statusIds = (await Task.WhenAll(
                    _create.ProductStatus.WithAccountId(account.Id).WithName("Test1").BuildAsync(),
                    _create.ProductStatus.WithAccountId(account.Id).WithName("Test2").BuildAsync())
                .ConfigureAwait(false)).Select(x => x.Id).ToList();

            var status = await _productStatusesClient.GetListAsync(statusIds).ConfigureAwait(false);

            Assert.NotEmpty(status);
            Assert.Equal(statusIds.Count, status.Count);
        }

        [Fact]
        public async Task WhenGetPagedList_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            await Task.WhenAll(_create.ProductStatus.WithAccountId(account.Id).WithName("Test1").BuildAsync())
                .ConfigureAwait(false);

            var status = await _productStatusesClient
                .GetPagedListAsync(account.Id, "Test1", sortBy: "CreateDateTime", orderBy: "desc")
                .ConfigureAwait(false);

            var results = status.Skip(1).Zip(status,
                (previous, current) => current.CreateDateTime >= previous.CreateDateTime);

            Assert.NotEmpty(status);
            Assert.All(results, Assert.True);
        }

        [Fact]
        public async Task WhenCreate_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            var status = new ProductStatus
            {
                AccountId = account.Id,
                Name = "Test",
                IsDeleted = false
            };

            var createdStatusId = await _productStatusesClient.CreateAsync(status).ConfigureAwait(false);

            var createdStatus = await _productStatusesClient.GetAsync(createdStatusId).ConfigureAwait(false);

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
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            var status = await _create.ProductStatus.WithAccountId(account.Id).WithName("Test1").BuildAsync()
                .ConfigureAwait(false);

            status.Name = "Test2";
            status.IsDeleted = true;

            await _productStatusesClient.UpdateAsync(status).ConfigureAwait(false);

            var updatedStatus = await _productStatusesClient.GetAsync(status.Id).ConfigureAwait(false);

            Assert.Equal(status.Name, updatedStatus.Name);
            Assert.Equal(status.IsDeleted, updatedStatus.IsDeleted);
        }

        [Fact]
        public async Task WhenDelete_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            var statusIds = (await Task.WhenAll(
                    _create.ProductStatus.WithAccountId(account.Id).WithName("Test1").BuildAsync(),
                    _create.ProductStatus.WithAccountId(account.Id).WithName("Test2").BuildAsync())
                .ConfigureAwait(false)).Select(x => x.Id).ToList();

            await _productStatusesClient.DeleteAsync(statusIds).ConfigureAwait(false);

            var statuses = await _productStatusesClient.GetListAsync(statusIds).ConfigureAwait(false);

            Assert.All(statuses, x => Assert.True(x.IsDeleted));
        }

        [Fact]
        public async Task WhenRestore_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            var statusIds = (await Task.WhenAll(
                    _create.ProductStatus.WithAccountId(account.Id).WithName("Test1").BuildAsync(),
                    _create.ProductStatus.WithAccountId(account.Id).WithName("Test2").BuildAsync())
                .ConfigureAwait(false)).Select(x => x.Id).ToList();

            await _productStatusesClient.RestoreAsync(statusIds).ConfigureAwait(false);

            var statuses = await _productStatusesClient.GetListAsync(statusIds).ConfigureAwait(false);

            Assert.All(statuses, x => Assert.False(x.IsDeleted));
        }
    }
}