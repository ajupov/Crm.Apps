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
    public class LeadChangesTests
    {
        private readonly ICreate _create;
        private readonly ILeadsClient _leadsClient;
        private readonly ILeadChangesClient _leadChangesClient;

        public LeadChangesTests(ICreate create, ILeadsClient leadsClient,
            ILeadChangesClient leadChangesClient)
        {
            _create = create;
            _leadsClient = leadsClient;
            _leadChangesClient = leadChangesClient;
        }

        [Fact]
        public async Task WhenGetPagedList_ThenSuccess()
        {
            
            var source = await _create.LeadSource.BuildAsync();
            var lead = await _create.Lead.WithSourceId(source.Id).BuildAsync()
                ;
            lead.IsDeleted = true;
            await _leadsClient.UpdateAsync(lead);

            var changes = await _leadChangesClient
                .GetPagedListAsync(leadId: lead.Id, sortBy: "CreateDateTime", orderBy: "asc")
                ;

            Assert.NotEmpty(changes);
            Assert.True(changes.All(x => !x.ChangerUserId.IsEmpty()));
            Assert.True(changes.All(x => x.LeadId == lead.Id));
            Assert.True(changes.All(x => x.CreateDateTime.IsMoreThanMinValue()));
            Assert.True(changes.First().OldValueJson.IsEmpty());
            Assert.True(!changes.First().NewValueJson.IsEmpty());
            Assert.NotNull(changes.First().NewValueJson.FromJsonString<Lead>());
            Assert.True(!changes.Last().OldValueJson.IsEmpty());
            Assert.True(!changes.Last().NewValueJson.IsEmpty());
            Assert.False(changes.Last().OldValueJson.FromJsonString<Lead>().IsDeleted);
            Assert.True(changes.Last().NewValueJson.FromJsonString<Lead>().IsDeleted);
        }
    }
}