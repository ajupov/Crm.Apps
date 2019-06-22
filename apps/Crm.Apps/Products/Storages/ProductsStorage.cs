using Crm.Apps.Products.Models;
using Crm.Infrastructure.Orm.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Crm.Apps.Products.Storages
{
    public class ProductsStorage : DbContext
    {
        private readonly OrmSettings _config;

        public DbSet<Product> Products { get; set; }

        public DbSet<ProductAttribute> ProductAttributes { get; set; }

        public DbSet<ProductAttributeChange> ProductAttributeChanges { get; set; }

        public DbSet<ProductCategory> ProductCategories { get; set; }

        public DbSet<ProductCategoryChange> ProductCategoryChanges { get; set; }

        public DbSet<ProductChange> ProductChanges { get; set; }

        public DbSet<ProductStatus> ProductStatuses { get; set; }

        public DbSet<ProductStatusChange> ProductStatusChanges { get; set; }

        public ProductsStorage(IOptions<OrmSettings> options)
        {
            _config = options.Value;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseNpgsql(_config.MainConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductAttributeLink>()
                .ToTable("ProductAttributeLinks")
                .Property(x => x.ProductAttributeId)
                .HasColumnName("AttributeId");

            modelBuilder.Entity<ProductCategoryLink>()
                .ToTable("ProductCategoryLinks")
                .Property(x => x.ProductCategoryId)
                .HasColumnName("CategoryId");
        }
    }
}