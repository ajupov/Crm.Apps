using FluentMigrator;

namespace Crm.Apps.Areas.Deals.Migrations
{
    [Migration(20190702223510)]
    public class Migration20190702223510AddTableDealStatuses : Migration
    {
        public override void Up()
        {
            Create.Table("DealStatuses")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("AccountId").AsGuid().NotNullable()
                .WithColumn("Name").AsString(64).NotNullable()
                .WithColumn("IsFinish").AsBoolean().NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("ModifyDateTime").AsDateTime2().Nullable();

            Create.PrimaryKey("PK_DealStatuses_Id").OnTable("DealStatuses")
                .Column("Id");

            Create.UniqueConstraint("UQ_DealStatuses_AccountId_Name").OnTable("DealStatuses")
                .Columns("AccountId", "Name");

            Create.Index("IX_DealStatuses_AccountId").OnTable("DealStatuses")
                .OnColumn("AccountId").Ascending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_DealStatuses_AccountId").OnTable("DealStatuses");
            Delete.UniqueConstraint("UQ_DealStatuses_AccountId_Name").FromTable("DealStatuses");
            Delete.PrimaryKey("PK_DealStatuses_Id").FromTable("DealStatuses");
            Delete.Table("DealStatuses");
        }
    }
}