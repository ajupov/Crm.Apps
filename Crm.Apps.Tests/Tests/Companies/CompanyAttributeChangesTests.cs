using System.Linq;
using System.Threading.Tasks;
using Ajupov.Utils.All.DateTime;
using Ajupov.Utils.All.Json;
using Crm.Apps.Clients.Companies.Clients;
using Crm.Apps.Clients.Companies.Models;
using Crm.Apps.Tests.Creator;
using Crm.Common.All.Types.AttributeType;
using Xunit;

namespace Crm.Apps.Tests.Tests.Companies
{
    public class CompanyAttributeChangesTests
    {
        private readonly ICreate _create;
        private readonly ICompanyAttributesClient _companyAttributesClient;
        private readonly ICompanyAttributeChangesClient _attributeChangesClient;

        public CompanyAttributeChangesTests(ICreate create, ICompanyAttributesClient companyAttributesClient,
            ICompanyAttributeChangesClient attributeChangesClient)
        {
            _create = create;
            _companyAttributesClient = companyAttributesClient;
            _attributeChangesClient = attributeChangesClient;
        }

        [Fact]
        public async Task WhenGetPagedList_ThenSuccess()
        {
            
            var attribute = await _create.CompanyAttribute.BuildAsync();
            attribute.Type = AttributeType.Link;
            attribute.Key = "TestLink";
            attribute.IsDeleted = true;
            await _companyAttributesClient.UpdateAsync(attribute);

            var changes = await _attributeChangesClient
                .GetPagedListAsync(attributeId: attribute.Id, sortBy: "CreateDateTime", orderBy: "asc")
                ;

            Assert.NotEmpty(changes);
            Assert.True(changes.All(x => !x.ChangerUserId.IsEmpty()));
            Assert.True(changes.All(x => x.AttributeId == attribute.Id));
            Assert.True(changes.All(x => x.CreateDateTime.IsMoreThanMinValue()));
            Assert.True(changes.First().OldValueJson.IsEmpty());
            Assert.True(!changes.First().NewValueJson.IsEmpty());
            Assert.NotNull(changes.First().NewValueJson.FromJsonString<CompanyAttribute>());
            Assert.True(!changes.Last().OldValueJson.IsEmpty());
            Assert.True(!changes.Last().NewValueJson.IsEmpty());
            Assert.False(changes.Last().OldValueJson.FromJsonString<CompanyAttribute>().IsDeleted);
            Assert.True(changes.Last().NewValueJson.FromJsonString<CompanyAttribute>().IsDeleted);
            Assert.Equal(changes.Last().NewValueJson.FromJsonString<CompanyAttribute>().Key, attribute.Key);
            Assert.Equal(changes.Last().NewValueJson.FromJsonString<CompanyAttribute>().Type, attribute.Type);
        }
    }
}