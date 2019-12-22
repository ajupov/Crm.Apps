using FluentMigrator;

namespace Crm.Apps.Areas.Deals.Migrations
{
    [Migration(20190702230819)]
    public class Migration20190702230819AddTableDealAttributes : Migration
    {
        public override void Up()
        {
            Create.Table("DealAttributes")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("AccountId").AsGuid().NotNullable()
                .WithColumn("Type").AsByte().NotNullable()
                .WithColumn("Key").AsString().NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("ModifyDateTime").AsDateTime2().Nullable();

            Create.PrimaryKey("PK_DealAttributes_Id").OnTable("DealAttributes")
                .Column("Id");

            Create.UniqueConstraint("UQ_DealAttributes_AccountId_Key").OnTable("DealAttributes")
                .Columns("AccountId", "Key");

            Create.Index("IX_DealAttributes_AccountId").OnTable("DealAttributes")
                .OnColumn("AccountId").Ascending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_DealAttributes_AccountId").OnTable("DealAttributes");
            Delete.UniqueConstraint("UQ_DealAttributes_AccountId_Key").FromTable("DealAttributes");
            Delete.PrimaryKey("PK_DealAttributes_Id").FromTable("DealAttributes");
            Delete.Table("DealAttributes");
        }
    }
}