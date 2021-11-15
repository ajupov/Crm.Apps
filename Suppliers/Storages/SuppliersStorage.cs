using Ajupov.Infrastructure.All.Orm;
using Ajupov.Infrastructure.All.Orm.Settings;
using Crm.Apps.Suppliers.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Crm.Apps.Suppliers.Storages
{
    public class SuppliersStorage : Storage
    {
        public SuppliersStorage(IOptions<OrmSettings> options)
            : base(options)
        {
        }

        public DbSet<Supplier> Suppliers { get; set; }

        public DbSet<SupplierAttribute> SupplierAttributes { get; set; }

        public DbSet<SupplierAttributeChange> SupplierAttributeChanges { get; set; }

        public DbSet<SupplierChange> SupplierChanges { get; set; }

        public DbSet<SupplierComment> SupplierComments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SupplierAttributeLink>()
                .ToTable("SupplierAttributeLinks")
                .Property(x => x.SupplierAttributeId)
                .HasColumnName("AttributeId");
        }
    }
}
