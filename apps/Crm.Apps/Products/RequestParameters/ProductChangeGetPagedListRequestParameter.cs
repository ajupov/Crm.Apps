using System;
using System.ComponentModel.DataAnnotations;

namespace Crm.Apps.Products.RequestParameters
{
    public class ProductChangeGetPagedListRequestParameter
    {
        [Required]
        public Guid ProductId { get; set; }
        
        public Guid? ChangerUserId { get; set; }
        
        public DateTime? MinCreateDate { get; set; }
        
        public DateTime? MaxCreateDate { get; set; }
        
        public int Offset { get; set; }
        
        public int Limit { get; set; } = 10;
        
        public string SortBy { get; set; }
        
        public string OrderBy { get; set; }
    }
}