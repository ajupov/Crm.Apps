using System;
using System.Threading.Tasks;
using Crm.Clients.Activities.Models;

namespace Crm.Apps.Tests.Builders.Activities
{
    public interface IActivityStatusBuilder
    {
        ActivityStatusBuilder WithAccountId(Guid accountId);

        ActivityStatusBuilder WithName(string name);

        ActivityStatusBuilder AsFinish();

        Task<ActivityStatus> BuildAsync();
    }
}