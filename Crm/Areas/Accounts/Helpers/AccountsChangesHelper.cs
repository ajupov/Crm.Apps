using System;
using System.Collections.Generic;
using Crm.Areas.Accounts.Models;
using Crm.Utils.Json;

namespace Crm.Areas.Accounts.Helpers
{
    public static class AccountsChangesHelper
    {
        public static Account CreateWithLog(this Account account, Guid userId, Action<Account> action)
        {
            action(account);

            var change = new AccountChange
            {
                AccountId = account.Id,
                ChangerUserId = userId,
                DateTime = DateTime.UtcNow,
                OldValueJson = string.Empty,
                NewValueJson = account.ToJsonString()
            };

            account.AddChange(change);

            return account;
        }

        public static void UpdateWithLog(this Account account, Guid userId, Action<Account> action)
        {
            var oldValueJson = account.ToJsonString();

            action(account);

            var change = new AccountChange
            {
                AccountId = account.Id,
                ChangerUserId = userId,
                DateTime = DateTime.UtcNow,
                OldValueJson = oldValueJson,
                NewValueJson = account.ToJsonString()
            };

            account.AddChange(change);
        }

        private static void AddChange(this Account account, AccountChange change)
        {
            if (account.Changes == null)
            {
                account.Changes = new List<AccountChange>();
            }

            account.Changes.Add(change);
        }
    }
}