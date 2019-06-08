using FluentMigrator;

namespace Crm.Apps.Identities.Migrations
{
    [Migration(20190525175236)]
    public class Migration20190525175236AddTableIdentities : Migration
    {
        public override void Up()
        {
            Create.Table("Identities")
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey("PK_Identities_Id")
                .WithColumn("UserId").AsGuid().NotNullable()
                .WithColumn("Type").AsByte().NotNullable()
                .WithColumn("Key").AsString().NotNullable()
                .WithColumn("PasswordHash").AsString().NotNullable()
                .WithColumn("IsPrimary").AsBoolean().NotNullable()
                .WithColumn("IsVerified").AsBoolean().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable();

            Create.UniqueConstraint("UQ_Identities_UserId_Type_Key").OnTable("Identities")
                .Columns("UserId", "Type", "Key");

            Create.Index("IX_Identities_UserId_Type_Key").OnTable("Identities")
                .OnColumn("UserId").Descending()
                .OnColumn("Type").Ascending()
                .OnColumn("Key").Ascending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_Identities_UserId_Type_Key").OnTable("Identities");
            Delete.UniqueConstraint("UQ_Identities_UserId_Type_Key").FromTable("Identities");
            Delete.Table("Identities");
        }
    }
}