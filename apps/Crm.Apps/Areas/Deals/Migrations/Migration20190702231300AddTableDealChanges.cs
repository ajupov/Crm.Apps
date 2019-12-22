using FluentMigrator;

namespace Crm.Apps.Areas.Deals.Migrations
{
    [Migration(20190702231300)]
    public class Migration20190702231300AddTableDealChanges : Migration
    {
        public override void Up()
        {
            Create.Table("DealChanges")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("ChangerUserId").AsGuid().NotNullable()
                .WithColumn("DealId").AsGuid().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("OldValueJson").AsString().Nullable()
                .WithColumn("NewValueJson").AsString().Nullable();

            Create.PrimaryKey("PK_DealChanges_Id").OnTable("DealChanges")
                .Column("Id");

            Create.Index("IX_DealChanges_DealId_CreateDateTime").OnTable("DealChanges")
                .OnColumn("DealId").Ascending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_DealChanges_DealId_CreateDateTime").OnTable("DealChanges");
            Delete.PrimaryKey("PK_DealChanges_Id").FromTable("DealChanges");
            Delete.Table("DealChanges");
        }
    }
}