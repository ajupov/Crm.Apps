using System;
using System.Threading.Tasks;

namespace Crm.Apps.Tests.Dsl.Builders.LeadComment
{
    public interface ILeadCommentBuilder
    {
        LeadCommentBuilder WithLeadId(Guid leadId);
        
        Task<Guid> BuildAsync();
    }
}