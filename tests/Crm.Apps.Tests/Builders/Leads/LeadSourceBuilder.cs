using System;
using System.Threading.Tasks;
using Crm.Clients.Leads.Clients;
using Crm.Utils.Guid;

namespace Crm.Apps.Tests.Builders.Leads
{
    public class LeadSourceBuilder : ILeadSourceBuilder
    {
        private readonly Clients.Leads.Models.LeadSource _leadSource;
        private readonly ILeadSourcesClient _leadSourcesClient;

        public LeadSourceBuilder(ILeadSourcesClient leadSourcesClient)
        {
            _leadSourcesClient = leadSourcesClient;
            _leadSource = new Clients.Leads.Models.LeadSource
            {
                AccountId = Guid.Empty,
                Name = "Test"
            };
        }

        public LeadSourceBuilder WithAccountId(Guid accountId)
        {
            _leadSource.AccountId = accountId;

            return this;
        }

        public LeadSourceBuilder WithName(string name)
        {
            _leadSource.Name = name;

            return this;
        }

        public async Task<Clients.Leads.Models.LeadSource> BuildAsync()
        {
            if (_leadSource.AccountId.IsEmpty())
            {
                throw new InvalidOperationException(nameof(_leadSource.AccountId));
            }

            var createdId = await _leadSourcesClient.CreateAsync(_leadSource).ConfigureAwait(false);

            return await _leadSourcesClient.GetAsync(createdId).ConfigureAwait(false);
        }
    }
}