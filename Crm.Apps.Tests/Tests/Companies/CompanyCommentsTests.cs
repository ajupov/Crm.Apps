using System.Linq;
using System.Threading.Tasks;
using Ajupov.Utils.All.DateTime;
using Ajupov.Utils.All.Guid;
using Crm.Apps.Tests.Services.Creator;
using Crm.Apps.v1.Clients.Companies.Clients;
using Crm.Apps.v1.Clients.Companies.Models;
using Crm.Apps.v1.Clients.Companies.RequestParameters;
using Xunit;

namespace Crm.Apps.Tests.Tests.Companies
{
    public class CompanyCommentsTests
    {
        private readonly ICreate _create;
        private readonly ICompanyCommentsClient _companyCommentsClient;

        public CompanyCommentsTests(ICreate create, ICompanyCommentsClient companyCommentsClient)
        {
            _create = create;
            _companyCommentsClient = companyCommentsClient;
        }

        [Fact]
        public async Task WhenGetPagedList_ThenSuccess()
        {
            var leadSource = await _create.LeadSource.BuildAsync();
            var lead = await _create.Lead
                .WithSourceId(leadSource.Id)
                .BuildAsync();
            var company = await _create.Company
                .WithLeadId(lead.Id)
                .BuildAsync();

            await Task.WhenAll(
                _create.CompanyComment
                    .WithCompanyId(company.Id)
                    .BuildAsync(),
                _create.CompanyComment
                    .WithCompanyId(company.Id)
                    .BuildAsync());

            var request = new CompanyCommentGetPagedListRequestParameter
            {
                CompanyId = company.Id,
                SortBy = "CreateDateTime",
                OrderBy = "desc"
            };

            var comments = await _companyCommentsClient.GetPagedListAsync(request);
            var results = comments
                .Skip(1)
                .Zip(comments, (previous, current) => current.CreateDateTime >= previous.CreateDateTime);

            Assert.NotEmpty(comments);
            Assert.All(results, Assert.True);
        }

        [Fact]
        public async Task WhenCreate_ThenSuccess()
        {
            var leadSource = await _create.LeadSource.BuildAsync();
            var lead = await _create.Lead
                .WithSourceId(leadSource.Id)
                .BuildAsync();
            var company = await _create.Company
                .WithLeadId(lead.Id)
                .BuildAsync();

            var comment = new CompanyComment
            {
                CompanyId = company.Id,
                Value = "Test"
            };

            await _companyCommentsClient.CreateAsync(comment);

            var request = new CompanyCommentGetPagedListRequestParameter
            {
                CompanyId = company.Id,
                SortBy = "CreateDateTime",
                OrderBy = "desc"
            };

            var createdComment = (await _companyCommentsClient.GetPagedListAsync(request)).First();

            Assert.NotNull(createdComment);
            Assert.Equal(comment.CompanyId, createdComment.CompanyId);
            Assert.True(!createdComment.CommentatorUserId.IsEmpty());
            Assert.Equal(comment.Value, createdComment.Value);
            Assert.True(createdComment.CreateDateTime.IsMoreThanMinValue());
        }
    }
}