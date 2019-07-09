using System;
using System.Threading.Tasks;
using Crm.Clients.Deals.Clients;
using Crm.Clients.Deals.Models;
using Crm.Utils.Guid;

namespace Crm.Apps.Tests.Builders.Deals
{
    public class DealCommentBuilder : IDealCommentBuilder
    {
        private readonly IDealCommentsClient _dealCommentsClient;
        private readonly DealComment _dealComment;

        public DealCommentBuilder(IDealCommentsClient dealCommentsClient)
        {
            _dealCommentsClient = dealCommentsClient;
            _dealComment = new DealComment
            {
                DealId = Guid.Empty,
                CommentatorUserId = Guid.Empty,
                Value = "Test"
            };
        }

        public DealCommentBuilder WithDealId(Guid dealId)
        {
            _dealComment.DealId = dealId;

            return this;
        }

        public Task BuildAsync()
        {
            if (_dealComment.DealId.IsEmpty())
            {
                throw new InvalidOperationException(nameof(_dealComment.DealId));
            }

            return _dealCommentsClient.CreateAsync(_dealComment);
        }
    }
}