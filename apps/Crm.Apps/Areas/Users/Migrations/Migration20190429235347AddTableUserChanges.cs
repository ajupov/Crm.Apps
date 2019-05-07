using FluentMigrator;

namespace Crm.Apps.Areas.Users.Migrations
{
    [Migration(20190429235347)]
    public class Migration20190429235347AddTableUserChanges : Migration
    {
        public override void Up()
        {
            Create.Table("UserChanges")
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey("PK_UserChanges_Id")
                .WithColumn("ChangerUserId").AsGuid().NotNullable()
                .WithColumn("UserId").AsGuid().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("OldValueJson").AsString().NotNullable()
                .WithColumn("NewValueJson").AsString().NotNullable();

            Create.ForeignKey("FK_UserChanges_UserId")
                .FromTable("UserChanges").ForeignColumn("UserId")
                .ToTable("Users").PrimaryColumn("Id");

            Create.Index("IX_UserChanges_ChangerUserId_UserId").OnTable("UserChanges")
                .OnColumn("ChangerUserId").Descending()
                .OnColumn("UserId").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_UserChanges_ChangerUserId_UserId").OnTable("UserChanges");
            Delete.ForeignKey("FK_UserChanges_UserId").OnTable("UserChanges");
            Delete.Table("UserChanges");
        }
    }
}