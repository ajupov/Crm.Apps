using System.Linq;
using System.Threading.Tasks;
using Crm.Apps.Tests.Creator;
using Crm.Clients.Deals.Clients;
using Crm.Clients.Deals.Models;
using Crm.Utils.DateTime;
using Crm.Utils.Guid;
using Crm.Utils.Json;
using Crm.Utils.String;
using Xunit;

namespace Crm.Apps.Tests.Tests.Deals
{
    public class DealTypeChangesTests
    {
        private readonly ICreate _create;
        private readonly IDealTypesClient _dealTypesClient;
        private readonly IDealTypeChangesClient _typeChangesClient;

        public DealTypeChangesTests(ICreate create, IDealTypesClient dealTypesClient,
            IDealTypeChangesClient typeChangesClient)
        {
            _create = create;
            _dealTypesClient = dealTypesClient;
            _typeChangesClient = typeChangesClient;
        }

        [Fact]
        public async Task WhenGetPagedList_ThenSuccess()
        {
            var account = await _create.Account.BuildAsync();
            var type = await _create.DealType.WithAccountId(account.Id).BuildAsync();
            type.Name = "Test2";
            type.IsDeleted = true;
            await _dealTypesClient.UpdateAsync(type);

            var changes = await _typeChangesClient
                .GetPagedListAsync(typeId: type.Id, sortBy: "CreateDateTime", orderBy: "asc")
                ;

            Assert.NotEmpty(changes);
            Assert.True(changes.All(x => !x.ChangerUserId.IsEmpty()));
            Assert.True(changes.All(x => x.TypeId == type.Id));
            Assert.True(changes.All(x => x.CreateDateTime.IsMoreThanMinValue()));
            Assert.True(changes.First().OldValueJson.IsEmpty());
            Assert.True(!changes.First().NewValueJson.IsEmpty());
            Assert.NotNull(changes.First().NewValueJson.FromJsonString<DealType>());
            Assert.True(!changes.Last().OldValueJson.IsEmpty());
            Assert.True(!changes.Last().NewValueJson.IsEmpty());
            Assert.False(changes.Last().OldValueJson.FromJsonString<DealType>().IsDeleted);
            Assert.True(changes.Last().NewValueJson.FromJsonString<DealType>().IsDeleted);
            Assert.Equal(changes.Last().NewValueJson.FromJsonString<DealType>().Name, type.Name);
        }
    }
}