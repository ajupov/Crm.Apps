using FluentMigrator;

namespace Crm.Apps.Activities.Migrations
{
    [Migration(20190712211959)]
    public class Migration20190712211959AddTableActivityTypeChanges : Migration
    {
        public override void Up()
        {
            Create.Table("ActivityTypeChanges")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("ChangerUserId").AsGuid().NotNullable()
                .WithColumn("TypeId").AsGuid().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("OldValueJson").AsString().Nullable()
                .WithColumn("NewValueJson").AsString().Nullable();

            Create.PrimaryKey("PK_ActivityTypeChanges_Id").OnTable("ActivityTypeChanges")
                .Columns("Id");

            Create.Index("IX_ActivityTypeChanges_TypeId_CreateDateTime").OnTable("ActivityTypeChanges")
                .OnColumn("TypeId").Ascending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_ActivityTypeChanges_TypeId_CreateDateTime").OnTable("ActivityTypeChanges");
            Delete.PrimaryKey("PK_ActivityTypeChanges_Id").FromTable("ActivityTypeChanges");
            Delete.Table("ActivityTypeChanges");
        }
    }
}