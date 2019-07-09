using System;
using System.Threading.Tasks;
using Crm.Clients.Companies.Clients;
using Crm.Clients.Companies.Models;
using Crm.Utils.Guid;

namespace Crm.Apps.Tests.Builders.Companies
{
    public class CompanyCommentBuilder : ICompanyCommentBuilder
    {
        private readonly ICompanyCommentsClient _companyCommentsClient;
        private readonly CompanyComment _companyComment;

        public CompanyCommentBuilder(ICompanyCommentsClient companyCommentsClient)
        {
            _companyCommentsClient = companyCommentsClient;
            _companyComment = new CompanyComment
            {
                CompanyId = Guid.Empty,
                CommentatorUserId = Guid.Empty,
                Value = "Test"
            };
        }

        public CompanyCommentBuilder WithCompanyId(Guid companyId)
        {
            _companyComment.CompanyId = companyId;

            return this;
        }

        public Task BuildAsync()
        {
            if (_companyComment.CompanyId.IsEmpty())
            {
                throw new InvalidOperationException(nameof(_companyComment.CompanyId));
            }

            return _companyCommentsClient.CreateAsync(_companyComment);
        }
    }
}