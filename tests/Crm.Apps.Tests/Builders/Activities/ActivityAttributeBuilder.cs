using System;
using System.Threading.Tasks;
using Crm.Clients.Activities.Clients;
using Crm.Clients.Activities.Models;
using Crm.Common.Types;
using Crm.Utils.Guid;

namespace Crm.Apps.Tests.Builders.Activities
{
    public class ActivityAttributeBuilder : IActivityAttributeBuilder
    {
        private readonly IActivityAttributesClient _activityAttributesClient;
        private readonly ActivityAttribute _activityAttribute;

        public ActivityAttributeBuilder(IActivityAttributesClient activityAttributesClient)
        {
            _activityAttributesClient = activityAttributesClient;
            _activityAttribute = new ActivityAttribute
            {
                AccountId = Guid.Empty,
                Type = AttributeType.Text,
                Key = "Test"
            };
        }

        public ActivityAttributeBuilder WithAccountId(Guid accountId)
        {
            _activityAttribute.AccountId = accountId;

            return this;
        }

        public ActivityAttributeBuilder WithType(AttributeType type)
        {
            _activityAttribute.Type = type;

            return this;
        }

        public ActivityAttributeBuilder WithKey(string key)
        {
            _activityAttribute.Key = key;

            return this;
        }

        public async Task<ActivityAttribute> BuildAsync()
        {
            if (_activityAttribute.AccountId.IsEmpty())
            {
                throw new InvalidOperationException(nameof(_activityAttribute.AccountId));
            }

            var createdId = await _activityAttributesClient.CreateAsync(_activityAttribute).ConfigureAwait(false);

            return await _activityAttributesClient.GetAsync(createdId).ConfigureAwait(false);
        }
    }
}