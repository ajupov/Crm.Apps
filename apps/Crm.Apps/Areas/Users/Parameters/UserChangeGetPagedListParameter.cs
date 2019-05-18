using System;

namespace Crm.Apps.Areas.Users.Parameters
{
    public class UserChangeGetPagedListParameter
    {
        public Guid? ChangerUserId { get; set; }

        public Guid? UserId { get; set; }
        
        public DateTime? MinCreateDate { get; set; }
        
        public DateTime? MaxCreateDate { get; set; }
        
        public int Offset { get; set; }
        
        public int Limit { get; set; } = 10;
        
        public string SortBy { get; set; }
        
        public string OrderBy { get; set; }
    }
}