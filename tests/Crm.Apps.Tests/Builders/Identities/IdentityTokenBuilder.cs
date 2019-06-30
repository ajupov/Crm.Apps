using System;
using System.Threading.Tasks;
using Crm.Clients.Identities.Clients;
using Crm.Utils.Guid;
using Crm.Utils.String;

namespace Crm.Apps.Tests.Builders.Identities
{
    public class IdentityTokenBuilder : IIdentityTokenBuilder
    {
        private readonly Clients.Identities.Models.IdentityToken _token;
        private readonly IIdentityTokensClient _identityTokensClient;

        public IdentityTokenBuilder(IIdentityTokensClient identityTokensClient)
        {
            _identityTokensClient = identityTokensClient;
            _token = new Clients.Identities.Models.IdentityToken
            {
                Value = "Test",
                ExpirationDateTime = DateTime.Now.AddDays(1)
            };
        }

        public IdentityTokenBuilder WithIdentityId(Guid identityId)
        {
            _token.IdentityId = identityId;

            return this;
        }

        public IdentityTokenBuilder WithType(string value)
        {
            _token.Value = value;

            return this;
        }

        public IdentityTokenBuilder WithExpirationDateTime(DateTime expirationDateTime)
        {
            _token.ExpirationDateTime = expirationDateTime;

            return this;
        }

        public async Task<Clients.Identities.Models.IdentityToken> BuildAsync()
        {
            if (_token.IdentityId.IsEmpty())
            {
                throw new InvalidOperationException(nameof(_token.IdentityId));
            }

            if (_token.Value.IsEmpty())
            {
                throw new InvalidOperationException(nameof(_token.IdentityId));
            }

            await _identityTokensClient.CreateAsync(_token).ConfigureAwait(false);

            return await _identityTokensClient.GetAsync(_token.IdentityId, _token.Value).ConfigureAwait(false);
        }
    }
}