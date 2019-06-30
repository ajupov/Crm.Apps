using FluentMigrator;

namespace Crm.Apps.Contacts.Migrations
{
    [Migration(20190630141422)]
    public class Migration20190630141422AddTableContactBankAccounts : Migration
    {
        public override void Up()
        {
            Create.Table("ContactBankAccounts")
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey("PK_ContactBankAccounts_Id")
                .WithColumn("ContactId").AsGuid().NotNullable()
                .WithColumn("Number").AsString(64).NotNullable()
                .WithColumn("BankNumber").AsString(64).NotNullable()
                .WithColumn("BankCorrespondentNumber").AsString(64).NotNullable()
                .WithColumn("BankName").AsString(256).NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable();

            Create.ForeignKey("FK_ContactBankAccounts_ContactId")
                .FromTable("ContactBankAccounts").ForeignColumn("ContactId")
                .ToTable("Contacts").PrimaryColumn("Id");

            Create.Index(
                    "IX_ContactBankAccounts_ContactId_Number_BankNumber_BankCorrespondentNumber_BankName_CreateDateTime")
                .OnTable("ContactBankAccounts")
                .OnColumn("ContactId").Descending()
                .OnColumn("Number").Ascending()
                .OnColumn("BankNumber").Ascending()
                .OnColumn("BankCorrespondentNumber").Ascending()
                .OnColumn("BankName").Ascending()
                .OnColumn("IsDeleted").Ascending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index(
                    "IX_ContactBankAccounts_ContactId_Number_BankNumber_BankCorrespondentNumber_BankName_CreateDateTime")
                .OnTable("ContactBankAccounts");
            Delete.ForeignKey("FK_ContactBankAccounts_ContactId").OnTable("ContactBankAccounts");
            Delete.Table("ContactBankAccounts");
        }
    }
}