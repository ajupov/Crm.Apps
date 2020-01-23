using System;
using System.Threading.Tasks;
using Crm.Apps.Tests.Services.AccessTokenGetter;
using Crm.Apps.v1.Clients.Leads.Clients;
using Crm.Apps.v1.Clients.Leads.Models;

namespace Crm.Apps.Tests.Builders.Leads
{
    public class LeadSourceBuilder : ILeadSourceBuilder
    {
        private readonly IAccessTokenGetter _accessTokenGetter;
        private readonly ILeadSourcesClient _leadSourcesClient;
        private readonly LeadSource _source;

        public LeadSourceBuilder(IAccessTokenGetter accessTokenGetter, ILeadSourcesClient leadSourcesClient)
        {
            _leadSourcesClient = leadSourcesClient;
            _accessTokenGetter = accessTokenGetter;
            _source = new LeadSource
            {
                AccountId = Guid.Empty,
                Name = "Test",
                IsDeleted = false
            };
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
            var accessToken = await _accessTokenGetter.GetAsync();

            var id = await _leadSourcesClient.CreateAsync(accessToken, _source);

            return await _leadSourcesClient.GetAsync(accessToken, id);
        }
    }
}