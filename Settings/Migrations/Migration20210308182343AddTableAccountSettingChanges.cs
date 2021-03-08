using FluentMigrator;

namespace Crm.Apps.Settings.Migrations
{
    [Migration(20210308182343)]
    public class Migration20210308182343AddTableAccountSettingChanges : Migration
    {
        public override void Up()
        {
            Create.Table("AccountSettingChanges")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("ChangerUserId").AsGuid().NotNullable()
                .WithColumn("AccountId").AsGuid().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("OldValueJson").AsString().Nullable()
                .WithColumn("NewValueJson").AsString().Nullable();

            Create.PrimaryKey("PK_AccountSettingChanges_Id").OnTable("AccountSettingChanges")
                .Column("Id");

            Create.Index("IX_AccountSettingChanges_AccountId_CreateDateTime").OnTable("AccountSettingChanges")
                .OnColumn("AccountId").Ascending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_AccountSettingChanges_AccountId_CreateDateTime").OnTable("AccountSettingChanges");
            Delete.PrimaryKey("PK_AccountSettingChanges_Id").FromTable("AccountSettingChanges");
            Delete.Table("AccountSettingChanges");
        }
    }
}
