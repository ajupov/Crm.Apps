using System;
using System.Threading.Tasks;

namespace Crm.Apps.Tests.Builders.Leads
{
    public interface ILeadCommentBuilder
    {
        LeadCommentBuilder WithLeadId(Guid leadId);

        Task BuildAsync();
    }
}