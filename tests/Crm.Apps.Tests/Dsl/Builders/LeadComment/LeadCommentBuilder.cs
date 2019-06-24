using System;
using System.Threading.Tasks;
using Crm.Clients.Leads.Clients;
using Crm.Utils.Guid;

namespace Crm.Apps.Tests.Dsl.Builders.LeadComment
{
    public class LeadCommentBuilder : ILeadCommentBuilder
    {
        private readonly Clients.Leads.Models.LeadComment _leadComment;
        private readonly ILeadCommentsClient _leadCommentsClient;

        public LeadCommentBuilder(ILeadCommentsClient leadCommentsClient)
        {
            _leadCommentsClient = leadCommentsClient;
            _leadComment = new Clients.Leads.Models.LeadComment
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