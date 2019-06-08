using System;
using System.Collections.Generic;
using Crm.Apps.Users.Models;
using Crm.Common.UserContext;

namespace Crm.Apps.Users.Parameters
{
    public class UserGetPagedListParameter
    {
        public Guid? AccountId { get; set; }
        
        public string Surname { get; set; }
        
        public string Name { get; set; }
        
        public string Patronymic { get; set; }
        
        public DateTime? MinBirthDate { get; set; }
        
        public DateTime? MaxBirthDate { get; set; }
        
        public UserGender? Gender { get; set; }
        
        public bool? IsLocked { get; set; }
        
        public bool? IsDeleted { get; set; }
        
        public DateTime? MinCreateDate { get; set; }
        
        public DateTime? MaxCreateDate { get; set; }
        
        public bool? AllAttributes { get; set; }
        
        public IDictionary<Guid, string> Attributes { get; set; }
        
        public bool? AllPermissions { get; set; }
        
        public List<Permission> Permissions { get; set; }
        
        public bool? AllGroupIds { get; set; }
        
        public List<Guid> GroupIds { get; set; }
        
        public int Offset { get; set; }
        
        public int Limit { get; set; } = 10;
        
        public string SortBy { get; set; }
        
        public string OrderBy { get; set; }
    }
}