using System;
using System.Threading.Tasks;
using Ajupov.Utils.All.Guid;
using Crm.Apps.v1.Clients.Leads.Clients;
using Crm.Apps.v1.Clients.Leads.Models;

namespace Crm.Apps.Tests.Builders.Leads
{
    public class LeadCommentBuilder : ILeadCommentBuilder
    {
        private readonly ILeadCommentsClient _leadCommentsClient;
        private readonly LeadComment _comment;

        public LeadCommentBuilder(ILeadCommentsClient leadCommentsClient)
        {
            _leadCommentsClient = leadCommentsClient;
            _comment = new LeadComment
            {
                LeadId = Guid.Empty,
                Value = "Test"
            };
        }

        public LeadCommentBuilder WithLeadId(Guid leadId)
        {
            _comment.LeadId = leadId;

            return this;
        }

        public Task BuildAsync()
        {
            if (_comment.LeadId.IsEmpty())
            {
                throw new InvalidOperationException(nameof(_comment.LeadId));
            }

            return _leadCommentsClient.CreateAsync(_comment);
        }
    }
}