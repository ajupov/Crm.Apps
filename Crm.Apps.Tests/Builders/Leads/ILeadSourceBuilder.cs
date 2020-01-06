using System;
using System.Threading.Tasks;
using Crm.Apps.Clients.Leads.Models;

namespace Crm.Apps.Tests.Builders.Leads
{
    public interface ILeadSourceBuilder
    {
        LeadSourceBuilder WithAccountId(Guid accountId);

        LeadSourceBuilder WithName(string name);

        LeadSourceBuilder AsDeleted();

        Task<LeadSource> BuildAsync();
    }
}