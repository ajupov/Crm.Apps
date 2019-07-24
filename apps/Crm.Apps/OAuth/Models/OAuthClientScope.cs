using System;

namespace Crm.Apps.OAuth.Models
{
    public class OAuthClientScope
    {
        public OAuthClientScope(
            Guid id,
            string value)
        {
            Id = id;
            Value = value;
        }

        public Guid Id { get; }

        public string Value { get; }
    }
}