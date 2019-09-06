﻿using System;
using System.Collections.Generic;
using Crm.Clients.Accounts.Models;

namespace Crm.Clients.Accounts.RequestParameters
{
    public class AccountUpdateRequest
    {
        public Guid Id { get; set; }

        public AccountType Type { get; set; }

        public bool IsLocked { get; set; }

        public bool IsDeleted { get; set; }

        public List<AccountSetting>? Settings { get; set; }
    }
}