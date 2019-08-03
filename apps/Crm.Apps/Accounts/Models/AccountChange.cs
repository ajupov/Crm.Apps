using System;
using System.ComponentModel.DataAnnotations;

namespace Crm.Apps.Accounts.Models
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

        [Required]
        public Guid Id { get; }

        [Required]
        public Guid ChangerUserId { get; }

        [Required]
        public Guid AccountId { get; }

        public DateTime CreateDateTime { get; }

        public string OldValueJson { get; }

        public string NewValueJson { get; }
    }
}