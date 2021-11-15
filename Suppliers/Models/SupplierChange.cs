﻿using System;

namespace Crm.Apps.Suppliers.Models
{
    public class SupplierChange
    {
        [NonDefaultGuid]
        public Guid Id { get; set; }

        public Guid ChangerUserId { get; set; }

        public Guid SupplierId { get; set; }

        public DateTime CreateDateTime { get; set; }

        public string OldValueJson { get; set; }

        public string NewValueJson { get; set; }
    }
}