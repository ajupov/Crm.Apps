using FluentMigrator;

namespace Crm.Apps.Deals.Migrations
{
    [Migration(20190702232323)]
    public class Migration20190702232323AddTableDealPositions : Migration
    {
        public override void Up()
        {
            Create.Table("DealPositions")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("DealId").AsGuid().NotNullable()
                .WithColumn("ProductId").AsGuid().NotNullable()
                .WithColumn("ProductName").AsString(64).NotNullable()
                .WithColumn("ProductVendorCode").AsString(16).Nullable()
                .WithColumn("Price").AsDecimal().NotNullable()
                .WithColumn("Count").AsDecimal().NotNullable();

            Create.PrimaryKey("PK_DealPositions_Id").OnTable("DealPositions")
                .Column("Id");

            Create.ForeignKey("FK_DealPositions_DealId")
                .FromTable("DealPositions").ForeignColumn("DealId")
                .ToTable("Deals").PrimaryColumn("Id");

            Create.UniqueConstraint("UQ_DealPositions_DealId_ProductId").OnTable("DealPositions")
                .Columns("DealId", "ProductId");

            Create.Index("IX_DealPositions_DealId").OnTable("DealPositions")
                .OnColumn("DealId").Ascending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_DealPositions_DealId").OnTable("DealPositions");
            Delete.UniqueConstraint("UQ_DealPositions_DealId_ProductId").FromTable("DealPositions");
            Delete.ForeignKey("FK_DealPositions_DealId").OnTable("DealPositions");
            Delete.PrimaryKey("PK_DealPositions_Id").FromTable("DealPositions");
            Delete.Table("DealPositions");
        }
    }
}