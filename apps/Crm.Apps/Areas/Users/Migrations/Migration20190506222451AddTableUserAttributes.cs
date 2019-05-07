using FluentMigrator;

namespace Crm.Apps.Areas.Users.Migrations
{
    [Migration(20190506222451)]
    public class Migration20190506222451AddTableUserAttributes : Migration
    {
        public override void Up()
        {
            Create.Table("UserAttributes")
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey("PK_UserAttributes_Id")
                .WithColumn("AccountId").AsGuid().NotNullable()
                .WithColumn("Type").AsByte().NotNullable()
                .WithColumn("Key").AsString().NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable();

            Create.UniqueConstraint("UQ_UserAttributes_AccountId_Key").OnTable("UserAttributes")
                .Columns("AccountId", "Key");

            Create.Index("IX_UserAttributes_AccountId_Type_Key_IsDeleted").OnTable("UserAttributes")
                .OnColumn("AccountId").Descending()
                .OnColumn("Type").Ascending()
                .OnColumn("Key").Ascending()
                .OnColumn("IsDeleted").Ascending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_UserAttributes_AccountId_Type_Key_IsDeleted").OnTable("UserAttributes");
            Delete.UniqueConstraint("UQ_UserAttributes_AccountId_Key").FromTable("UserAttributes");
            Delete.Table("UserAttributes");
        }
    }
}