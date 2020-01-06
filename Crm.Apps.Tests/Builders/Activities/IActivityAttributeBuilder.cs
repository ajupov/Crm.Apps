using System.Threading.Tasks;
using Crm.Apps.Clients.Activities.Models;
using Crm.Common.All.Types.AttributeType;

namespace Crm.Apps.Tests.Builders.Activities
{
    public interface IActivityAttributeBuilder
    {
        ActivityAttributeBuilder WithType(AttributeType type);

        ActivityAttributeBuilder WithKey(string key);

        ActivityAttributeBuilder AsDeleted();

        Task<ActivityAttribute> BuildAsync();
    }
}