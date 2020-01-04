using System.Linq;
using System.Threading.Tasks;
using Crm.Apps.Tests.Creator;
using Crm.Clients.Activities.Clients;
using Crm.Clients.Activities.RequestParameters;
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
            var account = await _create.Account.BuildAsync();
            var statusId = (await _create.ActivityStatus.WithAccountId(account.Id).BuildAsync())
                .Id;

            var status = await _activityStatusesClient.GetAsync(statusId);

            Assert.NotNull(status);
            Assert.Equal(statusId, status.Id);
        }

        [Fact]
        public async Task WhenGetList_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync();
            var statusIds = (await Task.WhenAll(
                    _create.ActivityStatus.WithAccountId(account.Id).WithName("Test1").BuildAsync(),
                    _create.ActivityStatus.WithAccountId(account.Id).WithName("Test2").BuildAsync())
                )
                .Select(x => x.Id)
                .ToArray();

            var statuses = await _activityStatusesClient.GetListAsync(statusIds);

            Assert.NotEmpty(statuses);
            Assert.Equal(statusIds.Length, statuses.Length);
        }

        [Fact]
        public async Task WhenGetPagedList_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync();
            await Task.WhenAll(_create.ActivityStatus.WithAccountId(account.Id).WithName("Test1").BuildAsync());

            var request = new ActivityStatusGetPagedListRequest
            {
                AccountId = account.Id,
                Name = "Test1",
                SortBy = "CreateDateTime",
                OrderBy = "desc"
            };

            var statuses = await _activityStatusesClient.GetPagedListAsync(request);

            var results = statuses
                .Skip(1)
                .Zip(statuses,
                    (previous, current) => current.CreateDateTime >= previous.CreateDateTime);

            Assert.NotEmpty(statuses);
            Assert.All(results, Assert.True);
        }

        [Fact]
        public async Task WhenCreate_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync();
            var status = new ActivityStatusCreateRequest
            {
                AccountId = account.Id,
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
            var account = await _create.Account.BuildAsync();
            var status = await _create.ActivityStatus.WithAccountId(account.Id).WithName("Test1").BuildAsync();

            var request = new ActivityStatusUpdateRequest
            {
                Id = status.Id,
                Name = "Test2",
                IsDeleted = true
            };

            await _activityStatusesClient.UpdateAsync(request);

            var updatedStatus = await _activityStatusesClient.GetAsync(status.Id);

            Assert.Equal(request.Name, updatedStatus.Name);
            Assert.Equal(request.IsDeleted, updatedStatus.IsDeleted);
        }

        [Fact]
        public async Task WhenDelete_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync();
            var statusIds = (await Task.WhenAll(
                    _create.ActivityStatus.WithAccountId(account.Id).WithName("Test1").BuildAsync(),
                    _create.ActivityStatus.WithAccountId(account.Id).WithName("Test2").BuildAsync())
                )
                .Select(x => x.Id)
                .ToArray();

            await _activityStatusesClient.DeleteAsync(statusIds);

            var statuses = await _activityStatusesClient.GetListAsync(statusIds);

            Assert.All(statuses, x => Assert.True(x.IsDeleted));
        }

        [Fact]
        public async Task WhenRestore_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync();
            var statusIds = (await Task.WhenAll(
                    _create.ActivityStatus.WithAccountId(account.Id).WithName("Test1").BuildAsync(),
                    _create.ActivityStatus.WithAccountId(account.Id).WithName("Test2").BuildAsync())
                )
                .Select(x => x.Id)
                .ToArray();

            await _activityStatusesClient.RestoreAsync(statusIds);

            var statuses = await _activityStatusesClient.GetListAsync(statusIds);

            Assert.All(statuses, x => Assert.False(x.IsDeleted));
        }
    }
}