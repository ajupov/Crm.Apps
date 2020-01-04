using System;
using System.Threading.Tasks;
using Crm.Clients.Leads.Clients;
using Crm.Clients.Leads.Models;
using Crm.Utils.Guid;

namespace Crm.Apps.Tests.Builders.Leads
{
    public class LeadSourceBuilder : ILeadSourceBuilder
    {
        private readonly ILeadSourcesClient _leadSourcesClient;
        private readonly LeadSource _leadSource;

        public LeadSourceBuilder(ILeadSourcesClient leadSourcesClient)
        {
            _leadSourcesClient = leadSourcesClient;
            _leadSource = new LeadSource
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

        public async Task<LeadSource> BuildAsync()
        {
            if (_leadSource.AccountId.IsEmpty())
            {
                throw new InvalidOperationException(nameof(_leadSource.AccountId));
            }

            var createdId = await _leadSourcesClient.CreateAsync(_leadSource);

            return await _leadSourcesClient.GetAsync(createdId);
        }
    }
}