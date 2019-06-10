using System.Linq;
using System.Threading.Tasks;
using Crm.Apps.Tests.Dsl.Creator;
using Crm.Clients.Identities.Clients;
using Crm.Clients.Identities.Models;
using Crm.Utils.DateTime;
using Crm.Utils.Guid;
using Crm.Utils.Json;
using Crm.Utils.String;
using Xunit;

namespace Crm.Apps.Tests.Tests.Identities
{
    public class IdentityChangesTests
    {
        private readonly ICreate _create;
        private readonly IIdentitiesClient _identitiesClient;
        private readonly IIdentityChangesClient _identityChangesClient;

        public IdentityChangesTests(ICreate create, IIdentitiesClient identitiesClient,
            IIdentityChangesClient identityChangesClient)
        {
            _create = create;
            _identitiesClient = identitiesClient;
            _identityChangesClient = identityChangesClient;
        }

        [Fact]
        public async Task WhenGetPagedList_ThenSuccess()
        {
            var identity = await _create.Identity.BuildAsync().ConfigureAwait(false);
            identity.IsPrimary = true;
            identity.IsVerified = true;
            await _identitiesClient.UpdateAsync(identity).ConfigureAwait(false);

            var changes = await _identityChangesClient
                .GetPagedListAsync(identityId: identity.Id, sortBy: "CreateDateTime", orderBy: "asc")
                .ConfigureAwait(false);

            Assert.NotEmpty(changes);
            Assert.True(changes.All(x => !x.ChangerUserId.IsEmpty()));
            Assert.True(changes.All(x => x.IdentityId == identity.Id));
            Assert.True(changes.All(x => x.CreateDateTime.IsMoreThanMinValue()));
            Assert.True(changes.First().OldValueJson.IsEmpty());
            Assert.True(!changes.First().NewValueJson.IsEmpty());
            Assert.NotNull(changes.First().NewValueJson.FromJsonString<Identity>());
            Assert.True(!changes.Last().OldValueJson.IsEmpty());
            Assert.True(!changes.Last().NewValueJson.IsEmpty());
            Assert.False(changes.Last().OldValueJson.FromJsonString<Identity>().IsPrimary);
            Assert.True(changes.Last().NewValueJson.FromJsonString<Identity>().IsPrimary);
            Assert.False(changes.Last().OldValueJson.FromJsonString<Identity>().IsVerified);
            Assert.True(changes.Last().NewValueJson.FromJsonString<Identity>().IsVerified);
        }
    }
}