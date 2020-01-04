using FluentMigrator;

namespace Crm.Apps.Users.Migrations
{
    [Migration(20190429235347)]
    public class Migration20190429235347AddTableUserChanges : Migration
    {
        public override void Up()
        {
            Create.Table("UserChanges")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("ChangerUserId").AsGuid().NotNullable()
                .WithColumn("UserId").AsGuid().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("OldValueJson").AsString().Nullable()
                .WithColumn("NewValueJson").AsString().Nullable();

            Create.PrimaryKey("PK_UserChanges_Id").OnTable("UserChanges")
                .Column("Id");

            Create.Index("IX_UserChanges_UserId_CreateDateTime").OnTable("UserChanges")
                .OnColumn("UserId").Ascending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_UserChanges_UserId_CreateDateTime").OnTable("UserChanges");
            Delete.PrimaryKey("PK_UserChanges_Id").FromTable("UserChanges");
            Delete.Table("UserChanges");
        }
    }
}