using System.Threading.Tasks;
using Crm.Apps.Clients.Deals.Models;
using Crm.Common.All.Types.AttributeType;

namespace Crm.Apps.Tests.Builders.Deals
{
    public interface IDealAttributeBuilder
    {
        DealAttributeBuilder WithType(AttributeType type);

        DealAttributeBuilder WithKey(string key);

        DealAttributeBuilder AsDeleted();

        Task<DealAttribute> BuildAsync();
    }
}