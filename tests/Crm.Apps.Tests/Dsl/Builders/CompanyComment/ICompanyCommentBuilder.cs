using System;
using System.Threading.Tasks;

namespace Crm.Apps.Tests.Dsl.Builders.CompanyComment
{
    public interface ICompanyCommentBuilder
    {
        CompanyCommentBuilder WithCompanyId(Guid companyId);
        
        Task BuildAsync();
    }
}