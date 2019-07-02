using FluentMigrator;

namespace Crm.Apps.Deals.Migrations
{
    [Migration(20190702223958)]
    public class Migration20190702223958AddTableDealTypes : Migration
    {
        public override void Up()
        {
            Create.Table("DealTypes")
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey("PK_DealTypes_Id")
                .WithColumn("AccountId").AsGuid().NotNullable()
                .WithColumn("Name").AsString(64).NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable();

            Create.UniqueConstraint("UQ_DealTypes_AccountId_Name").OnTable("DealTypes")
                .Columns("AccountId", "Name");

            Create.Index("IX_DealTypes_AccountId_Name_IsDeleted_CreateDateTime")
                .OnTable("DealTypes")
                .OnColumn("AccountId").Descending()
                .OnColumn("Name").Ascending()
                .OnColumn("IsDeleted").Ascending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_DealTypes_AccountId_Name_IsDeleted_CreateDateTime").OnTable("DealTypes");
            Delete.UniqueConstraint("UQ_DealTypes_AccountId_Name").FromTable("DealTypes");
            Delete.Table("DealTypes");
        }
    }
}