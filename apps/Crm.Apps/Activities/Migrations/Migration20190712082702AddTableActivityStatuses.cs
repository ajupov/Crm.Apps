using FluentMigrator;

namespace Crm.Apps.Activities.Migrations
{
    [Migration(20190712082702)]
    public class Migration20190712082702AddTableActivityStatuses : Migration
    {
        public override void Up()
        {
            Create.Table("ActivityStatuses")
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey("PK_ActivityStatuses_Id")
                .WithColumn("AccountId").AsGuid().NotNullable()
                .WithColumn("Name").AsString(64).NotNullable()
                .WithColumn("IsFinish").AsBoolean().NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable();

//            Create.PrimaryKey("PK_AccountSettings_AccountId_Type").OnTable("AccountSettings")
//                .Columns("AccountId", "Type");
//
//            
            Create.UniqueConstraint("UQ_ActivityStatuses_AccountId_Name").OnTable("ActivityStatuses")
                .Columns("AccountId", "Name");

            Create.Index("IX_ActivityStatuses_AccountId_Name_IsFinish_IsDeleted_CreateDateTime")
                .OnTable("ActivityStatuses")
                .OnColumn("AccountId").Descending()
                .OnColumn("Name").Ascending()
                .OnColumn("IsFinish").Ascending()
                .OnColumn("IsDeleted").Ascending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_ActivityStatuses_AccountId_Name_IsFinish_IsDeleted_CreateDateTime")
                .OnTable("ActivityStatuses");
            Delete.UniqueConstraint("UQ_ActivityStatuses_AccountId_Name").FromTable("ActivityStatuses");
            Delete.Table("ActivityStatuses");
        }
    }
}