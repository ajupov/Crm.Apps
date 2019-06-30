using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Crm.Apps.Tests.Creator;
using Crm.Clients.Companies.Clients;
using Crm.Clients.Companies.Models;
using Crm.Common.Types;
using Crm.Utils.DateTime;
using Xunit;

namespace Crm.Apps.Tests.Tests.Companies
{
    public class CompanyAttributesTests
    {
        private readonly ICreate _create;

        private readonly ICompanyAttributesClient _companyAttributesClient;

        public CompanyAttributesTests(ICreate create, ICompanyAttributesClient companyAttributesClient)
        {
            _create = create;
            _companyAttributesClient = companyAttributesClient;
        }

        [Fact]
        public async Task WhenGetTypes_ThenSuccess()
        {
            var types = await _companyAttributesClient.GetTypesAsync().ConfigureAwait(false);

            Assert.NotEmpty(types);
        }

        [Fact]
        public async Task WhenGet_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            var attributeId = (await _create.CompanyAttribute.WithAccountId(account.Id).BuildAsync().ConfigureAwait(false))
                .Id;

            var attribute = await _companyAttributesClient.GetAsync(attributeId).ConfigureAwait(false);

            Assert.NotNull(attribute);
            Assert.Equal(attributeId, attribute.Id);
        }

        [Fact]
        public async Task WhenGetList_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            var attributeIds = (await Task.WhenAll(
                    _create.CompanyAttribute.WithAccountId(account.Id).WithKey("Test1").BuildAsync(),
                    _create.CompanyAttribute.WithAccountId(account.Id).WithKey("Test2").BuildAsync())
                .ConfigureAwait(false)).Select(x => x.Id).ToList();

            var attributes = await _companyAttributesClient.GetListAsync(attributeIds).ConfigureAwait(false);

            Assert.NotEmpty(attributes);
            Assert.Equal(attributeIds.Count, attributes.Count);
        }

        [Fact]
        public async Task WhenGetPagedList_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            await Task.WhenAll(_create.CompanyAttribute.WithAccountId(account.Id).WithType(AttributeType.Text)
                .WithKey("Test1").BuildAsync()).ConfigureAwait(false);
            var filterTypes = new List<AttributeType> {AttributeType.Text};

            var attributes = await _companyAttributesClient.GetPagedListAsync(account.Id, key: "Test1",
                types: filterTypes,
                sortBy: "CreateDateTime", orderBy: "desc").ConfigureAwait(false);

            var results = attributes.Skip(1).Zip(attributes,
                (previous, current) => current.CreateDateTime >= previous.CreateDateTime);

            Assert.NotEmpty(attributes);
            Assert.All(results, Assert.True);
        }

        [Fact]
        public async Task WhenCreate_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            var attribute = new CompanyAttribute
            {
                AccountId = account.Id,
                Type = AttributeType.Text,
                Key = "Test",
                IsDeleted = false
            };

            var createdAttributeId = await _companyAttributesClient.CreateAsync(attribute).ConfigureAwait(false);

            var createdAttribute = await _companyAttributesClient.GetAsync(createdAttributeId).ConfigureAwait(false);

            Assert.NotNull(createdAttribute);
            Assert.Equal(createdAttributeId, createdAttribute.Id);
            Assert.Equal(attribute.AccountId, createdAttribute.AccountId);
            Assert.Equal(attribute.Type, createdAttribute.Type);
            Assert.Equal(attribute.Key, createdAttribute.Key);
            Assert.Equal(attribute.IsDeleted, createdAttribute.IsDeleted);
            Assert.True(createdAttribute.CreateDateTime.IsMoreThanMinValue());
        }

        [Fact]
        public async Task WhenUpdate_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            var attribute = await _create.CompanyAttribute.WithAccountId(account.Id).WithType(AttributeType.Text)
                .WithKey("Test").BuildAsync().ConfigureAwait(false);

            attribute.Type = AttributeType.Link;
            attribute.Key = "test.com";
            attribute.IsDeleted = true;

            await _companyAttributesClient.UpdateAsync(attribute).ConfigureAwait(false);

            var updatedAttribute = await _companyAttributesClient.GetAsync(attribute.Id).ConfigureAwait(false);

            Assert.Equal(attribute.Type, updatedAttribute.Type);
            Assert.Equal(attribute.Key, updatedAttribute.Key);
            Assert.Equal(attribute.IsDeleted, updatedAttribute.IsDeleted);
        }

        [Fact]
        public async Task WhenDelete_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            var attributeIds = (await Task.WhenAll(
                    _create.CompanyAttribute.WithAccountId(account.Id).WithKey("Test1").BuildAsync(),
                    _create.CompanyAttribute.WithAccountId(account.Id).WithKey("Test2").BuildAsync())
                .ConfigureAwait(false)).Select(x => x.Id).ToList();

            await _companyAttributesClient.DeleteAsync(attributeIds).ConfigureAwait(false);

            var attributes = await _companyAttributesClient.GetListAsync(attributeIds).ConfigureAwait(false);

            Assert.All(attributes, x => Assert.True(x.IsDeleted));
        }

        [Fact]
        public async Task WhenRestore_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync().ConfigureAwait(false);
            var attributeIds = (await Task.WhenAll(
                    _create.CompanyAttribute.WithAccountId(account.Id).WithKey("Test1").BuildAsync(),
                    _create.CompanyAttribute.WithAccountId(account.Id).WithKey("Test2").BuildAsync())
                .ConfigureAwait(false)).Select(x => x.Id).ToList();

            await _companyAttributesClient.RestoreAsync(attributeIds).ConfigureAwait(false);

            var attributes = await _companyAttributesClient.GetListAsync(attributeIds).ConfigureAwait(false);

            Assert.All(attributes, x => Assert.False(x.IsDeleted));
        }
    }
}