using System.Linq;
using System.Threading.Tasks;
using Crm.Apps.Tests.Creator;
using Crm.Clients.Leads.Clients;
using Crm.Clients.Leads.Models;
using Crm.Utils.DateTime;
using Crm.Utils.Guid;
using Xunit;

namespace Crm.Apps.Tests.Tests.Leads
{
    public class LeadCommentsTests
    {
        private readonly ICreate _create;
        private readonly ILeadCommentsClient _leadCommentsClient;

        public LeadCommentsTests(ICreate create, ILeadCommentsClient leadCommentsClient)
        {
            _create = create;
            _leadCommentsClient = leadCommentsClient;
        }

        [Fact]
        public async Task WhenGetPagedList_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync();
            var source = await _create.LeadSource.WithAccountId(account.Id).BuildAsync();
            var lead = await _create.Lead.WithAccountId(account.Id).WithSourceId(source.Id).BuildAsync()
                ;
            await Task.WhenAll(
                    _create.LeadComment.WithLeadId(lead.Id).BuildAsync(),
                    _create.LeadComment.WithLeadId(lead.Id).BuildAsync())
                ;

            var comments = await _leadCommentsClient
                .GetPagedListAsync(lead.Id, sortBy: "CreateDateTime", orderBy: "desc");

            var results = comments.Skip(1)
                .Zip(comments, (previous, current) => current.CreateDateTime >= previous.CreateDateTime);

            Assert.NotEmpty(comments);
            Assert.All(results, Assert.True);
        }

        [Fact]
        public async Task WhenCreate_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync();
            var source = await _create.LeadSource.WithAccountId(account.Id).BuildAsync();
            var lead = await _create.Lead.WithAccountId(account.Id).WithSourceId(source.Id).BuildAsync()
                ;

            var comment = new LeadComment
            {
                LeadId = lead.Id,
                Value = "Test"
            };

            await _leadCommentsClient.CreateAsync(comment);

            var createdComment = (await _leadCommentsClient.GetPagedListAsync(lead.Id, sortBy: "CreateDateTime",
                orderBy: "asc")).First();

            Assert.NotNull(createdComment);
            Assert.Equal(comment.LeadId, createdComment.LeadId);
            Assert.True(!createdComment.CommentatorUserId.IsEmpty());
            Assert.Equal(comment.Value, createdComment.Value);
            Assert.True(createdComment.CreateDateTime.IsMoreThanMinValue());
        }
    }
}