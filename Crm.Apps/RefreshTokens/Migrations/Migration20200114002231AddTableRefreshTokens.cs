using FluentMigrator;

namespace Crm.Apps.RefreshTokens.Migrations
{
    [Migration(20200114002231)]
    public class Migration20200114002231AddTableRefreshTokens : Migration
    {
        public override void Up()
        {
            Create.Table("RefreshTokens")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("UserId").AsGuid().NotNullable()
                .WithColumn("Key").AsString().NotNullable()
                .WithColumn("Value").AsString().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("ExpirationDateTime").AsDateTime2().Nullable();

            Create.PrimaryKey("PK_RefreshTokens_Id").OnTable("RefreshTokens")
                .Column("Id");

            Create.Index("IX_RefreshTokens_Key_ExpirationDateTime").OnTable("RefreshTokens")
                .OnColumn("Key").Ascending()
                .OnColumn("ExpirationDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_RefreshTokens_Key_ExpirationDateTime").OnTable("RefreshTokens");
            Delete.PrimaryKey("PK_RefreshTokens_Id").FromTable("RefreshTokens");
            Delete.Table("RefreshTokens");
        }
    }
}