using FluentMigrator;

namespace Crm.Apps.Contacts.Migrations
{
    [Migration(20190630140852)]
    public class Migration20190630140852AddTableContacts : Migration
    {
        public override void Up()
        {
            Create.Table("Contacts")
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey("PK_Contacts_Id")
                .WithColumn("AccountId").AsGuid().NotNullable()
                .WithColumn("LeadId").AsGuid().NotNullable()
                .WithColumn("CompanyId").AsGuid().NotNullable()
                .WithColumn("CreateUserId").AsGuid().NotNullable()
                .WithColumn("ResponsibleUserId").AsGuid().NotNullable()
                .WithColumn("Surname").AsString(64).NotNullable()
                .WithColumn("Name").AsString(64).NotNullable()
                .WithColumn("Patronymic").AsString(64).NotNullable()
                .WithColumn("Phone").AsString(10).NotNullable()
                .WithColumn("Email").AsString(256).NotNullable()
                .WithColumn("TaxNumber").AsString(64).NotNullable()
                .WithColumn("Post").AsString(256).NotNullable()
                .WithColumn("Postcode").AsString(8).NotNullable()
                .WithColumn("Country").AsString(64).NotNullable()
                .WithColumn("Region").AsString(64).NotNullable()
                .WithColumn("Province").AsString(64).NotNullable()
                .WithColumn("City").AsString(64).NotNullable()
                .WithColumn("Street").AsString(64).NotNullable()
                .WithColumn("House").AsString(64).NotNullable()
                .WithColumn("Apartment").AsString(64).NotNullable()
                .WithColumn("BirthDate").AsDate().Nullable()
                .WithColumn("Photo").AsBinary().Nullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable();

            Create.Index(
                    "IX_Contacts_AccountId_LeadId_CompanyId_CreateUserId_ResponsibleUserId_Surname_Name_Patronymic_" +
                    "Phone_Email_TaxNumber_Post_Postcode_Country_Region_Province_City_Street_House_Apartment_" +
                    "BirthDate_IsDeleted_CreateDateTime")
                .OnTable("Contacts")
                .OnColumn("AccountId").Descending()
                .OnColumn("LeadId").Descending()
                .OnColumn("CompanyId").Descending()
                .OnColumn("CreateUserId").Descending()
                .OnColumn("ResponsibleUserId").Descending()
                .OnColumn("Surname").Ascending()
                .OnColumn("Name").Ascending()
                .OnColumn("Patronymic").Ascending()
                .OnColumn("Phone").Ascending()
                .OnColumn("Email").Ascending()
                .OnColumn("TaxNumber").Ascending()
                .OnColumn("Post").Ascending()
                .OnColumn("Postcode").Ascending()
                .OnColumn("Country").Ascending()
                .OnColumn("Region").Ascending()
                .OnColumn("Province").Ascending()
                .OnColumn("City").Ascending()
                .OnColumn("Street").Ascending()
                .OnColumn("House").Ascending()
                .OnColumn("Apartment").Ascending()
                .OnColumn("BirthDate").Ascending()
                .OnColumn("IsDeleted").Ascending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index(
                    "IX_Contacts_AccountId_LeadId_CompanyId_CreateUserId_ResponsibleUserId_Surname_Name_Patronymic_" +
                    "Phone_Email_TaxNumber_Post_Postcode_Country_Region_Province_City_Street_House_Apartment_" +
                    "BirthDate_IsDeleted_CreateDateTime")
                .OnTable("Contacts");

            Delete.Table("Contacts");
        }
    }
}