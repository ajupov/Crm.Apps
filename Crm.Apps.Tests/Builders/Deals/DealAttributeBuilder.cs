using System;
using System.Threading.Tasks;
using Crm.Apps.Tests.Extensions;
using Crm.Apps.Tests.Services.AccessTokenGetter;
using Crm.Apps.v1.Clients.Deals.Clients;
using Crm.Apps.v1.Clients.Deals.Models;
using Crm.Common.All.Types.AttributeType;

namespace Crm.Apps.Tests.Builders.Deals
{
    public class DealAttributeBuilder : IDealAttributeBuilder
    {
        private readonly IAccessTokenGetter _accessTokenGetter;
        private readonly IDealAttributesClient _dealAttributesClient;
        private readonly DealAttribute _attribute;

        public DealAttributeBuilder(IAccessTokenGetter accessTokenGetter, IDealAttributesClient dealAttributesClient)
        {
            _dealAttributesClient = dealAttributesClient;
            _accessTokenGetter = accessTokenGetter;
            _attribute = new DealAttribute
            {
                Type = AttributeType.Text,
                Key = "Test".WithGuid(),
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
            var accessToken = await _accessTokenGetter.GetAsync();

            var id = await _dealAttributesClient.CreateAsync(accessToken, _attribute);

            return await _dealAttributesClient.GetAsync(accessToken, id);
        }
    }
}