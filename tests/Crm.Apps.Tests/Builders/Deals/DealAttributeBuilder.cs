using System;
using System.Threading.Tasks;
using Crm.Clients.Deals.Clients;
using Crm.Clients.Deals.Models;
using Crm.Common.Types;
using Crm.Utils.Guid;

namespace Crm.Apps.Tests.Builders.Deals
{
    public class DealAttributeBuilder : IDealAttributeBuilder
    {
        private readonly IDealAttributesClient _dealAttributesClient;
        private readonly DealAttribute _dealAttribute;

        public DealAttributeBuilder(IDealAttributesClient dealAttributesClient)
        {
            _dealAttributesClient = dealAttributesClient;
            _dealAttribute = new DealAttribute
            {
                AccountId = Guid.Empty,
                Type = AttributeType.Text,
                Key = "Test"
            };
        }

        public DealAttributeBuilder WithAccountId(Guid accountId)
        {
            _dealAttribute.AccountId = accountId;

            return this;
        }

        public DealAttributeBuilder WithType(AttributeType type)
        {
            _dealAttribute.Type = type;

            return this;
        }

        public DealAttributeBuilder WithKey(string key)
        {
            _dealAttribute.Key = key;

            return this;
        }

        public async Task<DealAttribute> BuildAsync()
        {
            if (_dealAttribute.AccountId.IsEmpty())
            {
                throw new InvalidOperationException(nameof(_dealAttribute.AccountId));
            }

            var createdId = await _dealAttributesClient.CreateAsync(_dealAttribute).ConfigureAwait(false);

            return await _dealAttributesClient.GetAsync(createdId).ConfigureAwait(false);
        }
    }
}