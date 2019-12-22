namespace Crm.Apps.Areas.Companies.Models
{
    public enum CompanyType : byte
    {
        None = 0,
        
        // Частные (не физические) лица
        SelfEmployed = 1,
        
        // ООО и подобные коммерческие организации
        Commercial = 2,
        
        // некоммерческие организации
        NonCommercial = 3
    }
}