using System;
using System.ComponentModel.DataAnnotations;

namespace Crm.Apps.Deals.v1.RequestParameters
{
    public class DealAttributeChangeGetPagedListRequestParameter
    {
        [Required]
        public Guid AttributeId { get; set; }
        
        public Guid? ChangerUserId { get; set; }

        public DateTime? MinCreateDate { get; set; }

        public DateTime? MaxCreateDate { get; set; }

        public int Offset { get; set; }

        public int Limit { get; set; } = 10;

        public string SortBy { get; set; }

        public string OrderBy { get; set; }
    }
}