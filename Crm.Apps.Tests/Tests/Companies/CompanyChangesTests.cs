using System.Linq;
using System.Threading.Tasks;
using Ajupov.Utils.All.DateTime;
using Ajupov.Utils.All.Guid;
using Ajupov.Utils.All.Json;
using Ajupov.Utils.All.String;
using Crm.Apps.Tests.Services.Creator;
using Crm.Apps.v1.Clients.Companies.Clients;
using Crm.Apps.v1.Clients.Companies.Models;
using Crm.Apps.v1.Clients.Companies.RequestParameters;
using Xunit;

namespace Crm.Apps.Tests.Tests.Companies
{
    public class CompanyChangesTests
    {
        private readonly ICreate _create;
        private readonly ICompaniesClient _companiesClient;
        private readonly ICompanyChangesClient _companyChangesClient;

        public CompanyChangesTests(
            ICreate create,
            ICompaniesClient companiesClient,
            ICompanyChangesClient companyChangesClient)
        {
            _create = create;
            _companiesClient = companiesClient;
            _companyChangesClient = companyChangesClient;
        }

        [Fact]
        public async Task WhenGetPagedList_ThenSuccess()
        {
            var leadSource = await _create.LeadSource.BuildAsync();
            var lead = await _create.Lead
                .WithSourceId(leadSource.Id)
                .BuildAsync();
            var company = await _create.Company
                .WithLeadId(lead.Id)
                .BuildAsync();

            company.IsDeleted = true;

            await _companiesClient.UpdateAsync(company);

            var request = new CompanyChangeGetPagedListRequestParameter
            {
                CompanyId = company.Id,
                SortBy = "CreateDateTime",
                OrderBy = "asc"
            };

            var changes = await _companyChangesClient.GetPagedListAsync(request);

            Assert.NotEmpty(changes);
            Assert.True(changes.All(x => !x.ChangerUserId.IsEmpty()));
            Assert.True(changes.All(x => x.CompanyId == company.Id));
            Assert.True(changes.All(x => x.CreateDateTime.IsMoreThanMinValue()));
            Assert.True(changes.First().OldValueJson.IsEmpty());
            Assert.True(!changes.First().NewValueJson.IsEmpty());
            Assert.NotNull(changes.First().NewValueJson.FromJsonString<Company>());
            Assert.True(!changes.Last().OldValueJson.IsEmpty());
            Assert.True(!changes.Last().NewValueJson.IsEmpty());
            Assert.False(changes.Last().OldValueJson.FromJsonString<Company>().IsDeleted);
            Assert.True(changes.Last().NewValueJson.FromJsonString<Company>().IsDeleted);
        }
    }
}