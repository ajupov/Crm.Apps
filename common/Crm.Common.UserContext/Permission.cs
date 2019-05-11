namespace Crm.Common.UserContext
{
    public enum Permission : byte
    {
        None = 0,
        System = 1,
        Development = 2,
        Administration = 3,
        TechnicalSupport = 4,
        AccountOwning = 5,
        ProductsManagement = 6,
        SalesManagement = 7,
        LeadsProviding = 8,
        DemoViewing = 9
    }
}