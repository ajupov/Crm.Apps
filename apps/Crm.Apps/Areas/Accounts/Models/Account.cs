﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Crm.Apps.Areas.Accounts.Models
{
    public class Account
    {
        public Guid Id { get; set; }

        public bool IsLocked { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreateDateTime { get; set; }

        public List<AccountSetting> Settings { get; set; }

        public List<AccountChange> Changes { get; set; }
    }
}