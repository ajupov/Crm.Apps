using System;
using System.Threading.Tasks;
using Crm.Apps.Tests.Extensions;
using Crm.Apps.Tests.Services.AccessTokenGetter;
using Crm.Apps.v1.Clients.Companies.Clients;
using Crm.Apps.v1.Clients.Companies.Models;
using Crm.Common.All.Types.AttributeType;

namespace Crm.Apps.Tests.Builders.Companies
{
    public class CompanyAttributeBuilder : ICompanyAttributeBuilder
    {
        private readonly IAccessTokenGetter _accessTokenGetter;
        private readonly ICompanyAttributesClient _companyAttributesClient;
        private readonly CompanyAttribute _companyAttribute;

        public CompanyAttributeBuilder(
            IAccessTokenGetter accessTokenGetter,
            ICompanyAttributesClient companyAttributesClient)
        {
            _companyAttributesClient = companyAttributesClient;
            _accessTokenGetter = accessTokenGetter;
            _companyAttribute = new CompanyAttribute
            {
                AccountId = Guid.Empty,
                Type = AttributeType.Text,
                Key = "Test".WithGuid(),
                IsDeleted = false
            };
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

        public CompanyAttributeBuilder AsDeleted()
        {
            _companyAttribute.IsDeleted = true;

            return this;
        }

        public async Task<CompanyAttribute> BuildAsync()
        {
            var accessToken = await _accessTokenGetter.GetAsync();

            var id = await _companyAttributesClient.CreateAsync(accessToken, _companyAttribute);

            return await _companyAttributesClient.GetAsync(accessToken, id);
        }
    }
}