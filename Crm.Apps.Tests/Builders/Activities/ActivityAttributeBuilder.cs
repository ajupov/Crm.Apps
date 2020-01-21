using System.Threading.Tasks;
using Crm.Apps.v1.Clients.Activities.Clients;
using Crm.Apps.v1.Clients.Activities.Models;
using Crm.Common.All.Types.AttributeType;

namespace Crm.Apps.Tests.Builders.Activities
{
    public class ActivityAttributeBuilder : IActivityAttributeBuilder
    {
        private readonly IActivityAttributesClient _activityAttributesClient;
        private readonly ActivityAttribute _attribute;

        public ActivityAttributeBuilder(IActivityAttributesClient activityAttributesClient)
        {
            _activityAttributesClient = activityAttributesClient;
            _attribute = new ActivityAttribute
            {
                Type = AttributeType.Text,
                Key = "Test",
                IsDeleted = false
            };
        }

        public ActivityAttributeBuilder WithType(AttributeType type)
        {
            _attribute.Type = type;

            return this;
        }

        public ActivityAttributeBuilder WithKey(string key)
        {
            _attribute.Key = key;

            return this;
        }

        public ActivityAttributeBuilder AsDeleted()
        {
            _attribute.IsDeleted = true;

            return this;
        }

        public async Task<ActivityAttribute> BuildAsync()
        {
            var id = await _activityAttributesClient.CreateAsync(_attribute);

            return await _activityAttributesClient.GetAsync(id);
        }
    }
}