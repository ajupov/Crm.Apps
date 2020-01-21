using System;
using System.Threading.Tasks;
using Ajupov.Utils.All.Guid;
using Crm.Apps.v1.Clients.Deals.Clients;
using Crm.Apps.v1.Clients.Deals.Models;

namespace Crm.Apps.Tests.Builders.Deals
{
    public class DealCommentBuilder : IDealCommentBuilder
    {
        private readonly IDealCommentsClient _dealCommentsClient;
        private readonly DealComment _comment;

        public DealCommentBuilder(IDealCommentsClient dealCommentsClient)
        {
            _dealCommentsClient = dealCommentsClient;
            _comment = new DealComment
            {
                DealId = Guid.Empty,
                Value = "Test"
            };
        }

        public DealCommentBuilder WithDealId(Guid dealId)
        {
            _comment.DealId = dealId;

            return this;
        }

        public Task BuildAsync()
        {
            if (_comment.DealId.IsEmpty())
            {
                throw new InvalidOperationException(nameof(_comment.DealId));
            }

            return _dealCommentsClient.CreateAsync(_comment);
        }
    }
}