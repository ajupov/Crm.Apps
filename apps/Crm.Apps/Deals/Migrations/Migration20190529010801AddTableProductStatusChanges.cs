using FluentMigrator;

namespace Crm.Apps.Products.Migrations
{
    [Migration(20190529010801)]
    public class Migration20190529010801AddTableProductStatusChanges : Migration
    {
        public override void Up()
        {
            Create.Table("ProductStatusChanges")
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey("PK_ProductStatusChanges_Id")
                .WithColumn("ChangerUserId").AsGuid().NotNullable()
                .WithColumn("StatusId").AsGuid().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("OldValueJson").AsString().NotNullable()
                .WithColumn("NewValueJson").AsString().NotNullable();

            Create.Index("IX_ProductStatusChanges_ChangerUserId_StatusId_CreateDateTime")
                .OnTable("ProductStatusChanges")
                .OnColumn("ChangerUserId").Descending()
                .OnColumn("StatusId").Descending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_ProductStatusChanges_ChangerUserId_StatusId_CreateDateTime")
                .OnTable("ProductStatusChanges");
            Delete.Table("ProductStatusChanges");
        }
    }
}