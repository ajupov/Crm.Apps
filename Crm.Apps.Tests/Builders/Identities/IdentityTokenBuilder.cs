using System;
using System.Threading.Tasks;
using Crm.Clients.Identities.Clients;
using Crm.Clients.Identities.Models;
using Crm.Utils.Guid;
using Crm.Utils.String;

namespace Crm.Apps.Tests.Builders.Identities
{
    public class IdentityTokenBuilder : IIdentityTokenBuilder
    {
        private readonly IIdentityTokensClient _identityTokensClient;
        private readonly IdentityToken _token;

        public IdentityTokenBuilder(IIdentityTokensClient identityTokensClient)
        {
            _identityTokensClient = identityTokensClient;
            _token = new IdentityToken
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

        public async Task<IdentityToken> BuildAsync()
        {
            if (_token.IdentityId.IsEmpty())
            {
                throw new InvalidOperationException(nameof(_token.IdentityId));
            }

            if (_token.Value.IsEmpty())
            {
                throw new InvalidOperationException(nameof(_token.IdentityId));
            }

            await _identityTokensClient.CreateAsync(_token);

            return await _identityTokensClient.GetAsync(_token.IdentityId, _token.Value);
        }
    }
}