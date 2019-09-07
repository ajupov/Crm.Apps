using System;
using System.Threading.Tasks;
using Crm.Clients.Activities.Clients;
using Crm.Clients.Activities.Models;
using Crm.Clients.Activities.RequestParameters;
using Crm.Common.Types;
using Crm.Utils.Guid;

namespace Crm.Apps.Tests.Builders.Activities
{
    public class ActivityAttributeBuilder : IActivityAttributeBuilder
    {
        private readonly IActivityAttributesClient _activityAttributesClient;
        private readonly ActivityAttributeCreateRequest _request;

        public ActivityAttributeBuilder(IActivityAttributesClient activityAttributesClient)
        {
            _activityAttributesClient = activityAttributesClient;
            _request = new ActivityAttributeCreateRequest
            {
                AccountId = Guid.Empty,
                Type = AttributeType.Text,
                Key = "Test"
            };
        }

        public ActivityAttributeBuilder WithAccountId(Guid accountId)
        {
            _request.AccountId = accountId;

            return this;
        }

        public ActivityAttributeBuilder WithType(AttributeType type)
        {
            _request.Type = type;

            return this;
        }

        public ActivityAttributeBuilder WithKey(string key)
        {
            _request.Key = key;

            return this;
        }

        public async Task<ActivityAttribute> BuildAsync()
        {
            if (_request.AccountId.IsEmpty())
            {
                throw new InvalidOperationException(nameof(_request.AccountId));
            }

            var createdId = await _activityAttributesClient.CreateAsync(_request);

            return await _activityAttributesClient.GetAsync(createdId);
        }
    }
}