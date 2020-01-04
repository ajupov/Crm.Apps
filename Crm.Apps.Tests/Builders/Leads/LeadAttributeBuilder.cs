using System;
using System.Threading.Tasks;
using Crm.Clients.Leads.Clients;
using Crm.Clients.Leads.Models;
using Crm.Common.Types;
using Crm.Utils.Guid;

namespace Crm.Apps.Tests.Builders.Leads
{
    public class LeadAttributeBuilder : ILeadAttributeBuilder
    {
        private readonly ILeadAttributesClient _leadAttributesClient;
        private readonly LeadAttribute _leadAttribute;

        public LeadAttributeBuilder(ILeadAttributesClient leadAttributesClient)
        {
            _leadAttributesClient = leadAttributesClient;
            _leadAttribute = new LeadAttribute
            {
                AccountId = Guid.Empty,
                Type = AttributeType.Text,
                Key = "Test"
            };
        }

        public LeadAttributeBuilder WithAccountId(Guid accountId)
        {
            _leadAttribute.AccountId = accountId;

            return this;
        }

        public LeadAttributeBuilder WithType(AttributeType type)
        {
            _leadAttribute.Type = type;

            return this;
        }

        public LeadAttributeBuilder WithKey(string key)
        {
            _leadAttribute.Key = key;

            return this;
        }

        public async Task<LeadAttribute> BuildAsync()
        {
            if (_leadAttribute.AccountId.IsEmpty())
            {
                throw new InvalidOperationException(nameof(_leadAttribute.AccountId));
            }

            var createdId = await _leadAttributesClient.CreateAsync(_leadAttribute);

            return await _leadAttributesClient.GetAsync(createdId);
        }
    }
}