using System;
using Crm.Apps.Accounts.Models;
using Crm.Utils.Json;

namespace Crm.Apps.Accounts.Helpers
{
    public static class AccountsChangesHelper
    {
        public static AccountChange WithCreateLog(this Account account, Guid userId, Action<Account> action)
        {
            action(account);

            return new AccountChange
            {
                AccountId = account.Id,
                ChangerUserId = userId,
                CreateDateTime = DateTime.UtcNow,
                OldValueJson = string.Empty,
                NewValueJson = account.ToJsonString()
            };
        }

        public static AccountChange WithUpdateLog(this Account account, Guid userId, Action<Account> action)
        {
            var oldValueJson = account.ToJsonString();

            action(account);

            return new AccountChange
            {
                AccountId = account.Id,
                ChangerUserId = userId,
                CreateDateTime = DateTime.UtcNow,
                OldValueJson = oldValueJson,
                NewValueJson = account.ToJsonString()
            };
        }
    }
}