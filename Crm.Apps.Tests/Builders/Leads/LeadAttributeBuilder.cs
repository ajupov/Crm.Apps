using System;
using System.Threading.Tasks;
using Crm.Apps.Clients.Leads.Clients;
using Crm.Apps.Clients.Leads.Models;
using Crm.Common.All.Types.AttributeType;

namespace Crm.Apps.Tests.Builders.Leads
{
    public class LeadAttributeBuilder : ILeadAttributeBuilder
    {
        private readonly ILeadAttributesClient _leadAttributesClient;
        private readonly LeadAttribute _attribute;

        public LeadAttributeBuilder(ILeadAttributesClient leadAttributesClient)
        {
            _leadAttributesClient = leadAttributesClient;
            _attribute = new LeadAttribute
            {
                AccountId = Guid.Empty,
                Type = AttributeType.Text,
                Key = "Test",
                IsDeleted = false
            };
        }

        public LeadAttributeBuilder WithAccountId(Guid accountId)
        {
            _attribute.AccountId = accountId;

            return this;
        }

        public LeadAttributeBuilder WithType(AttributeType type)
        {
            _attribute.Type = type;

            return this;
        }

        public LeadAttributeBuilder WithKey(string key)
        {
            _attribute.Key = key;

            return this;
        }

        public LeadAttributeBuilder AsDeleted()
        {
            _attribute.IsDeleted = true;

            return this;
        }

        public async Task<LeadAttribute> BuildAsync()
        {
            var id = await _leadAttributesClient.CreateAsync(_attribute);

            return await _leadAttributesClient.GetAsync(id);
        }
    }
}