using System.Linq;
using System.Threading.Tasks;
using Ajupov.Utils.All.DateTime;
using Crm.Apps.Tests.Services.AccessTokenGetter;
using Crm.Apps.Tests.Services.Creator;
using Crm.Apps.v1.Clients.Leads.Clients;
using Crm.Apps.v1.Clients.Leads.Models;
using Crm.Apps.v1.Clients.Leads.RequestParameters;
using Xunit;

namespace Crm.Apps.Tests.Tests.Leads
{
    public class LeadSourcesTests
    {
        private readonly IAccessTokenGetter _accessTokenGetter;
        private readonly ICreate _create;
        private readonly ILeadSourcesClient _leadSourcesClient;

        public LeadSourcesTests(
            IAccessTokenGetter accessTokenGetter,
            ICreate create,
            ILeadSourcesClient leadSourcesClient)
        {
            _accessTokenGetter = accessTokenGetter;
            _create = create;
            _leadSourcesClient = leadSourcesClient;
        }

        [Fact]
        public async Task WhenGet_ThenSuccess()
        {
            var accessToken = await _accessTokenGetter.GetAsync();

            var statusId = (await _create.LeadSource.BuildAsync()).Id;

            var status = await _leadSourcesClient.GetAsync(accessToken, statusId);

            Assert.NotNull(status);
            Assert.Equal(statusId, status.Id);
        }

        [Fact]
        public async Task WhenGetList_ThenSuccess()
        {
            var accessToken = await _accessTokenGetter.GetAsync();

            var statusIds = (
                    await Task.WhenAll(
                        _create.LeadSource
                            .WithName("Test1")
                            .BuildAsync(),
                        _create.LeadSource
                            .WithName("Test2")
                            .BuildAsync())
                )
                .Select(x => x.Id)
                .ToList();

            var statuses = await _leadSourcesClient.GetListAsync(accessToken, statusIds);

            Assert.NotEmpty(statuses);
            Assert.Equal(statusIds.Count, statuses.Count);
        }

        [Fact]
        public async Task WhenGetPagedList_ThenSuccess()
        {
            var accessToken = await _accessTokenGetter.GetAsync();

            await Task.WhenAll(
                _create.LeadSource
                    .WithName("Test1")
                    .BuildAsync());

            var request = new LeadSourceGetPagedListRequestParameter
            {
                Name = "Test1"
            };

            var statuses = await _leadSourcesClient.GetPagedListAsync(accessToken, request);

            var results = statuses
                .Skip(1)
                .Zip(statuses, (previous, current) => current.CreateDateTime >= previous.CreateDateTime);

            Assert.NotEmpty(statuses);
            Assert.All(results, Assert.True);
        }

        [Fact]
        public async Task WhenCreate_ThenSuccess()
        {
            var accessToken = await _accessTokenGetter.GetAsync();

            var status = new LeadSource
            {
                Name = "Test",
                IsDeleted = false
            };

            var createdSourceId = await _leadSourcesClient.CreateAsync(accessToken, status);

            var createdSource = await _leadSourcesClient.GetAsync(accessToken, createdSourceId);

            Assert.NotNull(createdSource);
            Assert.Equal(createdSourceId, createdSource.Id);
            Assert.Equal(status.AccountId, createdSource.AccountId);
            Assert.Equal(status.Name, createdSource.Name);
            Assert.Equal(status.IsDeleted, createdSource.IsDeleted);
            Assert.True(createdSource.CreateDateTime.IsMoreThanMinValue());
        }

        [Fact]
        public async Task WhenUpdate_ThenSuccess()
        {
            var accessToken = await _accessTokenGetter.GetAsync();

            var status = await _create.LeadSource
                .WithName("Test1")
                .BuildAsync();

            status.Name = "Test2";
            status.IsDeleted = true;

            await _leadSourcesClient.UpdateAsync(accessToken, status);

            var updatedSource = await _leadSourcesClient.GetAsync(accessToken, status.Id);

            Assert.Equal(status.Name, updatedSource.Name);
            Assert.Equal(status.IsDeleted, updatedSource.IsDeleted);
        }

        [Fact]
        public async Task WhenDelete_ThenSuccess()
        {
            var accessToken = await _accessTokenGetter.GetAsync();

            var statusIds = (
                    await Task.WhenAll(
                        _create.LeadSource
                            .WithName("Test1")
                            .BuildAsync(),
                        _create.LeadSource
                            .WithName("Test2")
                            .BuildAsync())
                )
                .Select(x => x.Id)
                .ToList();

            await _leadSourcesClient.DeleteAsync(accessToken, statusIds);

            var statuses = await _leadSourcesClient.GetListAsync(accessToken, statusIds);

            Assert.All(statuses, x => Assert.True(x.IsDeleted));
        }

        [Fact]
        public async Task WhenRestore_ThenSuccess()
        {
            var accessToken = await _accessTokenGetter.GetAsync();

            var statusIds = (
                    await Task.WhenAll(
                        _create.LeadSource
                            .WithName("Test1")
                            .BuildAsync(),
                        _create.LeadSource
                            .WithName("Test2")
                            .BuildAsync())
                )
                .Select(x => x.Id)
                .ToList();

            await _leadSourcesClient.RestoreAsync(accessToken, statusIds);

            var statuses = await _leadSourcesClient.GetListAsync(accessToken, statusIds);

            Assert.All(statuses, x => Assert.False(x.IsDeleted));
        }
    }
}