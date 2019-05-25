using System;
using System.Threading.Tasks;
using Crm.Clients.Identities.Clients.Identities;
using Crm.Clients.Identities.Models;
using Crm.Utils.Guid;

namespace Crm.Apps.Tests.Dsl.Builders.Identity
{
    public class IdentityBuilder : IIdentityBuilder
    {
        private readonly Clients.Identities.Models.Identity _identity;
        private readonly IIdentitiesClient _identitiesClient;

        public IdentityBuilder(IIdentitiesClient identitiesClient)
        {
            _identitiesClient = identitiesClient;
            _identity = new Clients.Identities.Models.Identity
            {
                Type = IdentityType.None,
                Key = "Test",
                PasswordHash = string.Empty,
                IsPrimary = false,
                IsVerified = false
            };
        }

        public IdentityBuilder WithUserId(Guid userId)
        {
            _identity.UserId = userId;

            return this;
        }

        public IdentityBuilder WithType(IdentityType type)
        {
            _identity.Type = type;

            return this;
        }

        public IdentityBuilder WithKey(string key)
        {
            _identity.Key = key;

            return this;
        }

        public IdentityBuilder WithPasswordHash(string passwordHash)
        {
            _identity.PasswordHash = passwordHash;

            return this;
        }

        public IdentityBuilder AsPrimary()
        {
            _identity.IsPrimary = true;

            return this;
        }

        public IdentityBuilder AsVerified()
        {
            _identity.IsVerified = true;

            return this;
        }

        public async Task<Clients.Identities.Models.Identity> BuildAsync()
        {
            if (_identity.UserId.IsEmpty())
            {
                throw new InvalidOperationException(nameof(_identity.UserId));
            }

            var createdId = await _identitiesClient.CreateAsync(_identity).ConfigureAwait(false);

            return await _identitiesClient.GetAsync(createdId).ConfigureAwait(false);
        }
    }
}