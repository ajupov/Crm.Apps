using System.Linq;
using System.Threading.Tasks;
using Ajupov.Utils.All.DateTime;
using Crm.Apps.Tests.Services.Creator;
using Crm.Apps.v1.Clients.Deals.Clients;
using Crm.Apps.v1.Clients.Deals.Models;
using Crm.Apps.v1.Clients.Deals.RequestParameters;
using Xunit;

namespace Crm.Apps.Tests.Tests.Deals
{
    public class DealStatusesTests
    {
        private readonly ICreate _create;
        private readonly IDealStatusesClient _dealStatusesClient;

        public DealStatusesTests(ICreate create, IDealStatusesClient dealStatusesClient)
        {
            _create = create;
            _dealStatusesClient = dealStatusesClient;
        }

        [Fact]
        public async Task WhenGet_ThenSuccess()
        {
            var statusId = (await _create.DealStatus.BuildAsync()).Id;

            var status = await _dealStatusesClient.GetAsync(statusId);

            Assert.NotNull(status);
            Assert.Equal(statusId, status.Id);
        }

        [Fact]
        public async Task WhenGetList_ThenSuccess()
        {
            var statusIds = (
                    await Task.WhenAll(
                        _create.DealStatus
                            .WithName("Test1")
                            .BuildAsync(),
                        _create.DealStatus
                            .WithName("Test2")
                            .BuildAsync())
                )
                .Select(x => x.Id)
                .ToList();

            var statuses = await _dealStatusesClient.GetListAsync(statusIds);

            Assert.NotEmpty(statuses);
            Assert.Equal(statusIds.Count, statuses.Count);
        }

        [Fact]
        public async Task WhenGetPagedList_ThenSuccess()
        {
            await Task.WhenAll(
                _create.DealStatus
                    .WithName("Test1")
                    .BuildAsync());

            var request = new DealStatusGetPagedListRequestParameter
            {
                Name = "Test1"
            };

            var statuses = await _dealStatusesClient.GetPagedListAsync(request);

            var results = statuses
                .Skip(1)
                .Zip(statuses, (previous, current) => current.CreateDateTime >= previous.CreateDateTime);

            Assert.NotEmpty(statuses);
            Assert.All(results, Assert.True);
        }

        [Fact]
        public async Task WhenCreate_ThenSuccess()
        {
            var status = new DealStatus
            {
                Name = "Test",
                IsDeleted = false
            };

            var createdStatusId = await _dealStatusesClient.CreateAsync(status);

            var createdStatus = await _dealStatusesClient.GetAsync(createdStatusId);

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
            var status = await _create.DealStatus
                .WithName("Test1")
                .BuildAsync();

            status.Name = "Test2";
            status.IsDeleted = true;

            await _dealStatusesClient.UpdateAsync(status);

            var updatedStatus = await _dealStatusesClient.GetAsync(status.Id);

            Assert.Equal(status.Name, updatedStatus.Name);
            Assert.Equal(status.IsDeleted, updatedStatus.IsDeleted);
        }

        [Fact]
        public async Task WhenDelete_ThenSuccess()
        {
            var statusIds = (
                    await Task.WhenAll(
                        _create.DealStatus
                            .WithName("Test1")
                            .BuildAsync(),
                        _create.DealStatus
                            .WithName("Test2")
                            .BuildAsync())
                )
                .Select(x => x.Id)
                .ToList();

            await _dealStatusesClient.DeleteAsync(statusIds);

            var statuses = await _dealStatusesClient.GetListAsync(statusIds);

            Assert.All(statuses, x => Assert.True(x.IsDeleted));
        }

        [Fact]
        public async Task WhenRestore_ThenSuccess()
        {
            var statusIds = (
                    await Task.WhenAll(
                        _create.DealStatus
                            .WithName("Test1")
                            .BuildAsync(),
                        _create.DealStatus
                            .WithName("Test2")
                            .BuildAsync())
                )
                .Select(x => x.Id)
                .ToList();

            await _dealStatusesClient.RestoreAsync(statusIds);

            var statuses = await _dealStatusesClient.GetListAsync(statusIds);

            Assert.All(statuses, x => Assert.False(x.IsDeleted));
        }
    }
}