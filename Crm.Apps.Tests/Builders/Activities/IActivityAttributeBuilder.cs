using System.Threading.Tasks;
using Crm.Apps.v1.Clients.Activities.Models;
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