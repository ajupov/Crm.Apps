using FluentMigrator;

namespace Crm.Apps.Contacts.Migrations
{
    [Migration(20190630141422)]
    public class Migration20190630141422AddTableContactBankAccounts : Migration
    {
        public override void Up()
        {
            Create.Table("ContactBankAccounts")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("ContactId").AsGuid().NotNullable()
                .WithColumn("Number").AsString(64).NotNullable()
                .WithColumn("BankNumber").AsString(64).NotNullable()
                .WithColumn("BankCorrespondentNumber").AsString(64).NotNullable()
                .WithColumn("BankName").AsString(256).NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable();

            Create.PrimaryKey("PK_ContactBankAccounts_Id").OnTable("ContactBankAccounts")
                .Column("Id");

            Create.ForeignKey("FK_ContactBankAccounts_ContactId")
                .FromTable("ContactBankAccounts").ForeignColumn("ContactId")
                .ToTable("Contacts").PrimaryColumn("Id");

            Create.Index("IX_ContactBankAccounts_ContactId").OnTable("ContactBankAccounts")
                .OnColumn("ContactId").Ascending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_ContactBankAccounts_ContactId").OnTable("ContactBankAccounts");
            Delete.ForeignKey("FK_ContactBankAccounts_ContactId").OnTable("ContactBankAccounts");
            Delete.PrimaryKey("PK_ContactBankAccounts_Id").FromTable("ContactBankAccounts");
            Delete.Table("ContactBankAccounts");
        }
    }
}