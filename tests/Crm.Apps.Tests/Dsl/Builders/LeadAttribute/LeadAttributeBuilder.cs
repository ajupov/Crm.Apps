using System;
using System.Threading.Tasks;
using Crm.Clients.Leads.Clients;
using Crm.Common.Types;
using Crm.Utils.Guid;

namespace Crm.Apps.Tests.Dsl.Builders.LeadAttribute
{
    public class LeadAttributeBuilder : ILeadAttributeBuilder
    {
        private readonly Clients.Leads.Models.LeadAttribute _leadAttribute;
        private readonly ILeadAttributesClient _leadAttributesClient;

        public LeadAttributeBuilder(ILeadAttributesClient leadAttributesClient)
        {
            _leadAttributesClient = leadAttributesClient;
            _leadAttribute = new Clients.Leads.Models.LeadAttribute
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

        public async Task<Clients.Leads.Models.LeadAttribute> BuildAsync()
        {
            if (_leadAttribute.AccountId.IsEmpty())
            {
                throw new InvalidOperationException(nameof(_leadAttribute.AccountId));
            }

            var createdId = await _leadAttributesClient.CreateAsync(_leadAttribute).ConfigureAwait(false);

            return await _leadAttributesClient.GetAsync(createdId).ConfigureAwait(false);
        }
    }
}