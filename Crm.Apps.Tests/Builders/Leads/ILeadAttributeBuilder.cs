using System.Threading.Tasks;
using Crm.Apps.v1.Clients.Leads.Models;
using Crm.Common.All.Types.AttributeType;

namespace Crm.Apps.Tests.Builders.Leads
{
    public interface ILeadAttributeBuilder
    {
        LeadAttributeBuilder WithType(AttributeType type);

        LeadAttributeBuilder WithKey(string key);

        LeadAttributeBuilder AsDeleted();

        Task<LeadAttribute> BuildAsync();
    }
}