using System.Threading.Tasks;
using Crm.Apps.v1.Clients.Activities.Models;

namespace Crm.Apps.Tests.Builders.Activities
{
    public interface IActivityStatusBuilder
    {
        ActivityStatusBuilder WithName(string name);

        ActivityStatusBuilder AsFinish();

        ActivityStatusBuilder AsDeleted();

        Task<ActivityStatus> BuildAsync();
    }
}