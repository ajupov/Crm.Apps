using Crm.Apps.Leads.Models;
using Crm.Infrastructure.Orm.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Crm.Apps.Leads.Storages
{
    public class LeadsStorage : DbContext
    {
        private readonly OrmSettings _config;

        public DbSet<LeadSource> LeadSources { get; set; }

        public DbSet<LeadSourceChange> LeadSourceChanges { get; set; }

        public DbSet<Lead> Leads { get; set; }

        public DbSet<LeadAttribute> LeadAttributes { get; set; }

        public DbSet<LeadAttributeChange> LeadAttributeChanges { get; set; }

        public DbSet<LeadChange> LeadChanges { get; set; }

        public DbSet<LeadComment> LeadComments { get; set; }

        public LeadsStorage(IOptions<OrmSettings> options)
        {
            _config = options.Value;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseNpgsql(_config.MainConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LeadAttributeLink>()
                .ToTable("LeadAttributeLinks")
                .Property(b => b.LeadAttributeId)
                .HasColumnName("AttributeId");
        }
    }
}