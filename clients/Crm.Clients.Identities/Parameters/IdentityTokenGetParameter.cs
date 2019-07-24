using System;

namespace Crm.Clients.Identities.Parameters
{
    public class IdentityTokenGetParameter
    {
        public IdentityTokenGetParameter(
            Guid identityId, 
            string value)
        {
            IdentityId = identityId;
            Value = value;
        }

        public Guid IdentityId { get; }

        public string Value { get; }
    }
}