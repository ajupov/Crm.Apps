using FluentMigrator;

namespace Crm.Apps.Deals.Migrations
{
    [Migration(20190702231419)]
    public class Migration20190702231419AddTableDealStatusChanges : Migration
    {
        public override void Up()
        {
            Create.Table("DealStatusChanges")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("ChangerUserId").AsGuid().NotNullable()
                .WithColumn("StatusId").AsGuid().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("OldValueJson").AsString().Nullable()
                .WithColumn("NewValueJson").AsString().Nullable();

            Create.PrimaryKey("PK_DealStatusChanges_Id").OnTable("DealStatusChanges")
                .Column("Id");

            Create.Index("IX_DealStatusChanges_StatusId_CreateDateTime").OnTable("DealStatusChanges")
                .OnColumn("StatusId").Ascending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_DealStatusChanges_StatusId_CreateDateTime").OnTable("DealStatusChanges");
            Delete.PrimaryKey("PK_DealStatusChanges_Id").FromTable("DealStatusChanges");
            Delete.Table("DealStatusChanges");
        }
    }
}