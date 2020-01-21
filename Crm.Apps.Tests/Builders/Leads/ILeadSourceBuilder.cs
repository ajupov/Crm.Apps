using System.Threading.Tasks;
using Crm.Apps.v1.Clients.Leads.Models;

namespace Crm.Apps.Tests.Builders.Leads
{
    public interface ILeadSourceBuilder
    {
        LeadSourceBuilder WithName(string name);

        LeadSourceBuilder AsDeleted();

        Task<LeadSource> BuildAsync();
    }
}