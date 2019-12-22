using FluentMigrator;

namespace Crm.Apps.Areas.Deals.Migrations
{
    [Migration(20190702230819)]
    public class Migration20190702230819AddTableDealAttributes : Migration
    {
        public override void Up()
        {
            Create.Table("DealAttributes")
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey("PK_DealAttributes_Id")
                .WithColumn("AccountId").AsGuid().NotNullable()
                .WithColumn("Type").AsByte().NotNullable()
                .WithColumn("Key").AsString().NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable();

            Create.UniqueConstraint("UQ_DealAttributes_AccountId_Key").OnTable("DealAttributes")
                .Columns("AccountId", "Key");

            Create.Index("IX_DealAttributes_AccountId_Type_Key_IsDeleted_CreateDateTime")
                .OnTable("DealAttributes")
                .OnColumn("AccountId").Descending()
                .OnColumn("Type").Ascending()
                .OnColumn("Key").Ascending()
                .OnColumn("IsDeleted").Ascending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_DealAttributes_AccountId_Type_Key_IsDeleted_CreateDateTime")
                .OnTable("DealAttributes");
            Delete.UniqueConstraint("UQ_DealAttributes_AccountId_Key").FromTable("DealAttributes");
            Delete.Table("DealAttributes");
        }
    }
}