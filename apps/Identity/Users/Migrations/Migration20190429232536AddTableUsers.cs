using FluentMigrator;

namespace Identity.Users.Migrations
{
    [Migration(20190429232536)]
    public class Migration20190429232536AddTableUsers : Migration
    {
        public override void Up()
        {
            Create.Table("Users")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("Surname").AsString(256).NotNullable()
                .WithColumn("Name").AsString(256).NotNullable()
                .WithColumn("Patronymic").AsString(256).NotNullable()
                .WithColumn("BirthDate").AsDateTime2().Nullable()
                .WithColumn("Gender").AsByte().Nullable()
                .WithColumn("AvatarUrl").AsString(2048).NotNullable()
                .WithColumn("IsLocked").AsBoolean().NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable();

            Create.PrimaryKey("PK_Users_Id").OnTable("Users")
                .Column("Id");

            Create.Index("IX_Users_IsLocked_IsDeleted_CreateDateTime").OnTable("Users")
                .OnColumn("IsLocked").Ascending()
                .OnColumn("IsDeleted").Ascending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_Users_IsLocked_IsDeleted_CreateDateTime").OnTable("Users");
            Delete.PrimaryKey("PK_Users_Id").FromTable("Users");
            Delete.Table("Users");
        }
    }
}