//using System;
//using System.Threading.Tasks;
//using Crm.Clients.Identities.Clients;
//using Crm.Clients.Identities.Models;
//using Crm.Utils.Guid;
//using Crm.Utils.String;
//
//namespace Crm.Apps.Tests.Builders.Identities
//{
//    public class IdentityBuilder : IIdentityBuilder
//    {
//        private readonly IIdentitiesClient _identitiesClient;
//        private readonly Identity _identity;
//        private string _password;
//
//        public IdentityBuilder(IIdentitiesClient identitiesClient)
//        {
//            _identitiesClient = identitiesClient;
//            _identity = new Identity
//            {
//                Type = IdentityType.None,
//                Key = "Test",
//                IsPrimary = false,
//                IsVerified = false
//            };
//        }
//
//        public IdentityBuilder WithUserId(Guid userId)
//        {
//            _identity.UserId = userId;
//
//            return this;
//        }
//
//        public IdentityBuilder WithType(IdentityType type)
//        {
//            _identity.Type = type;
//
//            return this;
//        }
//
//        public IdentityBuilder WithKey(string key)
//        {
//            _identity.Key = key;
//
//            return this;
//        }
//
//        public IdentityBuilder WithPassword(string value)
//        {
//            _password = value;
//
//            return this;
//        }
//
//        public IdentityBuilder AsPrimary()
//        {
//            _identity.IsPrimary = true;
//
//            return this;
//        }
//
//        public IdentityBuilder AsVerified()
//        {
//            _identity.IsVerified = true;
//
//            return this;
//        }
//
//        public async Task<Identity> BuildAsync()
//        {
//            if (_identity.UserId.IsEmpty())
//            {
//                throw new InvalidOperationException(nameof(_identity.UserId));
//            }
//
//            var createdId = await _identitiesClient.CreateAsync(_identity);
//
//            if (!_password.IsEmpty())
//            {
//                await _identitiesClient.SetPasswordAsync(createdId, _password);
//            }
//
//            return await _identitiesClient.GetAsync(createdId);
//        }
//    }
//}