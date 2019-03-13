using System;
using System.Collections.Generic;
using Crm.Areas.Accounts.Models;
using Crm.Utils.Json;

namespace Crm.Areas.Accounts.Helpers
{
    public static class AccountsChangesHelper
    {
        public static void LogCreating(
            this Account account,
            Guid changerUserId)
        {
            var change = new AccountChange
            {
                AccountId = account.Id,
                ChangerUserId = changerUserId,
                DateTime = DateTime.UtcNow,
                OldValueJson = string.Empty,
                NewValueJson = account.ToJsonString()
            };

            account.AddChange(change);
        }

        public static void LogUpdating(
            this Account account,
            Guid changerUserId,
            object oldValue = null,
            object newValue = null)
        {
            var change = new AccountChange
            {
                AccountId = account.Id,
                ChangerUserId = changerUserId,
                DateTime = DateTime.UtcNow,
                OldValueJson = oldValue.ToString(),
                NewValueJson = newValue.ToJsonString()
            };

            account.AddChange(change);
        }

        private static void AddChange(
            this Account account,
            AccountChange change)
        {
            if (account.Changes == null)
            {
                account.Changes = new List<AccountChange>();
            }

            account.Changes.Add(change);
        }
    }
}