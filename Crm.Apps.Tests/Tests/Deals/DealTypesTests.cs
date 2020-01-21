using System.Linq;
using System.Threading.Tasks;
using Ajupov.Utils.All.DateTime;
using Crm.Apps.Tests.Creator;
using Crm.Apps.v1.Clients.Deals.Clients;
using Crm.Apps.v1.Clients.Deals.Models;
using Crm.Apps.v1.Clients.Deals.RequestParameters;
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
            var typeId = (await _create.DealType.BuildAsync()).Id;

            var type = await _dealTypesClient.GetAsync(typeId);

            Assert.NotNull(type);
            Assert.Equal(typeId, type.Id);
        }

        [Fact]
        public async Task WhenGetList_ThenSuccess()
        {
            var typeIds = (
                    await Task.WhenAll(
                        _create.DealType
                            .WithName("Test1")
                            .BuildAsync(),
                        _create.DealType
                            .WithName("Test2")
                            .BuildAsync())
                )
                .Select(x => x.Id)
                .ToList();

            var types = await _dealTypesClient.GetListAsync(typeIds);

            Assert.NotEmpty(types);
            Assert.Equal(typeIds.Count, types.Count);
        }

        [Fact]
        public async Task WhenGetPagedList_ThenSuccess()
        {
            await Task.WhenAll(_create.DealType.WithName("Test1").BuildAsync());

            var request = new DealTypeGetPagedListRequestParameter
            {
                Name = "Test1"
            };

            var types = await _dealTypesClient.GetPagedListAsync(request);

            var results = types
                .Skip(1)
                .Zip(types, (previous, current) => current.CreateDateTime >= previous.CreateDateTime);

            Assert.NotEmpty(types);
            Assert.All(results, Assert.True);
        }

        [Fact]
        public async Task WhenCreate_ThenSuccess()
        {
            var type = new DealType
            {
                Name = "Test",
                IsDeleted = false
            };

            var createdTypeId = await _dealTypesClient.CreateAsync(type);

            var createdType = await _dealTypesClient.GetAsync(createdTypeId);

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
            var type = await _create.DealType
                .WithName("Test1")
                .BuildAsync();

            type.Name = "Test2";
            type.IsDeleted = true;

            await _dealTypesClient.UpdateAsync(type);

            var updatedType = await _dealTypesClient.GetAsync(type.Id);

            Assert.Equal(type.Name, updatedType.Name);
            Assert.Equal(type.IsDeleted, updatedType.IsDeleted);
        }

        [Fact]
        public async Task WhenDelete_ThenSuccess()
        {
            var typeIds = (
                    await Task.WhenAll(
                        _create.DealType
                            .WithName("Test1")
                            .BuildAsync(),
                        _create.DealType
                            .WithName("Test2")
                            .BuildAsync())
                )
                .Select(x => x.Id)
                .ToList();

            await _dealTypesClient.DeleteAsync(typeIds);

            var types = await _dealTypesClient.GetListAsync(typeIds);

            Assert.All(types, x => Assert.True(x.IsDeleted));
        }

        [Fact]
        public async Task WhenRestore_ThenSuccess()
        {
            var typeIds = (
                    await Task.WhenAll(
                        _create.DealType
                            .WithName("Test1")
                            .BuildAsync(),
                        _create.DealType
                            .WithName("Test2")
                            .BuildAsync())
                )
                .Select(x => x.Id)
                .ToList();

            await _dealTypesClient.RestoreAsync(typeIds);

            var types = await _dealTypesClient.GetListAsync(typeIds);

            Assert.All(types, x => Assert.False(x.IsDeleted));
        }
    }
}