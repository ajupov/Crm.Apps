using System.Threading.Tasks;
using Crm.Apps.v1.Clients.Activities.Models;

namespace Crm.Apps.Tests.Builders.Activities
{
    public interface IActivityTypeBuilder
    {
        ActivityTypeBuilder WithName(string name);

        ActivityTypeBuilder AsDeleted();

        Task<ActivityType> BuildAsync();
    }
}