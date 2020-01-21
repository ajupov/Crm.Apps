using System;
using System.Threading.Tasks;
using Ajupov.Utils.All.Guid;
using Crm.Apps.v1.Clients.Companies.Clients;
using Crm.Apps.v1.Clients.Companies.Models;

namespace Crm.Apps.Tests.Builders.Companies
{
    public class CompanyCommentBuilder : ICompanyCommentBuilder
    {
        private readonly ICompanyCommentsClient _companyCommentsClient;
        private readonly CompanyComment _comment;

        public CompanyCommentBuilder(ICompanyCommentsClient companyCommentsClient)
        {
            _companyCommentsClient = companyCommentsClient;
            _comment = new CompanyComment
            {
                CompanyId = Guid.Empty,
                Value = "Test"
            };
        }

        public CompanyCommentBuilder WithCompanyId(Guid companyId)
        {
            _comment.CompanyId = companyId;

            return this;
        }

        public Task BuildAsync()
        {
            if (_comment.CompanyId.IsEmpty())
            {
                throw new InvalidOperationException(nameof(_comment.CompanyId));
            }

            return _companyCommentsClient.CreateAsync(_comment);
        }
    }
}