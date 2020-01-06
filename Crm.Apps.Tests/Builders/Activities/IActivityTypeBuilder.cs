using System;
using System.Threading.Tasks;
using Crm.Apps.Clients.Activities.Models;

namespace Crm.Apps.Tests.Builders.Activities
{
    public interface IActivityTypeBuilder
    {
        ActivityTypeBuilder WithAccountId(Guid accountId);

        ActivityTypeBuilder WithName(string name);

        ActivityTypeBuilder AsDeleted();

        Task<ActivityType> BuildAsync();
    }
}