using System;
using System.Collections.Generic;
using Crm.Areas.Accounts.Models;
using Crm.Common.Types;
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
                DateTIme = DateTime.UtcNow,
                Items = new List<ChangeItem>
                    {
                        new ChangeItem
                        {
                            ChangeType = ChangeType.Creating,
                            FieldPath = null,
                            NewValue = account.ToJsonString()
                        }
                    }
            };

            account.AddChange(change);
        }

        public static void LogUpdating(
            this Account account,
            Guid changerUserId,
            string fieldPath,
            object oldValue = null,
            object newValue = null)
        {
            var change = new AccountChange
            {
                AccountId = account.Id,
                ChangerUserId = changerUserId,
                DateTIme = DateTime.UtcNow,
                Items = new List<ChangeItem>
                    {
                        new ChangeItem
                        {
                            ChangeType = ChangeType.Updating,
                            FieldPath = fieldPath.ToString(),
                            OldValue = oldValue.ToString(),
                            NewValue = newValue.ToString()
                        }
                    }
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