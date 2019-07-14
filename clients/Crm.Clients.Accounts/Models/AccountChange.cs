using System;
using System.ComponentModel.DataAnnotations;

namespace Crm.Clients.Accounts.Models
{
    public class AccountChange
    {
        public AccountChange(
            Guid accountId,
            Guid changerUserId,
            string oldValueJson = default,
            string newValueJson = default)
        {
            Id = Guid.NewGuid();
            AccountId = accountId;
            ChangerUserId = changerUserId;
            CreateDateTime = DateTime.UtcNow;
            OldValueJson = oldValueJson;
            NewValueJson = newValueJson;
        }

        public Guid Id { get; set; }

        public Guid ChangerUserId { get; set; }

        public Guid AccountId { get; set; }

        public DateTime CreateDateTime { get; set; }

        public string OldValueJson { get; set; }

        public string NewValueJson { get; set; }
    }
}