using FluentMigrator;

namespace Crm.Apps.Deals.Migrations
{
    [Migration(20190702223958)]
    public class Migration20190702223958AddTableDealTypes : Migration
    {
        public override void Up()
        {
            Create.Table("DealTypes")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("AccountId").AsGuid().NotNullable()
                .WithColumn("Name").AsString(64).NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("ModifyDateTime").AsDateTime2().Nullable();

            Create.PrimaryKey("PK_DealTypes_Id").OnTable("DealTypes")
                .Column("Id");

            Create.UniqueConstraint("UQ_DealTypes_AccountId_Name").OnTable("DealTypes")
                .Columns("AccountId", "Name");

            Create.Index("IX_DealTypes_AccountId").OnTable("DealTypes")
                .OnColumn("AccountId").Ascending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_DealTypes_AccountId").OnTable("DealTypes");
            Delete.UniqueConstraint("UQ_DealTypes_AccountId_Name").FromTable("DealTypes");
            Delete.PrimaryKey("PK_DealTypes_Id").FromTable("DealTypes");
            Delete.Table("DealTypes");
        }
    }
}