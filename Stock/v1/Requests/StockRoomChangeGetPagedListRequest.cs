using System;
using System.ComponentModel.DataAnnotations;

namespace Crm.Apps.Stock.V1.Requests
{
    public class StockRoomChangeGetPagedListRequest
    {
        [Required]
        public Guid StockRoomId { get; set; }

        public DateTime? MinCreateDate { get; set; }

        public DateTime? MaxCreateDate { get; set; }

        public int Offset { get; set; }

        public int Limit { get; set; } = 10;

        public string SortBy { get; set; } = "CreateDateTime";

        public string OrderBy { get; set; } = "desc";
    }
}
