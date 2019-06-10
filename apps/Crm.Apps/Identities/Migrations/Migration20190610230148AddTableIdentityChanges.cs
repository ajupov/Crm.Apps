using FluentMigrator;

namespace Crm.Apps.Identities.Migrations
{
    [Migration(20190610230148)]
    public class Migration20190610230148AddTableIdentityChanges : Migration
    {
        public override void Up()
        {
            Create.Table("IdentityChanges")
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey("PK_IdentityChanges_Id")
                .WithColumn("ChangerUserId").AsGuid().NotNullable()
                .WithColumn("IdentityId").AsGuid().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("OldValueJson").AsString().NotNullable()
                .WithColumn("NewValueJson").AsString().NotNullable();

            Create.Index("IX_IdentityChanges_ChangerUserId_IdentityId").OnTable("IdentityChanges")
                .OnColumn("ChangerUserId").Descending()
                .OnColumn("IdentityId").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_IdentityChanges_ChangerUserId_IdentityId").OnTable("IdentityChanges");
            Delete.Table("IdentityChanges");
        }
    }
}