using FluentMigrator;

namespace Crm.Apps.Areas.Deals.Migrations
{
    [Migration(20190702232323)]
    public class Migration20190702232323AddTableDealPositions : Migration
    {
        public override void Up()
        {
            Create.Table("DealPositions")
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey("PK_DealPositions_Id")
                .WithColumn("DealId").AsGuid().NotNullable()
                .WithColumn("ProductId").AsGuid().NotNullable()
                .WithColumn("Price").AsDecimal().NotNullable()
                .WithColumn("Count").AsInt32().NotNullable();

            Create.ForeignKey("FK_DealPositions_DealId")
                .FromTable("DealPositions").ForeignColumn("DealId")
                .ToTable("Deals").PrimaryColumn("Id");

            Create.UniqueConstraint("UQ_DealPositions_DealId_ProductId").OnTable("DealPositions")
                .Columns("DealId", "ProductId");

            Create.Index("IX_DealPositions_DealId_ProductId_Price_Count_CreateDateTime").OnTable("DealPositions")
                .OnColumn("DealId").Descending()
                .OnColumn("ProductId").Descending()
                .OnColumn("Price").Ascending()
                .OnColumn("Count").Ascending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_DealPositions_DealId_ProductId_Price_Count_CreateDateTime").OnTable("DealPositions");
            Delete.UniqueConstraint("UQ_DealPositions_DealId_ProductId").FromTable("DealPositions");
            Delete.ForeignKey("FK_DealPositions_DealId").OnTable("DealPositions");
            Delete.Table("DealPositions");
        }
    }
}