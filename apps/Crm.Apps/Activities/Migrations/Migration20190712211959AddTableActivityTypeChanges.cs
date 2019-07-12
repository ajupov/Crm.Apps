using FluentMigrator;

namespace Crm.Apps.Activities.Migrations
{
    [Migration(20190712211959)]
    public class Migration20190712211959AddTableActivityTypeChanges : Migration
    {
        public override void Up()
        {
            Create.Table("ActivityTypeChanges")
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey("PK_ActivityTypeChanges_Id")
                .WithColumn("ChangerUserId").AsGuid().NotNullable()
                .WithColumn("TypeId").AsGuid().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("OldValueJson").AsString().NotNullable()
                .WithColumn("NewValueJson").AsString().NotNullable();

            Create.Index("IX_ActivityTypeChanges_ChangerUserId_TypeId_CreateDateTime")
                .OnTable("ActivityTypeChanges")
                .OnColumn("ChangerUserId").Descending()
                .OnColumn("TypeId").Descending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_ActivityTypeChanges_ChangerUserId_TypeId_CreateDateTime")
                .OnTable("ActivityTypeChanges");
            Delete.Table("ActivityTypeChanges");
        }
    }
}