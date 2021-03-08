using FluentMigrator;

namespace Crm.Apps.Settings.Migrations
{
    [Migration(20210308182501)]
    public class Migration20210308182501AddTableUserSettingChanges : Migration
    {
        public override void Up()
        {
            Create.Table("UserSettingChanges")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("ChangerUserId").AsGuid().NotNullable()
                .WithColumn("UserId").AsGuid().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("OldValueJson").AsString().Nullable()
                .WithColumn("NewValueJson").AsString().Nullable();

            Create.PrimaryKey("PK_UserSettingChanges_Id").OnTable("UserSettingChanges")
                .Column("Id");

            Create.Index("IX_UserSettingChanges_UserId_CreateDateTime").OnTable("UserSettingChanges")
                .OnColumn("UserId").Ascending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_UserSettingChanges_UserId_CreateDateTime").OnTable("UserSettingChanges");
            Delete.PrimaryKey("PK_UserSettingChanges_Id").FromTable("UserSettingChanges");
            Delete.Table("UserSettingChanges");
        }
    }
}
