using System.Linq;
using System.Threading.Tasks;
using Crm.Apps.Tests.Creator;
using Crm.Clients.Deals.Clients;
using Crm.Clients.Deals.Models;
using Crm.Utils.DateTime;
using Xunit;

namespace Crm.Apps.Tests.Tests.Deals
{
    public class DealTypesTests
    {
        private readonly ICreate _create;
        private readonly IDealTypesClient _dealTypesClient;

        public DealTypesTests(ICreate create, IDealTypesClient dealTypesClient)
        {
            _create = create;
            _dealTypesClient = dealTypesClient;
        }

        [Fact]
        public async Task WhenGet_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            var typeId = (await _create.DealType.WithAccountId(account.Id).BuildAsync().ConfigureAwait(false)).Id;

            var type = await _dealTypesClient.GetAsync(typeId).ConfigureAwait(false);

            Assert.NotNull(type);
            Assert.Equal(typeId, type.Id);
        }

        [Fact]
        public async Task WhenGetList_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            var typeIds = (await Task.WhenAll(
                    _create.DealType.WithAccountId(account.Id).WithName("Test1").BuildAsync(),
                    _create.DealType.WithAccountId(account.Id).WithName("Test2").BuildAsync())
                .ConfigureAwait(false)).Select(x => x.Id).ToList();

            var types = await _dealTypesClient.GetListAsync(typeIds).ConfigureAwait(false);

            Assert.NotEmpty(types);
            Assert.Equal(typeIds.Count, types.Count);
        }

        [Fact]
        public async Task WhenGetPagedList_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            await Task.WhenAll(_create.DealType.WithAccountId(account.Id).WithName("Test1").BuildAsync())
                .ConfigureAwait(false);

            var types = await _dealTypesClient
                .GetPagedListAsync(account.Id, "Test1", sortBy: "CreateDateTime", orderBy: "desc")
                .ConfigureAwait(false);

            var results = types.Skip(1).Zip(types,
                (previous, current) => current.CreateDateTime >= previous.CreateDateTime);

            Assert.NotEmpty(types);
            Assert.All(results, Assert.True);
        }

        [Fact]
        public async Task WhenCreate_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            var type = new DealType
            {
                AccountId = account.Id,
                Name = "Test",
                IsDeleted = false
            };

            var createdTypeId = await _dealTypesClient.CreateAsync(type).ConfigureAwait(false);

            var createdType = await _dealTypesClient.GetAsync(createdTypeId).ConfigureAwait(false);

            Assert.NotNull(createdType);
            Assert.Equal(createdTypeId, createdType.Id);
            Assert.Equal(type.AccountId, createdType.AccountId);
            Assert.Equal(type.Name, createdType.Name);
            Assert.Equal(type.IsDeleted, createdType.IsDeleted);
            Assert.True(createdType.CreateDateTime.IsMoreThanMinValue());
        }

        [Fact]
        public async Task WhenUpdate_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            var type = await _create.DealType.WithAccountId(account.Id).WithName("Test1").BuildAsync()
                .ConfigureAwait(false);

            type.Name = "Test2";
            type.IsDeleted = true;

            await _dealTypesClient.UpdateAsync(type).ConfigureAwait(false);

            var updatedType = await _dealTypesClient.GetAsync(type.Id).ConfigureAwait(false);

            Assert.Equal(type.Name, updatedType.Name);
            Assert.Equal(type.IsDeleted, updatedType.IsDeleted);
        }

        [Fact]
        public async Task WhenDelete_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            var typeIds = (await Task.WhenAll(
                    _create.DealType.WithAccountId(account.Id).WithName("Test1").BuildAsync(),
                    _create.DealType.WithAccountId(account.Id).WithName("Test2").BuildAsync())
                .ConfigureAwait(false)).Select(x => x.Id).ToList();

            await _dealTypesClient.DeleteAsync(typeIds).ConfigureAwait(false);

            var types = await _dealTypesClient.GetListAsync(typeIds).ConfigureAwait(false);

            Assert.All(types, x => Assert.True(x.IsDeleted));
        }

        [Fact]
        public async Task WhenRestore_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            var typeIds = (await Task.WhenAll(
                    _create.DealType.WithAccountId(account.Id).WithName("Test1").BuildAsync(),
                    _create.DealType.WithAccountId(account.Id).WithName("Test2").BuildAsync())
                .ConfigureAwait(false)).Select(x => x.Id).ToList();

            await _dealTypesClient.RestoreAsync(typeIds).ConfigureAwait(false);

            var types = await _dealTypesClient.GetListAsync(typeIds).ConfigureAwait(false);

            Assert.All(types, x => Assert.False(x.IsDeleted));
        }
    }
}