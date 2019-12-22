using FluentMigrator;

namespace Crm.Apps.Areas.Products.Migrations
{
    [Migration(20190529010801)]
    public class Migration20190529010801AddTableProductStatusChanges : Migration
    {
        public override void Up()
        {
            Create.Table("ProductStatusChanges")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("ChangerUserId").AsGuid().NotNullable()
                .WithColumn("StatusId").AsGuid().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("OldValueJson").AsString().Nullable()
                .WithColumn("NewValueJson").AsString().Nullable();

            Create.PrimaryKey("PK_ProductStatusChanges_Id").OnTable("ProductStatusChanges")
                .Column("Id");

            Create.Index("IX_ProductStatusChanges_StatusId_CreateDateTime")
                .OnTable("ProductStatusChanges")
                .OnColumn("StatusId").Ascending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_ProductStatusChanges_StatusId_CreateDateTime").OnTable("ProductStatusChanges");
            Delete.PrimaryKey("PK_ProductStatusChanges_Id").FromTable("ProductStatusChanges");
            Delete.Table("ProductStatusChanges");
        }
    }
}