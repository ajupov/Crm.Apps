using System.Linq;
using System.Threading.Tasks;
using Crm.Apps.Tests.Creator;
using Crm.Clients.Activities.Clients;
using Crm.Clients.Activities.Models;
using Crm.Utils.DateTime;
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
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            var statusId = (await _create.ActivityStatus.WithAccountId(account.Id).BuildAsync().ConfigureAwait(false))
                .Id;

            var status = await _activityStatusesClient.GetAsync(statusId).ConfigureAwait(false);

            Assert.NotNull(status);
            Assert.Equal(statusId, status.Id);
        }

        [Fact]
        public async Task WhenGetList_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            var statusIds = (await Task.WhenAll(
                    _create.ActivityStatus.WithAccountId(account.Id).WithName("Test1").BuildAsync(),
                    _create.ActivityStatus.WithAccountId(account.Id).WithName("Test2").BuildAsync())
                .ConfigureAwait(false)).Select(x => x.Id).ToList();

            var statuses = await _activityStatusesClient.GetListAsync(statusIds).ConfigureAwait(false);

            Assert.NotEmpty(statuses);
            Assert.Equal(statusIds.Count, statuses.Count);
        }

        [Fact]
        public async Task WhenGetPagedList_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            await Task.WhenAll(_create.ActivityStatus.WithAccountId(account.Id).WithName("Test1").BuildAsync())
                .ConfigureAwait(false);

            var statuses = await _activityStatusesClient
                .GetPagedListAsync(account.Id, "Test1", sortBy: "CreateDateTime", orderBy: "desc")
                .ConfigureAwait(false);

            var results = statuses.Skip(1).Zip(statuses,
                (previous, current) => current.CreateDateTime >= previous.CreateDateTime);

            Assert.NotEmpty(statuses);
            Assert.All(results, Assert.True);
        }

        [Fact]
        public async Task WhenCreate_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            var status = new ActivityStatus
            {
                AccountId = account.Id,
                Name = "Test",
                IsDeleted = false
            };

            var createdStatusId = await _activityStatusesClient.CreateAsync(status).ConfigureAwait(false);

            var createdStatus = await _activityStatusesClient.GetAsync(createdStatusId).ConfigureAwait(false);

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
            var status = await _create.ActivityStatus.WithAccountId(account.Id).WithName("Test1").BuildAsync()
                .ConfigureAwait(false);

            status.Name = "Test2";
            status.IsDeleted = true;

            await _activityStatusesClient.UpdateAsync(status).ConfigureAwait(false);

            var updatedStatus = await _activityStatusesClient.GetAsync(status.Id).ConfigureAwait(false);

            Assert.Equal(status.Name, updatedStatus.Name);
            Assert.Equal(status.IsDeleted, updatedStatus.IsDeleted);
        }

        [Fact]
        public async Task WhenDelete_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            var statusIds = (await Task.WhenAll(
                    _create.ActivityStatus.WithAccountId(account.Id).WithName("Test1").BuildAsync(),
                    _create.ActivityStatus.WithAccountId(account.Id).WithName("Test2").BuildAsync())
                .ConfigureAwait(false)).Select(x => x.Id).ToList();

            await _activityStatusesClient.DeleteAsync(statusIds).ConfigureAwait(false);

            var statuses = await _activityStatusesClient.GetListAsync(statusIds).ConfigureAwait(false);

            Assert.All(statuses, x => Assert.True(x.IsDeleted));
        }

        [Fact]
        public async Task WhenRestore_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            var statusIds = (await Task.WhenAll(
                    _create.ActivityStatus.WithAccountId(account.Id).WithName("Test1").BuildAsync(),
                    _create.ActivityStatus.WithAccountId(account.Id).WithName("Test2").BuildAsync())
                .ConfigureAwait(false)).Select(x => x.Id).ToList();

            await _activityStatusesClient.RestoreAsync(statusIds).ConfigureAwait(false);

            var statuses = await _activityStatusesClient.GetListAsync(statusIds).ConfigureAwait(false);

            Assert.All(statuses, x => Assert.False(x.IsDeleted));
        }
    }
}