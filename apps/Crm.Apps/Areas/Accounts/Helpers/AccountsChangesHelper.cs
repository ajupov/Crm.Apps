using System;
using Ajupov.Utils.All.Json;
using Crm.Apps.Areas.Accounts.Models;

namespace Crm.Apps.Areas.Accounts.Helpers
{
    public static class AccountsChangesHelper
    {
        public static AccountChange CreateWithLog(this Account account, Guid userId, Action<Account> action)
        {
            action(account);

            return new AccountChange
            {
                AccountId = account.Id,
                ChangerUserId = userId,
                CreateDateTime = DateTime.UtcNow,
                OldValueJson = null,
                NewValueJson = account.ToJsonString()
            };
        }

        public static AccountChange UpdateWithLog(this Account account, Guid userId, Action<Account> action)
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