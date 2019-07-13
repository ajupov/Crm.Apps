using System.Linq;
using System.Threading.Tasks;
using Crm.Apps.Tests.Creator;
using Crm.Clients.Leads.Clients;
using Crm.Clients.Leads.Models;
using Crm.Utils.DateTime;
using Xunit;

namespace Crm.Apps.Tests.Tests.Leads
{
    public class LeadSourcesTests
    {
        private readonly ICreate _create;
        private readonly ILeadSourcesClient _leadSourcesClient;

        public LeadSourcesTests(ICreate create, ILeadSourcesClient leadSourcesClient)
        {
            _create = create;
            _leadSourcesClient = leadSourcesClient;
        }

        [Fact]
        public async Task WhenGet_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync();
            var statusId = (await _create.LeadSource.WithAccountId(account.Id).BuildAsync()).Id;

            var status = await _leadSourcesClient.GetAsync(statusId);

            Assert.NotNull(status);
            Assert.Equal(statusId, status.Id);
        }

        [Fact]
        public async Task WhenGetList_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync();
            var statusIds = (await Task.WhenAll(
                    _create.LeadSource.WithAccountId(account.Id).WithName("Test1").BuildAsync(),
                    _create.LeadSource.WithAccountId(account.Id).WithName("Test2").BuildAsync())
                ).Select(x => x.Id).ToList();

            var statuses = await _leadSourcesClient.GetListAsync(statusIds);

            Assert.NotEmpty(statuses);
            Assert.Equal(statusIds.Count, statuses.Count);
        }

        [Fact]
        public async Task WhenGetPagedList_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync();
            await Task.WhenAll(_create.LeadSource.WithAccountId(account.Id).WithName("Test1").BuildAsync())
                ;

            var statuses = await _leadSourcesClient
                .GetPagedListAsync(account.Id, "Test1", sortBy: "CreateDateTime", orderBy: "desc")
                ;

            var results = statuses.Skip(1).Zip(statuses,
                (previous, current) => current.CreateDateTime >= previous.CreateDateTime);

            Assert.NotEmpty(statuses);
            Assert.All(results, Assert.True);
        }

        [Fact]
        public async Task WhenCreate_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync();
            var status = new LeadSource
            {
                AccountId = account.Id,
                Name = "Test",
                IsDeleted = false
            };

            var createdSourceId = await _leadSourcesClient.CreateAsync(status);

            var createdSource = await _leadSourcesClient.GetAsync(createdSourceId);

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
            var account = await _create.Account.BuildAsync();
            var status = await _create.LeadSource.WithAccountId(account.Id).WithName("Test1").BuildAsync()
                ;

            status.Name = "Test2";
            status.IsDeleted = true;

            await _leadSourcesClient.UpdateAsync(status);

            var updatedSource = await _leadSourcesClient.GetAsync(status.Id);

            Assert.Equal(status.Name, updatedSource.Name);
            Assert.Equal(status.IsDeleted, updatedSource.IsDeleted);
        }

        [Fact]
        public async Task WhenDelete_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync();
            var statusIds = (await Task.WhenAll(
                    _create.LeadSource.WithAccountId(account.Id).WithName("Test1").BuildAsync(),
                    _create.LeadSource.WithAccountId(account.Id).WithName("Test2").BuildAsync())
                ).Select(x => x.Id).ToList();

            await _leadSourcesClient.DeleteAsync(statusIds);

            var statuses = await _leadSourcesClient.GetListAsync(statusIds);

            Assert.All(statuses, x => Assert.True(x.IsDeleted));
        }

        [Fact]
        public async Task WhenRestore_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync();
            var statusIds = (await Task.WhenAll(
                    _create.LeadSource.WithAccountId(account.Id).WithName("Test1").BuildAsync(),
                    _create.LeadSource.WithAccountId(account.Id).WithName("Test2").BuildAsync())
                ).Select(x => x.Id).ToList();

            await _leadSourcesClient.RestoreAsync(statusIds);

            var statuses = await _leadSourcesClient.GetListAsync(statusIds);

            Assert.All(statuses, x => Assert.False(x.IsDeleted));
        }
    }
}