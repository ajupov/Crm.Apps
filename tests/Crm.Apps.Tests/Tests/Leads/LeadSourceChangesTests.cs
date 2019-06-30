using System.Linq;
using System.Threading.Tasks;
using Crm.Apps.Tests.Creator;
using Crm.Clients.Leads.Clients;
using Crm.Clients.Leads.Models;
using Crm.Utils.DateTime;
using Crm.Utils.Guid;
using Crm.Utils.Json;
using Crm.Utils.String;
using Xunit;

namespace Crm.Apps.Tests.Tests.Leads
{
    public class LeadSourceChangesTests
    {
        private readonly ICreate _create;
        private readonly ILeadSourcesClient _leadSourcesClient;
        private readonly ILeadSourceChangesClient _sourceChangesClient;

        public LeadSourceChangesTests(ICreate create, ILeadSourcesClient leadSourcesClient,
            ILeadSourceChangesClient sourceChangesClient)
        {
            _create = create;
            _leadSourcesClient = leadSourcesClient;
            _sourceChangesClient = sourceChangesClient;
        }

        [Fact]
        public async Task WhenGetPagedList_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            var source = await _create.LeadSource.WithAccountId(account.Id).BuildAsync().ConfigureAwait(false);
            source.Name = "Test2";
            source.IsDeleted = true;
            await _leadSourcesClient.UpdateAsync(source).ConfigureAwait(false);

            var changes = await _sourceChangesClient
                .GetPagedListAsync(sourceId: source.Id, sortBy: "CreateDateTime", orderBy: "asc")
                .ConfigureAwait(false);

            Assert.NotEmpty(changes);
            Assert.True(changes.All(x => !x.ChangerUserId.IsEmpty()));
            Assert.True(changes.All(x => x.SourceId == source.Id));
            Assert.True(changes.All(x => x.CreateDateTime.IsMoreThanMinValue()));
            Assert.True(changes.First().OldValueJson.IsEmpty());
            Assert.True(!changes.First().NewValueJson.IsEmpty());
            Assert.NotNull(changes.First().NewValueJson.FromJsonString<LeadSource>());
            Assert.True(!changes.Last().OldValueJson.IsEmpty());
            Assert.True(!changes.Last().NewValueJson.IsEmpty());
            Assert.False(changes.Last().OldValueJson.FromJsonString<LeadSource>().IsDeleted);
            Assert.True(changes.Last().NewValueJson.FromJsonString<LeadSource>().IsDeleted);
            Assert.Equal(changes.Last().NewValueJson.FromJsonString<LeadSource>().Name, source.Name);
        }
    }
}