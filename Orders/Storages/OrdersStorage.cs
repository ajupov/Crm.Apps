using Ajupov.Infrastructure.All.Orm;
using Ajupov.Infrastructure.All.Orm.Settings;
using Crm.Apps.Orders.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Crm.Apps.Orders.Storages
{
    public class OrdersStorage : Storage
    {
        public OrdersStorage(IOptions<OrmSettings> options)
            : base(options)
        {
        }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderAttribute> OrderAttributes { get; set; }

        public DbSet<OrderAttributeChange> OrderAttributeChanges { get; set; }

        public DbSet<OrderChange> OrderChanges { get; set; }

        public DbSet<OrderType> OrderTypes { get; set; }

        public DbSet<OrderTypeChange> OrderTypeChanges { get; set; }

        public DbSet<OrderStatus> OrderStatuses { get; set; }

        public DbSet<OrderStatusChange> OrderStatusChanges { get; set; }

        public DbSet<OrderComment> OrderComments { get; set; }

        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderAttributeLink>()
                .ToTable("OrderAttributeLinks")
                .Property(x => x.OrderAttributeId)
                .HasColumnName("AttributeId");
        }
    }
}
