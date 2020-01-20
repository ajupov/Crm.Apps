using System.Linq;
using System.Threading.Tasks;
using Ajupov.Utils.All.DateTime;
using Ajupov.Utils.All.Guid;
using Crm.Apps.Clients.Deals.Clients;
using Crm.Apps.Clients.Deals.Models;
using Crm.Apps.Clients.Deals.RequestParameters;
using Crm.Apps.Tests.Creator;
using Xunit;

namespace Crm.Apps.Tests.Tests.Deals
{
    public class DealCommentsTests
    {
        private readonly ICreate _create;
        private readonly IDealCommentsClient _dealCommentsClient;

        public DealCommentsTests(ICreate create, IDealCommentsClient dealCommentsClient)
        {
            _create = create;
            _dealCommentsClient = dealCommentsClient;
        }

        [Fact]
        public async Task WhenGetPagedList_ThenSuccess()
        {
            var type = await _create.DealType.BuildAsync();
            var status = await _create.DealStatus.BuildAsync();
            var deal = await _create.Deal
                .WithTypeId(type.Id)
                .WithStatusId(status.Id)
                .BuildAsync();
            await Task.WhenAll(
                _create.DealComment
                    .WithDealId(deal.Id)
                    .BuildAsync(),
                _create.DealComment
                    .WithDealId(deal.Id)
                    .BuildAsync());

            var request = new DealCommentGetPagedListRequestParameter
            {
                DealId = deal.Id,
            };

            var comments = await _dealCommentsClient.GetPagedListAsync(request);

            var results = comments
                .Skip(1)
                .Zip(comments, (previous, current) => current.CreateDateTime >= previous.CreateDateTime);

            Assert.NotEmpty(comments);
            Assert.All(results, Assert.True);
        }

        [Fact]
        public async Task WhenCreate_ThenSuccess()
        {
            var type = await _create.DealType.BuildAsync();
            var status = await _create.DealStatus.BuildAsync();
            var deal = await _create.Deal
                .WithTypeId(type.Id)
                .WithStatusId(status.Id)
                .BuildAsync();

            var comment = new DealComment
            {
                DealId = deal.Id,
                Value = "Test"
            };

            await _dealCommentsClient.CreateAsync(comment);

            var request = new DealCommentGetPagedListRequestParameter
            {
                DealId = deal.Id,
                SortBy = "CreateDateTime",
                OrderBy = "asc"
            };

            var createdComment = (await _dealCommentsClient.GetPagedListAsync(request)).First();

            Assert.NotNull(createdComment);
            Assert.Equal(comment.DealId, createdComment.DealId);
            Assert.True(!createdComment.CommentatorUserId.IsEmpty());
            Assert.Equal(comment.Value, createdComment.Value);
            Assert.True(createdComment.CreateDateTime.IsMoreThanMinValue());
        }
    }
}