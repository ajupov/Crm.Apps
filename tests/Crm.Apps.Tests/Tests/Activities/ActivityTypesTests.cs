using System.Linq;
using System.Threading.Tasks;
using Crm.Apps.Tests.Creator;
using Crm.Clients.Activities.Clients;
using Crm.Clients.Activities.RequestParameters;
using Crm.Utils.DateTime;
using Xunit;

namespace Crm.Apps.Tests.Tests.Activities
{
    public class ActivityTypesTests
    {
        private readonly ICreate _create;
        private readonly IActivityTypesClient _activityTypesClient;

        public ActivityTypesTests(ICreate create, IActivityTypesClient activityTypesClient)
        {
            _create = create;
            _activityTypesClient = activityTypesClient;
        }

        [Fact]
        public async Task WhenGet_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync();
            var typeId = (await _create.ActivityType.WithAccountId(account.Id).BuildAsync()).Id;

            var type = await _activityTypesClient.GetAsync(typeId);

            Assert.NotNull(type);
            Assert.Equal(typeId, type.Id);
        }

        [Fact]
        public async Task WhenGetList_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync();
            var typeIds = (await Task.WhenAll(
                    _create.ActivityType.WithAccountId(account.Id).WithName("Test1").BuildAsync(),
                    _create.ActivityType.WithAccountId(account.Id).WithName("Test2").BuildAsync())
                )
                .Select(x => x.Id)
                .ToArray();

            var types = await _activityTypesClient.GetListAsync(typeIds);

            Assert.NotEmpty(types);
            Assert.Equal(typeIds.Length, types.Length);
        }

        [Fact]
        public async Task WhenGetPagedList_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync();
            await Task.WhenAll(_create.ActivityType.WithAccountId(account.Id).WithName("Test1").BuildAsync());

            var request = new ActivityTypeGetPagedListRequest
            {
                AccountId = account.Id, 
                Name = "Test1", 
                SortBy = "CreateDateTime", 
                OrderBy = "asc"
            };

            var types = await _activityTypesClient.GetPagedListAsync(request);

            var results = types
                .Skip(1)
                .Zip(types,
                (previous, current) => current.CreateDateTime >= previous.CreateDateTime);

            Assert.NotEmpty(types);
            Assert.All(results, Assert.True);
        }

        [Fact]
        public async Task WhenCreate_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync();
            var request = new ActivityTypeCreateRequest
            {
                AccountId = account.Id,
                Name = "Test",
                IsDeleted = false
            };

            var createdTypeId = await _activityTypesClient.CreateAsync(request);

            var createdType = await _activityTypesClient.GetAsync(createdTypeId);

            Assert.NotNull(createdType);
            Assert.Equal(createdTypeId, createdType.Id);
            Assert.Equal(request.AccountId, createdType.AccountId);
            Assert.Equal(request.Name, createdType.Name);
            Assert.Equal(request.IsDeleted, createdType.IsDeleted);
            Assert.True(createdType.CreateDateTime.IsMoreThanMinValue());
        }

        [Fact]
        public async Task WhenUpdate_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync();
            var type = await _create.ActivityType.WithAccountId(account.Id).WithName("Test1").BuildAsync();

            var request = new ActivityTypeUpdateRequest
            {
                Id = type.Id,
                Name = "Test2",
                IsDeleted = true
            };

            await _activityTypesClient.UpdateAsync(request);

            var updatedType = await _activityTypesClient.GetAsync(type.Id);

            Assert.Equal(request.Name, updatedType.Name);
            Assert.Equal(request.IsDeleted, updatedType.IsDeleted);
        }

        [Fact]
        public async Task WhenDelete_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync();
            var typeIds = (await Task.WhenAll(
                    _create.ActivityType.WithAccountId(account.Id).WithName("Test1").BuildAsync(),
                    _create.ActivityType.WithAccountId(account.Id).WithName("Test2").BuildAsync())
                )
                .Select(x => x.Id)
                .ToArray();

            await _activityTypesClient.DeleteAsync(typeIds);

            var types = await _activityTypesClient.GetListAsync(typeIds);

            Assert.All(types, x => Assert.True(x.IsDeleted));
        }

        [Fact]
        public async Task WhenRestore_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync();
            var typeIds = (await Task.WhenAll(
                    _create.ActivityType.WithAccountId(account.Id).WithName("Test1").BuildAsync(),
                    _create.ActivityType.WithAccountId(account.Id).WithName("Test2").BuildAsync())
                )
                .Select(x => x.Id)
                .ToArray();

            await _activityTypesClient.RestoreAsync(typeIds);

            var types = await _activityTypesClient.GetListAsync(typeIds);

            Assert.All(types, x => Assert.False(x.IsDeleted));
        }
    }
}