using Ajupov.Infrastructure.All.Orm;
using Ajupov.Infrastructure.All.Orm.Settings;
using Crm.Apps.Areas.Leads.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Crm.Apps.Areas.Leads.Storages
{
    public class LeadsStorage : Storage
    {
        public LeadsStorage(IOptions<OrmSettings> options) : base(options)
        {
        }

        public DbSet<LeadSource> LeadSources { get; set; }

        public DbSet<LeadSourceChange> LeadSourceChanges { get; set; }

        public DbSet<Lead> Leads { get; set; }

        public DbSet<LeadAttribute> LeadAttributes { get; set; }

        public DbSet<LeadAttributeChange> LeadAttributeChanges { get; set; }

        public DbSet<LeadChange> LeadChanges { get; set; }

        public DbSet<LeadComment> LeadComments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LeadAttributeLink>()
                .ToTable("LeadAttributeLinks")
                .Property(x => x.LeadAttributeId)
                .HasColumnName("AttributeId");
        }
    }
}