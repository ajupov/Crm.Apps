using FluentMigrator;

namespace Crm.Apps.Areas.Deals.Migrations
{
    [Migration(20190702231419)]
    public class Migration20190702231419AddTableDealStatusChanges : Migration
    {
        public override void Up()
        {
            Create.Table("DealStatusChanges")
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey("PK_DealStatusChanges_Id")
                .WithColumn("ChangerUserId").AsGuid().NotNullable()
                .WithColumn("StatusId").AsGuid().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("OldValueJson").AsString().NotNullable()
                .WithColumn("NewValueJson").AsString().NotNullable();

            Create.Index("IX_DealStatusChanges_ChangerUserId_StatusId_CreateDateTime")
                .OnTable("DealStatusChanges")
                .OnColumn("ChangerUserId").Descending()
                .OnColumn("StatusId").Descending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_DealStatusChanges_ChangerUserId_StatusId_CreateDateTime")
                .OnTable("DealStatusChanges");
            Delete.Table("DealStatusChanges");
        }
    }
}