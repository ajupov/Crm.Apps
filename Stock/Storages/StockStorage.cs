using Ajupov.Infrastructure.All.Orm;
using Ajupov.Infrastructure.All.Orm.Settings;
using Crm.Apps.Stock.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Crm.Apps.Stock.Storages
{
    public class StockStorage : Storage
    {
        public StockStorage(IOptions<OrmSettings> options)
            : base(options)
        {
        }

        public DbSet<StockArrival> StockArrivals { get; set; }

        public DbSet<StockArrivalItem> StockArrivalItems { get; set; }

        public DbSet<StockArrivalChange> StockArrivalChanges { get; set; }

        public DbSet<StockBalance> StockBalances { get; set; }

        public DbSet<StockBalanceChange> StockBalanceChanges { get; set; }

        public DbSet<StockConsumption> StockConsumptions { get; set; }

        public DbSet<StockConsumptionItem> StockConsumptionItems { get; set; }

        public DbSet<StockConsumptionChange> StockConsumptionChanges { get; set; }

        public DbSet<StockRoom> StockRooms { get; set; }

        public DbSet<StockRoomChange> StockRoomChanges { get; set; }

        public DbSet<StockItemUniqueElement> StockItemUniqueElements { get; set; }

        public DbSet<StockItemUniqueElementChange> StockItemUniqueElementChanges { get; set; }
    }
}
