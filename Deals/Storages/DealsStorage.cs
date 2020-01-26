using Ajupov.Infrastructure.All.Orm;
using Ajupov.Infrastructure.All.Orm.Settings;
using Crm.Apps.Deals.v1.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Crm.Apps.Deals.Storages
{
    public class DealsStorage : Storage
    {
        public DealsStorage(IOptions<OrmSettings> options) : base(options)
        {
        }

        public DbSet<Deal> Deals { get; set; }

        public DbSet<DealAttribute> DealAttributes { get; set; }

        public DbSet<DealAttributeChange> DealAttributeChanges { get; set; }

        public DbSet<DealChange> DealChanges { get; set; }

        public DbSet<DealType> DealTypes { get; set; }

        public DbSet<DealTypeChange> DealTypeChanges { get; set; }

        public DbSet<DealStatus> DealStatuses { get; set; }

        public DbSet<DealStatusChange> DealStatusChanges { get; set; }

        public DbSet<DealComment> DealComments { get; set; }

        public DbSet<DealPosition> DealPositions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DealAttributeLink>()
                .ToTable("DealAttributeLinks")
                .Property(x => x.DealAttributeId)
                .HasColumnName("AttributeId");
        }
    }
}