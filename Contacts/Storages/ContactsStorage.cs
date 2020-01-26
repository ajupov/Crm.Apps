using Ajupov.Infrastructure.All.Orm;
using Ajupov.Infrastructure.All.Orm.Settings;
using Crm.Apps.Contacts.v1.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Crm.Apps.Contacts.Storages
{
    public class ContactsStorage : Storage
    {
        public ContactsStorage(IOptions<OrmSettings> options) : base(options)
        {
        }

        public DbSet<Contact> Contacts { get; set; }

        public DbSet<ContactBankAccount> ContactBankAccounts { get; set; }

        public DbSet<ContactAttribute> ContactAttributes { get; set; }

        public DbSet<ContactAttributeChange> ContactAttributeChanges { get; set; }

        public DbSet<ContactChange> ContactChanges { get; set; }

        public DbSet<ContactComment> ContactComments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ContactAttributeLink>()
                .ToTable("ContactAttributeLinks")
                .Property(x => x.ContactAttributeId)
                .HasColumnName("AttributeId");
        }
    }
}