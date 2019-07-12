using FluentMigrator;

namespace Crm.Apps.Activities.Migrations
{
    [Migration(20190712211941)]
    public class Migration20190712211941AddTableActivityStatusChanges : Migration
    {
        public override void Up()
        {
            Create.Table("ActivityStatusChanges")
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey("PK_ActivityStatusChanges_Id")
                .WithColumn("ChangerUserId").AsGuid().NotNullable()
                .WithColumn("StatusId").AsGuid().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("OldValueJson").AsString().NotNullable()
                .WithColumn("NewValueJson").AsString().NotNullable();

            Create.Index("IX_ActivityStatusChanges_ChangerUserId_StatusId_CreateDateTime")
                .OnTable("ActivityStatusChanges")
                .OnColumn("ChangerUserId").Descending()
                .OnColumn("StatusId").Descending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_ActivityStatusChanges_ChangerUserId_StatusId_CreateDateTime")
                .OnTable("ActivityStatusChanges");
            Delete.Table("ActivityStatusChanges");
        }
    }
}