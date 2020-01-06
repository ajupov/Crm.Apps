using System.Threading.Tasks;
using Crm.Apps.Clients.Activities.Models;

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