using FluentMigrator;

namespace Identity.Identities.Migrations
{
    [Migration(20190525175236)]
    public class Migration20190525175236AddTableIdentities : Migration
    {
        public override void Up()
        {
            Create.Table("Identities")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("UserId").AsGuid().NotNullable()
                .WithColumn("Type").AsByte().NotNullable()
                .WithColumn("Key").AsString().NotNullable()
                .WithColumn("PasswordHash").AsString().Nullable()
                .WithColumn("IsPrimary").AsBoolean().NotNullable()
                .WithColumn("IsVerified").AsBoolean().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable();

            Create.PrimaryKey("PK_Identities_Id").OnTable("Identities")
                .Column("Id");

            Create.UniqueConstraint("UQ_Identities_UserId_Key").OnTable("Identities")
                .Columns("UserId", "Key");

            Create.Index("IX_Identities_UserId_Type_Key").OnTable("Identities")
                .OnColumn("UserId").Descending()
                .OnColumn("Type").Ascending()
                .OnColumn("Key").Ascending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_Identities_UserId_Type_Key").OnTable("Identities");
            Delete.UniqueConstraint("UQ_Identities_UserId_Key").FromTable("Identities");
            Delete.PrimaryKey("PK_Identities_Id").FromTable("Identities");
            Delete.Table("Identities");
        }
    }
}