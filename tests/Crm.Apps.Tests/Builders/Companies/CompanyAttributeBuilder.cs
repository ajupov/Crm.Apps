using System;
using System.Threading.Tasks;
using Crm.Clients.Companies.Clients;
using Crm.Clients.Companies.Models;
using Crm.Common.Types;
using Crm.Utils.Guid;

namespace Crm.Apps.Tests.Builders.Companies
{
    public class CompanyAttributeBuilder : ICompanyAttributeBuilder
    {
        private readonly ICompanyAttributesClient _companyAttributesClient;
        private readonly CompanyAttribute _companyAttribute;

        public CompanyAttributeBuilder(ICompanyAttributesClient companyAttributesClient)
        {
            _companyAttributesClient = companyAttributesClient;
            _companyAttribute = new CompanyAttribute
            {
                AccountId = Guid.Empty,
                Type = AttributeType.Text,
                Key = "Test"
            };
        }

        public CompanyAttributeBuilder WithAccountId(Guid accountId)
        {
            _companyAttribute.AccountId = accountId;

            return this;
        }

        public CompanyAttributeBuilder WithType(AttributeType type)
        {
            _companyAttribute.Type = type;

            return this;
        }

        public CompanyAttributeBuilder WithKey(string key)
        {
            _companyAttribute.Key = key;

            return this;
        }

        public async Task<CompanyAttribute> BuildAsync()
        {
            if (_companyAttribute.AccountId.IsEmpty())
            {
                throw new InvalidOperationException(nameof(_companyAttribute.AccountId));
            }

            var createdId = await _companyAttributesClient.CreateAsync(_companyAttribute).ConfigureAwait(false);

            return await _companyAttributesClient.GetAsync(createdId).ConfigureAwait(false);
        }
    }
}