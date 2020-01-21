using Ajupov.Infrastructure.All.Orm;
using Ajupov.Infrastructure.All.Orm.Settings;
using Crm.Apps.Companies.v1.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Crm.Apps.Companies.Storages
{
    public class CompaniesStorage : Storage
    {
        public CompaniesStorage(IOptions<OrmSettings> options) : base(options)
        {
        }

        public DbSet<Company> Companies { get; set; }

        public DbSet<CompanyBankAccount> CompanyBankAccounts { get; set; }

        public DbSet<CompanyAttribute> CompanyAttributes { get; set; }

        public DbSet<CompanyAttributeChange> CompanyAttributeChanges { get; set; }

        public DbSet<CompanyChange> CompanyChanges { get; set; }

        public DbSet<CompanyComment> CompanyComments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CompanyAttributeLink>()
                .ToTable("CompanyAttributeLinks")
                .Property(x => x.CompanyAttributeId)
                .HasColumnName("AttributeId");
        }
    }
}