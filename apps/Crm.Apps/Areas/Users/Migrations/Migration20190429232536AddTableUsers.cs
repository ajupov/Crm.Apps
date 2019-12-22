using FluentMigrator;

namespace Crm.Apps.Areas.Users.Migrations
{
    [Migration(20190429232536)]
    public class Migration20190429232536AddTableUsers : Migration
    {
        public override void Up()
        {
            Create.Table("Users")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("AccountId").AsGuid().NotNullable()
                .WithColumn("Surname").AsString(256).NotNullable()
                .WithColumn("Name").AsString(256).NotNullable()
                .WithColumn("Patronymic").AsString(256).NotNullable()
                .WithColumn("BirthDate").AsDateTime2().Nullable()
                .WithColumn("Gender").AsInt16().NotNullable()
                .WithColumn("AvatarUrl").AsString(2048).NotNullable()
                .WithColumn("IsLocked").AsBoolean().NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("ModifyDateTime").AsDateTime2().Nullable();
            
            Create.PrimaryKey("PK_Users_Id").OnTable("Users")
                .Column("Id");
            
            Create.Index("IX_Users_AccountId_IsLocked_IsDeleted")
                .OnTable("Users")
                .OnColumn("AccountId").Ascending()
                .OnColumn("IsLocked").Ascending()
                .OnColumn("IsDeleted").Ascending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_Users_AccountId_IsLocked_IsDeleted").OnTable("Users");
            Delete.PrimaryKey("PK_Users_Id").FromTable("Users");
            Delete.Table("Users");
        }
    }
}