using System;
using System.Threading.Tasks;
using Crm.Clients.Leads.Clients;
using Crm.Clients.Leads.Models;
using Crm.Utils.Guid;

namespace Crm.Apps.Tests.Builders.Leads
{
    public class LeadCommentBuilder : ILeadCommentBuilder
    {
        private readonly ILeadCommentsClient _leadCommentsClient;
        private readonly LeadComment _leadComment;

        public LeadCommentBuilder(ILeadCommentsClient leadCommentsClient)
        {
            _leadCommentsClient = leadCommentsClient;
            _leadComment = new LeadComment
            {
                LeadId = Guid.Empty,
                CommentatorUserId = Guid.Empty,
                Value = "Test"
            };
        }

        public LeadCommentBuilder WithLeadId(Guid leadId)
        {
            _leadComment.LeadId = leadId;

            return this;
        }

        public Task BuildAsync()
        {
            if (_leadComment.LeadId.IsEmpty())
            {
                throw new InvalidOperationException(nameof(_leadComment.LeadId));
            }

            return _leadCommentsClient.CreateAsync(_leadComment);
        }
    }
}