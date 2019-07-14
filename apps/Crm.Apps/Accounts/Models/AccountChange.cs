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

        [Required] public Guid Id { get; set; }

        [Required] public Guid ChangerUserId { get; set; }

        [Required] public Guid AccountId { get; set; }

        public DateTime CreateDateTime { get; set; }

        public string OldValueJson { get; set; }

        public string NewValueJson { get; set; }
    }
}