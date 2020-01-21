using System;
using System.Threading.Tasks;
using Crm.Apps.v1.Clients.Deals.Clients;
using Crm.Apps.v1.Clients.Deals.Models;
using Crm.Common.All.Types.AttributeType;

namespace Crm.Apps.Tests.Builders.Deals
{
    public class DealAttributeBuilder : IDealAttributeBuilder
    {
        private readonly IDealAttributesClient _dealAttributesClient;
        private readonly DealAttribute _attribute;

        public DealAttributeBuilder(IDealAttributesClient dealAttributesClient)
        {
            _dealAttributesClient = dealAttributesClient;
            _attribute = new DealAttribute
            {
                AccountId = Guid.Empty,
                Type = AttributeType.Text,
                Key = "Test",
                IsDeleted = false
            };
        }

        public DealAttributeBuilder WithType(AttributeType type)
        {
            _attribute.Type = type;

            return this;
        }

        public DealAttributeBuilder WithKey(string key)
        {
            _attribute.Key = key;

            return this;
        }

        public DealAttributeBuilder AsDeleted()
        {
            _attribute.IsDeleted = true;

            return this;
        }

        public async Task<DealAttribute> BuildAsync()
        {
            var id = await _dealAttributesClient.CreateAsync(_attribute);

            return await _dealAttributesClient.GetAsync(id);
        }
    }
}