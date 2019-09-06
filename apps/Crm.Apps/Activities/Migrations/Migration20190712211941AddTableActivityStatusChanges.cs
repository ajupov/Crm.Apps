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
                .WithColumn("OldValueJson").AsString().Nullable()
                .WithColumn("NewValueJson").AsString().Nullable();

            Create.PrimaryKey("PK_ActivityStatusChanges_Id").OnTable("ActivityStatusChanges")
                .Columns("Id");

            Create.Index("IX_ActivityStatusChanges_ChangerUserId_StatusId_CreateDateTime")
                .OnTable("ActivityStatusChanges")
                .OnColumn("ChangerUserId").Ascending()
                .OnColumn("StatusId").Ascending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_ActivityStatusChanges_ChangerUserId_StatusId_CreateDateTime")
                .OnTable("ActivityStatusChanges");
            Delete.PrimaryKey("PK_ActivityStatusChanges_Id").FromTable("ActivityStatusChanges");
            Delete.Table("ActivityStatusChanges");
        }
    }
}