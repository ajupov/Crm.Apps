using System;

namespace Identity.Clients.Models
{
    public class Client
    {
        public Client(
            string clientId,
            string clientSecret,
            string redirectUriPattern,
            bool isLocked,
            bool isDeleted,
            ClientScope[] scopes = null)
        {
            Id = Guid.NewGuid();
            ClientId = clientId;
            ClientSecret = clientSecret;
            RedirectUriPattern = redirectUriPattern;
            IsLocked = isLocked;
            IsDeleted = isDeleted;
            CreateDateTime = DateTime.UtcNow;
            Scopes = scopes ?? new ClientScope[0];
        }

        public Guid Id { get; }

        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public string RedirectUriPattern { get; set; }

        public bool IsLocked { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreateDateTime { get; }

        public ClientScope[] Scopes { get; set; }
    }
}