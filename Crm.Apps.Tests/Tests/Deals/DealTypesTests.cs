using System.Linq;
using System.Threading.Tasks;
using Ajupov.Utils.All.DateTime;
using Crm.Apps.Tests.Extensions;
using Crm.Apps.Tests.Services.AccessTokenGetter;
using Crm.Apps.Tests.Services.Creator;
using Crm.Apps.v1.Clients.Deals.Clients;
using Crm.Apps.v1.Clients.Deals.Models;
using Crm.Apps.v1.Clients.Deals.RequestParameters;
using Xunit;

namespace Crm.Apps.Tests.Tests.Deals
{
    public class DealTypesTests
    {
        private readonly IAccessTokenGetter _accessTokenGetter;
        private readonly ICreate _create;
        private readonly IDealTypesClient _dealTypesClient;

        public DealTypesTests(IAccessTokenGetter accessTokenGetter, ICreate create, IDealTypesClient dealTypesClient)
        {
            _accessTokenGetter = accessTokenGetter;
            _create = create;
            _dealTypesClient = dealTypesClient;
        }

        [Fact]
        public async Task WhenGet_ThenSuccess()
        {
            var accessToken = await _accessTokenGetter.GetAsync();

            var typeId = (await _create.DealType.BuildAsync()).Id;

            var type = await _dealTypesClient.GetAsync(accessToken, typeId);

            Assert.NotNull(type);
            Assert.Equal(typeId, type.Id);
        }

        [Fact]
        public async Task WhenGetList_ThenSuccess()
        {
            var accessToken = await _accessTokenGetter.GetAsync();

            var typeIds = (
                    await Task.WhenAll(
                        _create.DealType
                            .WithName("Test1".WithGuid())
                            .BuildAsync(),
                        _create.DealType
                            .WithName("Test2".WithGuid())
                            .BuildAsync())
                )
                .Select(x => x.Id)
                .ToList();

            var types = await _dealTypesClient.GetListAsync(accessToken, typeIds);

            Assert.NotEmpty(types);
            Assert.Equal(typeIds.Count, types.Count);
        }

        [Fact]
        public async Task WhenGetPagedList_ThenSuccess()
        {
            var accessToken = await _accessTokenGetter.GetAsync();

            var name = "Test1".WithGuid();
            await Task.WhenAll(
                _create.DealType
                    .WithName(name)
                    .BuildAsync());

            var request = new DealTypeGetPagedListRequestParameter
            {
                Name = name
            };

            var types = await _dealTypesClient.GetPagedListAsync(accessToken, request);

            var results = types
                .Skip(1)
                .Zip(types, (previous, current) => current.CreateDateTime >= previous.CreateDateTime);

            Assert.NotEmpty(types);
            Assert.All(results, Assert.True);
        }

        [Fact]
        public async Task WhenCreate_ThenSuccess()
        {
            var accessToken = await _accessTokenGetter.GetAsync();

            var type = new DealType
            {
                Name = "Test".WithGuid(),
                IsDeleted = false
            };

            var createdTypeId = await _dealTypesClient.CreateAsync(accessToken, type);

            var createdType = await _dealTypesClient.GetAsync(accessToken, createdTypeId);

            Assert.NotNull(createdType);
            Assert.Equal(createdTypeId, createdType.Id);
            Assert.Equal(type.Name, createdType.Name);
            Assert.Equal(type.IsDeleted, createdType.IsDeleted);
            Assert.True(createdType.CreateDateTime.IsMoreThanMinValue());
        }

        [Fact]
        public async Task WhenUpdate_ThenSuccess()
        {
            var accessToken = await _accessTokenGetter.GetAsync();

            var type = await _create.DealType
                .WithName("Test1".WithGuid())
                .BuildAsync();

            type.Name = "Test2".WithGuid();
            type.IsDeleted = true;

            await _dealTypesClient.UpdateAsync(accessToken, type);

            var updatedType = await _dealTypesClient.GetAsync(accessToken, type.Id);

            Assert.Equal(type.Name, updatedType.Name);
            Assert.Equal(type.IsDeleted, updatedType.IsDeleted);
        }

        [Fact]
        public async Task WhenDelete_ThenSuccess()
        {
            var accessToken = await _accessTokenGetter.GetAsync();

            var typeIds = (
                    await Task.WhenAll(
                        _create.DealType
                            .WithName("Test1".WithGuid())
                            .BuildAsync(),
                        _create.DealType
                            .WithName("Test2".WithGuid())
                            .BuildAsync())
                )
                .Select(x => x.Id)
                .ToList();

            await _dealTypesClient.DeleteAsync(accessToken, typeIds);

            var types = await _dealTypesClient.GetListAsync(accessToken, typeIds);

            Assert.All(types, x => Assert.True(x.IsDeleted));
        }

        [Fact]
        public async Task WhenRestore_ThenSuccess()
        {
            var accessToken = await _accessTokenGetter.GetAsync();

            var typeIds = (
                    await Task.WhenAll(
                        _create.DealType
                            .WithName("Test1".WithGuid())
                            .BuildAsync(),
                        _create.DealType
                            .WithName("Test2".WithGuid())
                            .BuildAsync())
                )
                .Select(x => x.Id)
                .ToList();

            await _dealTypesClient.RestoreAsync(accessToken, typeIds);

            var types = await _dealTypesClient.GetListAsync(accessToken, typeIds);

            Assert.All(types, x => Assert.False(x.IsDeleted));
        }
    }
}