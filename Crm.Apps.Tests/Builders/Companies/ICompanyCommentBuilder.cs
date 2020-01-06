using System;
using System.Threading.Tasks;

namespace Crm.Apps.Tests.Builders.Companies
{
    public interface ICompanyCommentBuilder
    {
        CompanyCommentBuilder WithCompanyId(Guid companyId);

        Task BuildAsync();
    }
}