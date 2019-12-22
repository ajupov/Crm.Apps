using FluentMigrator;

namespace Crm.Apps.Areas.Activities.Migrations
{
    [Migration(20190712211804)]
    public class Migration20190712211804AddTableActivityChanges : Migration
    {
        public override void Up()
        {
            Create.Table("ActivityChanges")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("ChangerUserId").AsGuid().NotNullable()
                .WithColumn("ActivityId").AsGuid().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("OldValueJson").AsString().Nullable()
                .WithColumn("NewValueJson").AsString().Nullable();

            Create.PrimaryKey("PK_ActivityChanges_Id").OnTable("ActivityChanges")
                .Columns("Id");

            Create.Index("IX_ActivityChanges_ActivityId_CreateDateTime").OnTable("ActivityChanges")
                .OnColumn("ActivityId").Ascending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_ActivityChanges_ActivityId_CreateDateTime").OnTable("ActivityChanges");
            Delete.PrimaryKey("PK_ActivityChanges_Id").FromTable("ActivityChanges");
            Delete.Table("ActivityChanges");
        }
    }
}