using FluentMigrator;

namespace Crm.Apps.Areas.Products.Migrations
{
    [Migration(20190528234953)]
    public class Migration20190528234953AddTableProductStatuses : Migration
    {
        public override void Up()
        {
            Create.Table("ProductStatuses")
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey("PK_ProductStatuses_Id")
                .WithColumn("AccountId").AsGuid().NotNullable()
                .WithColumn("Name").AsString(64).NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable();
            
            Create.UniqueConstraint("UQ_ProductStatuses_AccountId_Name").OnTable("ProductStatuses")
                .Columns("AccountId", "Name");
            
            Create.Index("IX_ProductStatuses_AccountId_Name_IsDeleted_CreateDateTime")
                .OnTable("ProductStatuses")
                .OnColumn("AccountId").Descending()
                .OnColumn("Name").Ascending()
                .OnColumn("IsDeleted").Ascending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_ProductStatuses_AccountId_Name_IsDeleted_CreateDateTime").OnTable("ProductStatuses");
            Delete.UniqueConstraint("UQ_ProductStatuses_AccountId_Name").FromTable("ProductStatuses");
            Delete.Table("ProductStatuses");
        }
    }
}