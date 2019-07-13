using System.Linq;
using System.Threading.Tasks;
using Crm.Apps.Tests.Creator;
using Crm.Clients.Deals.Clients;
using Crm.Clients.Deals.Models;
using Crm.Utils.DateTime;
using Crm.Utils.Guid;
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
            var account = await _create.Account.BuildAsync();
            var type = await _create.DealType.WithAccountId(account.Id).BuildAsync();
            var status = await _create.DealStatus.WithAccountId(account.Id).BuildAsync();
            var deal = await _create.Deal.WithAccountId(account.Id).WithTypeId(type.Id).WithStatusId(status.Id)
                .BuildAsync();
            await Task.WhenAll(
                    _create.DealComment.WithDealId(deal.Id).BuildAsync(),
                    _create.DealComment.WithDealId(deal.Id).BuildAsync())
                ;

            var comments = await _dealCommentsClient
                .GetPagedListAsync(deal.Id, sortBy: "CreateDateTime", orderBy: "desc");

            var results = comments.Skip(1)
                .Zip(comments, (previous, current) => current.CreateDateTime >= previous.CreateDateTime);

            Assert.NotEmpty(comments);
            Assert.All(results, Assert.True);
        }

        [Fact]
        public async Task WhenCreate_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync();
            var type = await _create.DealType.WithAccountId(account.Id).BuildAsync();
            var status = await _create.DealStatus.WithAccountId(account.Id).BuildAsync();
            var deal = await _create.Deal.WithAccountId(account.Id).WithTypeId(type.Id).WithStatusId(status.Id)
                .BuildAsync();

            var comment = new DealComment
            {
                DealId = deal.Id,
                Value = "Test"
            };

            await _dealCommentsClient.CreateAsync(comment);

            var createdComment = (await _dealCommentsClient.GetPagedListAsync(deal.Id, sortBy: "CreateDateTime",
                orderBy: "asc")).First();

            Assert.NotNull(createdComment);
            Assert.Equal(comment.DealId, createdComment.DealId);
            Assert.True(!createdComment.CommentatorUserId.IsEmpty());
            Assert.Equal(comment.Value, createdComment.Value);
            Assert.True(createdComment.CreateDateTime.IsMoreThanMinValue());
        }
    }
}