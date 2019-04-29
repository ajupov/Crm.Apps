using FluentMigrator;

namespace Crm.Apps.Areas.Users.Migrations
{
    [Migration(20190429235943)]
    public class Migration20190429235943AddTableUserSettings : Migration
    {
        public override void Up()
        {
            Create.Table("UserSettings")
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey("PK_UserSettings_Id")
                .WithColumn("UserId").AsGuid().NotNullable().ForeignKey()
                .WithColumn("Type").AsByte().NotNullable()
                .WithColumn("Value").AsString().NotNullable();

            Create.ForeignKey("FK_UserSettings_UserId")
                .FromTable("UserSettings").ForeignColumn("UserId")
                .ToTable("Users").PrimaryColumn("Id");

            Create.UniqueConstraint("UQ_UserSettings_UserId_Type").OnTable("UserSettings")
                .Columns("UserId", "Type");

            Create.Index("IX_UserSettings_UserId_Type").OnTable("UserSettings")
                .OnColumn("UserId").Descending()
                .OnColumn("Type").Ascending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_UserSettings_UserId_Type").OnTable("UserSettings");
            Delete.UniqueConstraint("UQ_UserSettings_UserId_Type").FromTable("UserSettings");
            Delete.ForeignKey("FK_UserSettings_UserId");
            Delete.Table("UserSettings");
        }
    }
}