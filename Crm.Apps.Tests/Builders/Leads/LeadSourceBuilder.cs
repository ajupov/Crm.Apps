using System;
using System.Threading.Tasks;
using Crm.Apps.Clients.Leads.Clients;
using Crm.Apps.Clients.Leads.Models;

namespace Crm.Apps.Tests.Builders.Leads
{
    public class LeadSourceBuilder : ILeadSourceBuilder
    {
        private readonly ILeadSourcesClient _leadSourcesClient;
        private readonly LeadSource _source;

        public LeadSourceBuilder(ILeadSourcesClient leadSourcesClient)
        {
            _leadSourcesClient = leadSourcesClient;
            _source = new LeadSource
            {
                AccountId = Guid.Empty,
                Name = "Test",
                IsDeleted = false
            };
        }

        public LeadSourceBuilder WithAccountId(Guid accountId)
        {
            _source.AccountId = accountId;

            return this;
        }

        public LeadSourceBuilder WithName(string name)
        {
            _source.Name = name;

            return this;
        }

        public LeadSourceBuilder AsDeleted()
        {
            _source.IsDeleted = true;

            return this;
        }

        public async Task<LeadSource> BuildAsync()
        {
            var id = await _leadSourcesClient.CreateAsync(_source);

            return await _leadSourcesClient.GetAsync(id);
        }
    }
}