using System;
using System.Threading.Tasks;
using Crm.Apps.Clients.Activities.Clients;
using Crm.Apps.Clients.Activities.Models;

namespace Crm.Apps.Tests.Builders.Activities
{
    public class ActivityTypeBuilder : IActivityTypeBuilder
    {
        private readonly IActivityTypesClient _activityTypesClient;
        private readonly ActivityType _type;

        public ActivityTypeBuilder(IActivityTypesClient activityTypesClient)
        {
            _activityTypesClient = activityTypesClient;
            _type = new ActivityType
            {
                AccountId = Guid.Empty,
                Name = "Test",
                IsDeleted = false
            };
        }

        public ActivityTypeBuilder WithName(string name)
        {
            _type.Name = name;

            return this;
        }

        public ActivityTypeBuilder AsDeleted()
        {
            _type.IsDeleted = true;

            return this;
        }

        public async Task<ActivityType> BuildAsync()
        {
            var id = await _activityTypesClient.CreateAsync(_type);

            return await _activityTypesClient.GetAsync(id);
        }
    }
}