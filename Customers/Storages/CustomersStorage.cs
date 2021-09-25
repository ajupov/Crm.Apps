using Ajupov.Infrastructure.All.Orm;
using Ajupov.Infrastructure.All.Orm.Settings;
using Crm.Apps.Customers.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Crm.Apps.Customers.Storages
{
    public class CustomersStorage : Storage
    {
        public CustomersStorage(IOptions<OrmSettings> options)
            : base(options)
        {
        }

        public DbSet<CustomerSource> CustomerSources { get; set; }

        public DbSet<CustomerSourceChange> CustomerSourceChanges { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<CustomerAttribute> CustomerAttributes { get; set; }

        public DbSet<CustomerAttributeChange> CustomerAttributeChanges { get; set; }

        public DbSet<CustomerChange> CustomerChanges { get; set; }

        public DbSet<CustomerComment> CustomerComments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CustomerAttributeLink>()
                .ToTable("CustomerAttributeLinks")
                .Property(x => x.CustomerAttributeId)
                .HasColumnName("AttributeId");
        }
    }
}
