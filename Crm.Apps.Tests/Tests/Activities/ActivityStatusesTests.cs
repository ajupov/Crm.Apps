using System.Linq;
using System.Threading.Tasks;
using Ajupov.Utils.All.DateTime;
using Crm.Apps.Clients.Activities.Clients;
using Crm.Apps.Clients.Activities.Models;
using Crm.Apps.Clients.Activities.RequestParameters;
using Crm.Apps.Tests.Creator;
using Xunit;

namespace Crm.Apps.Tests.Tests.Activities
{
    public class ActivityStatusesTests
    {
        private readonly ICreate _create;
        private readonly IActivityStatusesClient _activityStatusesClient;

        public ActivityStatusesTests(ICreate create, IActivityStatusesClient activityStatusesClient)
        {
            _create = create;
            _activityStatusesClient = activityStatusesClient;
        }

        [Fact]
        public async Task WhenGet_ThenSuccess()
        {
            var statusId = (await _create.ActivityStatus.BuildAsync()).Id;

            var status = await _activityStatusesClient.GetAsync(statusId);

            Assert.NotNull(status);
            Assert.Equal(statusId, status.Id);
        }

        [Fact]
        public async Task WhenGetList_ThenSuccess()
        {
            var statusIds = (
                    await Task.WhenAll(
                        _create.ActivityStatus
                            .WithName("Test1")
                            .BuildAsync(),
                        _create.ActivityStatus
                            .WithName("Test2")
                            .BuildAsync())
                )
                .Select(x => x.Id)
                .ToList();

            var statuses = await _activityStatusesClient.GetListAsync(statusIds);

            Assert.NotEmpty(statuses);
            Assert.Equal(statusIds.Count, statuses.Count);
        }

        [Fact]
        public async Task WhenGetPagedList_ThenSuccess()
        {
            await Task.WhenAll(_create.ActivityStatus.WithName("Test1").BuildAsync());

            var request = new ActivityStatusGetPagedListRequestParameter
            {
                Name = "Test1",
                SortBy = "CreateDateTime",
                OrderBy = "desc"
            };

            var statuses = await _activityStatusesClient.GetPagedListAsync(request);

            var results = statuses
                .Skip(1)
                .Zip(statuses, (previous, current) => current.CreateDateTime >= previous.CreateDateTime);

            Assert.NotEmpty(statuses);
            Assert.All(results, Assert.True);
        }

        [Fact]
        public async Task WhenCreate_ThenSuccess()
        {
            var status = new ActivityStatus
            {
                Name = "Test",
                IsDeleted = false
            };

            var createdStatusId = await _activityStatusesClient.CreateAsync(status);

            var createdStatus = await _activityStatusesClient.GetAsync(createdStatusId);

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
            var status = await _create.ActivityStatus.WithName("Test1").BuildAsync();

            status.Name = "Test2";
            status.IsDeleted = true;

            await _activityStatusesClient.UpdateAsync(status);

            var updatedStatus = await _activityStatusesClient.GetAsync(status.Id);

            Assert.Equal(status.Name, updatedStatus.Name);
            Assert.Equal(status.IsDeleted, updatedStatus.IsDeleted);
        }

        [Fact]
        public async Task WhenDelete_ThenSuccess()
        {
            var statusIds = (
                    await Task.WhenAll(
                        _create.ActivityStatus
                            .WithName("Test1")
                            .BuildAsync(),
                        _create.ActivityStatus
                            .WithName("Test2")
                            .BuildAsync())
                )
                .Select(x => x.Id)
                .ToList();

            await _activityStatusesClient.DeleteAsync(statusIds);

            var statuses = await _activityStatusesClient.GetListAsync(statusIds);

            Assert.All(statuses, x => Assert.True(x.IsDeleted));
        }

        [Fact]
        public async Task WhenRestore_ThenSuccess()
        {
            var statusIds = (
                    await Task.WhenAll(
                        _create.ActivityStatus
                            .WithName("Test1")
                            .BuildAsync(),
                        _create.ActivityStatus
                            .WithName("Test2")
                            .BuildAsync())
                )
                .Select(x => x.Id)
                .ToList();

            await _activityStatusesClient.RestoreAsync(statusIds);

            var statuses = await _activityStatusesClient.GetListAsync(statusIds);

            Assert.All(statuses, x => Assert.False(x.IsDeleted));
        }
    }
}