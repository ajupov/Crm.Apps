using FluentMigrator;

namespace Crm.Apps.Areas.Users.Migrations
{
    [Migration(20190507225048)]
    public class Migration20190507225048AddTableUserGroups : Migration
    {
        public override void Up()
        {
            Create.Table("UserGroups")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("AccountId").AsGuid().NotNullable()
                .WithColumn("Name").AsString().NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("ModifyDateTime").AsDateTime2().Nullable();

            Create.PrimaryKey("PK_UserGroups_Id").OnTable("UserGroups")
                .Column("Id");

            Create.UniqueConstraint("UQ_UserGroups_AccountId_Name").OnTable("UserGroups")
                .Columns("AccountId", "Name");

            Create.Index("IX_UserGroups_AccountId").OnTable("UserGroups")
                .OnColumn("AccountId").Ascending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_UserGroups_AccountId").OnTable("UserGroups");
            Delete.UniqueConstraint("UQ_UserGroups_AccountId_Name").FromTable("UserGroups");
            Delete.PrimaryKey("PK_UserGroups_Id").FromTable("UserGroups");
            Delete.Table("UserGroups");
        }
    }
}