namespace Crm.Infrastructure.Orm
{
    public class OrmSettings
    {
        public string MainConnectionString { get; set; }
        
        public string ReadonlyConnectionString { get; set; }
    }
}