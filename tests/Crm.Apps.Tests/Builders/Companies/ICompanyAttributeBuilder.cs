using System;
using System.Threading.Tasks;
using Crm.Common.Types;

namespace Crm.Apps.Tests.Builders.Companies
{
    public interface ICompanyAttributeBuilder
    {
        CompanyAttributeBuilder WithAccountId(Guid accountId);

        CompanyAttributeBuilder WithType(AttributeType type);

        CompanyAttributeBuilder WithKey(string key);

        Task<Clients.Companies.Models.CompanyAttribute> BuildAsync();
    }
}