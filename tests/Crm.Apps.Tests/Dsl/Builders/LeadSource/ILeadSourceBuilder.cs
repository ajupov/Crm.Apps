using System;
using System.Threading.Tasks;

namespace Crm.Apps.Tests.Dsl.Builders.LeadSource
{
    public interface ILeadSourceBuilder
    {
        LeadSourceBuilder WithAccountId(Guid accountId);

        LeadSourceBuilder WithName(string name);

        Task<Clients.Leads.Models.LeadSource> BuildAsync();
    }
}