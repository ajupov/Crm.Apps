using FluentMigrator;

namespace Crm.Apps.Identities.Migrations
{
    [Migration(20190525175248)]
    public class Migration20190525175248AddTableIdentityTokens : Migration
    {
        public override void Up()
        {
            Create.Table("IdentityTokens")
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey("PK_IdentityTokens_Id")
                .WithColumn("IdentityId").AsGuid().NotNullable()
                .WithColumn("Value").AsString().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("ExpirationDateTime").AsDateTime2().NotNullable()
                .WithColumn("UseDateTime").AsDateTime2().Nullable();

            Create.ForeignKey("FK_IdentityTokens_IdentityId")
                .FromTable("IdentityTokens").ForeignColumn("IdentityId")
                .ToTable("Identities").PrimaryColumn("Id");

            Create.Index("IX_IdentityTokens_IdentityId_Value").OnTable("IdentityTokens")
                .OnColumn("IdentityId").Descending()
                .OnColumn("Value").Ascending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_IdentityTokens_IdentityId_Value").OnTable("IdentityTokens");
            Delete.ForeignKey("FK_IdentityTokens_IdentityId").OnTable("IdentityTokens");
            Delete.Table("IdentityTokens");
        }
    }
}