using System.Linq;
using System.Threading.Tasks;
using Crm.Apps.Tests.Creator;
using Crm.Clients.Companies.Clients;
using Crm.Clients.Companies.Models;
using Crm.Utils.DateTime;
using Crm.Utils.Guid;
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
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            var company = await _create.Company.WithAccountId(account.Id).BuildAsync().ConfigureAwait(false);
            await Task.WhenAll(
                    _create.CompanyComment.WithCompanyId(company.Id).BuildAsync(),
                    _create.CompanyComment.WithCompanyId(company.Id).BuildAsync())
                .ConfigureAwait(false);

            var comments = await _companyCommentsClient
                .GetPagedListAsync(company.Id, sortBy: "CreateDateTime", orderBy: "desc").ConfigureAwait(false);

            var results = comments.Skip(1)
                .Zip(comments, (previous, current) => current.CreateDateTime >= previous.CreateDateTime);

            Assert.NotEmpty(comments);
            Assert.All(results, Assert.True);
        }

        [Fact]
        public async Task WhenCreate_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            var company = await _create.Company.WithAccountId(account.Id).BuildAsync().ConfigureAwait(false);

            var comment = new CompanyComment
            {
                CompanyId = company.Id,
                Value = "Test"
            };

            await _companyCommentsClient.CreateAsync(comment).ConfigureAwait(false);

            var createdComment = (await _companyCommentsClient.GetPagedListAsync(company.Id, sortBy: "CreateDateTime",
                orderBy: "asc").ConfigureAwait(false)).First();

            Assert.NotNull(createdComment);
            Assert.Equal(comment.CompanyId, createdComment.CompanyId);
            Assert.True(!createdComment.CommentatorUserId.IsEmpty());
            Assert.Equal(comment.Value, createdComment.Value);
            Assert.True(createdComment.CreateDateTime.IsMoreThanMinValue());
        }
    }
}