using System.Threading.Tasks;
using Crm.Apps.Clients.Activities.Models;

namespace Crm.Apps.Tests.Builders.Activities
{
    public interface IActivityTypeBuilder
    {
        ActivityTypeBuilder WithName(string name);

        ActivityTypeBuilder AsDeleted();

        Task<ActivityType> BuildAsync();
    }
}