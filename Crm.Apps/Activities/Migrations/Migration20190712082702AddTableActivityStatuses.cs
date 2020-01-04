using FluentMigrator;

namespace Crm.Apps.Activities.Migrations
{
    [Migration(20190712082702)]
    public class Migration20190712082702AddTableActivityStatuses : Migration
    {
        public override void Up()
        {
            Create.Table("ActivityStatuses")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("AccountId").AsGuid().NotNullable()
                .WithColumn("Name").AsString(64).NotNullable()
                .WithColumn("IsFinish").AsBoolean().NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("ModifyDateTime").AsDateTime2().Nullable();

            Create.PrimaryKey("PK_ActivityStatuses_Id").OnTable("ActivityStatuses")
                .Columns("Id");

            Create.UniqueConstraint("UQ_ActivityStatuses_AccountId_Name").OnTable("ActivityStatuses")
                .Columns("AccountId", "Name");

            Create.Index("IX_ActivityStatuses_AccountId").OnTable("ActivityStatuses")
                .OnColumn("AccountId").Ascending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_ActivityStatuses_AccountId").OnTable("ActivityStatuses");
            Delete.UniqueConstraint("UQ_ActivityStatuses_AccountId_Name").FromTable("ActivityStatuses");
            Delete.PrimaryKey("PK_ActivityStatuses_Id").FromTable("ActivityStatuses");
            Delete.Table("ActivityStatuses");
        }
    }
}