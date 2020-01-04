using System;
using System.Threading.Tasks;
using Crm.Clients.Activities.Models;

namespace Crm.Apps.Tests.Builders.Activities
{
    public interface IActivityTypeBuilder
    {
        ActivityTypeBuilder WithAccountId(Guid accountId);

        ActivityTypeBuilder WithName(string name);

        Task<ActivityType> BuildAsync();
    }
}