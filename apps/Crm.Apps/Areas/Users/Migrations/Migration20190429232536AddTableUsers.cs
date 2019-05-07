using FluentMigrator;

namespace Crm.Apps.Areas.Users.Migrations
{
    [Migration(20190429232536)]
    public class Migration20190429232536AddTableUsers : Migration
    {
        public override void Up()
        {
            Create.Table("Users")
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey("PK_Users_Id")
                .WithColumn("AccountId").AsGuid().NotNullable()
                .WithColumn("Surname").AsString(256).NotNullable()
                .WithColumn("Name").AsString(256).NotNullable()
                .WithColumn("Patronymic").AsString(256).NotNullable()
                .WithColumn("BirthDate").AsDateTime2().Nullable()
                .WithColumn("Gender").AsInt16().NotNullable()
                .WithColumn("AvatarUrl").AsString(2048).NotNullable()
                .WithColumn("IsLocked").AsBoolean().NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable();

            Create.Index("IX_Users_AccountId_Surname_Name_Patronymic_Gender_CreateDateTime_IsLocked_IsDeleted")
                .OnTable("Users")
                .OnColumn("AccountId").Descending()
                .OnColumn("Surname").Ascending()
                .OnColumn("Name").Ascending()
                .OnColumn("Patronymic").Ascending()
                .OnColumn("Gender").Descending()
                .OnColumn("IsLocked").Ascending()
                .OnColumn("IsDeleted").Ascending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_Users_AccountId_Surname_Name_Patronymic_Gender_CreateDateTime_IsLocked_IsDeleted")
                .OnTable("Users");
            Delete.Table("Users");
        }
    }
}