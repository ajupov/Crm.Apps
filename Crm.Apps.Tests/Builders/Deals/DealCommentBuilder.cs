using System;
using System.Threading.Tasks;
using Ajupov.Utils.All.Guid;
using Crm.Apps.Tests.Extensions;
using Crm.Apps.Tests.Services.AccessTokenGetter;
using Crm.Apps.v1.Clients.Deals.Clients;
using Crm.Apps.v1.Clients.Deals.Models;

namespace Crm.Apps.Tests.Builders.Deals
{
    public class DealCommentBuilder : IDealCommentBuilder
    {
        private readonly IAccessTokenGetter _accessTokenGetter;
        private readonly IDealCommentsClient _dealCommentsClient;
        private readonly DealComment _comment;

        public DealCommentBuilder(IAccessTokenGetter accessTokenGetter, IDealCommentsClient dealCommentsClient)
        {
            _dealCommentsClient = dealCommentsClient;
            _accessTokenGetter = accessTokenGetter;
            _comment = new DealComment
            {
                DealId = Guid.Empty,
                Value = "Test".WithGuid()
            };
        }

        public DealCommentBuilder WithDealId(Guid dealId)
        {
            _comment.DealId = dealId;

            return this;
        }

        public async Task BuildAsync()
        {
            var accessToken = await _accessTokenGetter.GetAsync();

            if (_comment.DealId.IsEmpty())
            {
                throw new InvalidOperationException(nameof(_comment.DealId));
            }

            await _dealCommentsClient.CreateAsync(accessToken, _comment);
        }
    }
}