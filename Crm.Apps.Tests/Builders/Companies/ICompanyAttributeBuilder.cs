using System;
using System.Threading.Tasks;
using Crm.Apps.Clients.Companies.Models;
using Crm.Common.All.Types.AttributeType;

namespace Crm.Apps.Tests.Builders.Companies
{
    public interface ICompanyAttributeBuilder
    {
        CompanyAttributeBuilder WithAccountId(Guid accountId);

        CompanyAttributeBuilder WithType(AttributeType type);

        CompanyAttributeBuilder WithKey(string key);

        CompanyAttributeBuilder AsDeleted();

        Task<CompanyAttribute> BuildAsync();
    }
}