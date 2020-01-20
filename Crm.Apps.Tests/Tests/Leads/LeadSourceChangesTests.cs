using System.Linq;
using System.Threading.Tasks;
using Ajupov.Utils.All.DateTime;
using Ajupov.Utils.All.Json;
using Crm.Apps.Clients.Leads.Clients;
using Crm.Apps.Clients.Leads.Models;
using Crm.Apps.Tests.Creator;
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
            
            var source = await _create.LeadSource.BuildAsync();
            source.Name = "Test2";
            source.IsDeleted = true;
            await _leadSourcesClient.UpdateAsync(source);

            var changes = await _sourceChangesClient
                .GetPagedListAsync(sourceId: source.Id, sortBy: "CreateDateTime", orderBy: "asc")
                ;

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