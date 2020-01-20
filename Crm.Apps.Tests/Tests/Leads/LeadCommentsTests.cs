using System.Linq;
using System.Threading.Tasks;
using Ajupov.Utils.All.DateTime;
using Ajupov.Utils.All.Guid;
using Crm.Apps.Clients.Leads.Clients;
using Crm.Apps.Clients.Leads.Models;
using Crm.Apps.Clients.Leads.RequestParameters;
using Crm.Apps.Tests.Creator;
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
            var source = await _create.LeadSource.BuildAsync();
            var lead = await _create.Lead
                .WithSourceId(source.Id)
                .BuildAsync();
            await Task.WhenAll(
                _create.LeadComment
                    .WithLeadId(lead.Id)
                    .BuildAsync(),
                _create.LeadComment
                    .WithLeadId(lead.Id)
                    .BuildAsync());

            var request = new LeadCommentGetPagedListRequestParameter
            {
                LeadId = lead.Id
            };

            var comments = await _leadCommentsClient.GetPagedListAsync(request);

            var results = comments
                .Skip(1)
                .Zip(comments, (previous, current) => current.CreateDateTime >= previous.CreateDateTime);

            Assert.NotEmpty(comments);
            Assert.All(results, Assert.True);
        }

        [Fact]
        public async Task WhenCreate_ThenSuccess()
        {
            var source = await _create.LeadSource.BuildAsync();
            var lead = await _create.Lead
                .WithSourceId(source.Id)
                .BuildAsync();

            var comment = new LeadComment
            {
                LeadId = lead.Id,
                Value = "Test"
            };

            await _leadCommentsClient.CreateAsync(comment);

            var request = new LeadCommentGetPagedListRequestParameter
            {
                LeadId = lead.Id,
                SortBy = "CreateDateTime",
                OrderBy = "asc"
            };

            var createdComment = (await _leadCommentsClient.GetPagedListAsync(request)).First();

            Assert.NotNull(createdComment);
            Assert.Equal(comment.LeadId, createdComment.LeadId);
            Assert.True(!createdComment.CommentatorUserId.IsEmpty());
            Assert.Equal(comment.Value, createdComment.Value);
            Assert.True(createdComment.CreateDateTime.IsMoreThanMinValue());
        }
    }
}