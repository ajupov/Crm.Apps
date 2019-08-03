using Crm.Infrastructure.Orm;
using Crm.Infrastructure.Orm.Settings;
using Identity.Clients.Models;
using Identity.OAuth.Models.OAuth;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Identity.Clients.Storages
{
    public class ClientsStorage : Storage
    {
        public ClientsStorage(IOptions<OrmSettings> options)
            : base(options)
        {
        }

        public DbSet<Client> Clients { get; set; }
    }
}