using System;
using System.Collections.Generic;

namespace Crm.Apps.OAuth.Models
{
    public class OAuthClient
    {
        public OAuthClient(
            int id,
            string secret,
            string redirectUriPattern,
            bool isLocked,
            bool isDeleted,
            DateTime dateTime,
            List<OAuthClientScope> scopes)
        {
            Id = id;
            Secret = secret;
            RedirectUriPattern = redirectUriPattern;
            IsLocked = isLocked;
            IsDeleted = isDeleted;
            CreateDateTime = dateTime;
            Scopes = scopes;
        }

        public int Id { get; }

        public string Secret { get; }

        public string RedirectUriPattern { get; }

        public bool IsLocked { get; }

        public bool IsDeleted { get; }

        public DateTime CreateDateTime { get; }

        public List<OAuthClientScope> Scopes { get; }
    }
}