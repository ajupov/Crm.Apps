using System;
using System.Collections.Generic;
using Crm.Apps.Stock.Models;

namespace Crm.Apps.Stock.V1.Requests
{
    public class StockArrivalGetPagedListRequest
    {
        public bool? IsDeleted { get; set; }

        public DateTime? MinCreateDate { get; set; }

        public DateTime? MaxCreateDate { get; set; }

        public DateTime? MinModifyDate { get; set; }

        public DateTime? MaxModifyDate { get; set; }

        public List<StockArrivalType> Types { get; set; }

        public List<Guid> CreateUserIds { get; set; }

        public List<Guid> OrderIds { get; set; }

        public List<Guid> ItemsRoomIds { get; set; }

        public List<Guid> ItemsProductIds { get; set; }

        public int Offset { get; set; }

        public int Limit { get; set; } = 10;

        public string SortBy { get; set; } = "CreateDateTime";

        public string OrderBy { get; set; } = "desc";
    }
}
