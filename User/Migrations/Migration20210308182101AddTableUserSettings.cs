using FluentMigrator;

namespace Crm.Apps.User.Migrations
{
    [Migration(20210308182101)]
    public class Migration20210308182101AddTableUserSettings : Migration
    {
        public override void Up()
        {
            Create.Table("UserSettings")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("UserId").AsGuid().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("ModifyDateTime").AsDateTime2().Nullable();

            Create.PrimaryKey("PK_UserSettings_Id").OnTable("UserSettings")
                .Column("Id");

            Create.UniqueConstraint("UQ_UserSettings_UserId").OnTable("UserSettings")
                .Columns("UserId");

            Create.Index("IX_UserSettings_UserId").OnTable("UserSettings")
                .OnColumn("UserId").Ascending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_UserSettings_UserId").OnTable("UserSettings");
            Delete.UniqueConstraint("UQ_UserSettings_UserId").FromTable("UserSettings");
            Delete.PrimaryKey("PK_UserSettings_Id").FromTable("UserSettings");
            Delete.Table("UserSettings");
        }
    }
}
