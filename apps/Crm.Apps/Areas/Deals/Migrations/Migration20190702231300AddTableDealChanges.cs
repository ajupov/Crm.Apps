using FluentMigrator;

namespace Crm.Apps.Areas.Deals.Migrations
{
    [Migration(20190702231300)]
    public class Migration20190702231300AddTableDealChanges : Migration
    {
        public override void Up()
        {
            Create.Table("DealChanges")
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey("PK_DealChanges_Id")
                .WithColumn("ChangerUserId").AsGuid().NotNullable()
                .WithColumn("DealId").AsGuid().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("OldValueJson").AsString().NotNullable()
                .WithColumn("NewValueJson").AsString().NotNullable();

            Create.Index("IX_DealChanges_ChangerUserId_DealId_CreateDateTime").OnTable("DealChanges")
                .OnColumn("ChangerUserId").Descending()
                .OnColumn("DealId").Descending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_DealChanges_ChangerUserId_DealId_CreateDateTime").OnTable("DealChanges");
            Delete.Table("DealChanges");
        }
    }
}