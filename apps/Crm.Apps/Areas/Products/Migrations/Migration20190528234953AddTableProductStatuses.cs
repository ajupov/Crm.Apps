using FluentMigrator;

namespace Crm.Apps.Areas.Products.Migrations
{
    [Migration(20190528234953)]
    public class Migration20190528234953AddTableProductStatuses : Migration
    {
        public override void Up()
        {
            Create.Table("ProductStatuses")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("AccountId").AsGuid().NotNullable()
                .WithColumn("Name").AsString(64).NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("ModifyDateTime").AsDateTime2().Nullable();

            Create.PrimaryKey("PK_ProductStatuses_Id").OnTable("ProductStatuses")
                .Column("Id");

            Create.UniqueConstraint("UQ_ProductStatuses_AccountId_Name").OnTable("ProductStatuses")
                .Columns("AccountId", "Name");

            Create.Index("IX_ProductStatuses_AccountId")
                .OnTable("ProductStatuses")
                .OnColumn("AccountId").Ascending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_ProductStatuses_AccountId").OnTable("ProductStatuses");
            Delete.UniqueConstraint("UQ_ProductStatuses_AccountId_Name").FromTable("ProductStatuses");
            Delete.PrimaryKey("PK_ProductStatuses_Id").FromTable("ProductStatuses");
            Delete.Table("ProductStatuses");
        }
    }
}