using FluentMigrator;

namespace Crm.Apps.Activities.Migrations
{
    [Migration(20190712211804)]
    public class Migration20190712211804AddTableActivityChanges : Migration
    {
        public override void Up()
        {
            Create.Table("ActivityChanges")
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey("PK_ActivityChanges_Id")
                .WithColumn("ChangerUserId").AsGuid().NotNullable()
                .WithColumn("ActivityId").AsGuid().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("OldValueJson").AsString().NotNullable()
                .WithColumn("NewValueJson").AsString().NotNullable();

            Create.Index("IX_ActivityChanges_ChangerUserId_ActivityId_CreateDateTime").OnTable("ActivityChanges")
                .OnColumn("ChangerUserId").Descending()
                .OnColumn("ActivityId").Descending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_ActivityChanges_ChangerUserId_ActivityId_CreateDateTime").OnTable("ActivityChanges");
            Delete.Table("ActivityChanges");
        }
    }
}