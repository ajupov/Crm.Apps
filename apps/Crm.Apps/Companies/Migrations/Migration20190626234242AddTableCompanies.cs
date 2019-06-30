using FluentMigrator;

namespace Crm.Apps.Companies.Migrations
{
    [Migration(20190626234242)]
    public class Migration20190626234242AddTableCompanies : Migration
    {
        public override void Up()
        {
            Create.Table("Companies")
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey("PK_Companies_Id")
                .WithColumn("AccountId").AsGuid().NotNullable()
                .WithColumn("LeadId").AsGuid().NotNullable()
                .WithColumn("CreateUserId").AsGuid().NotNullable()
                .WithColumn("ResponsibleUserId").AsGuid().NotNullable()
                .WithColumn("Type").AsByte().NotNullable()
                .WithColumn("IndustryType").AsByte().NotNullable()
                .WithColumn("FullName").AsString(256).NotNullable()
                .WithColumn("ShortName").AsString(64).NotNullable()
                .WithColumn("Phone").AsString(10).NotNullable()
                .WithColumn("Email").AsString(256).NotNullable()
                .WithColumn("TaxNumber").AsString(64).NotNullable()
                .WithColumn("RegistrationNumber").AsString(64).NotNullable()
                .WithColumn("RegistrationDate").AsDateTime2().Nullable()
                .WithColumn("EmployeesCount").AsInt32().NotNullable()
                .WithColumn("YearlyTurnover").AsDecimal(18, 2).NotNullable()
                .WithColumn("JuridicalPostcode").AsString(8).NotNullable()
                .WithColumn("JuridicalCountry").AsString(64).NotNullable()
                .WithColumn("JuridicalRegion").AsString(64).NotNullable()
                .WithColumn("JuridicalProvince").AsString(64).NotNullable()
                .WithColumn("JuridicalCity").AsString(64).NotNullable()
                .WithColumn("JuridicalStreet").AsString(64).NotNullable()
                .WithColumn("JuridicalHouse").AsString(64).NotNullable()
                .WithColumn("JuridicalApartment").AsString(64).NotNullable()
                .WithColumn("LegalPostcode").AsString(8).NotNullable()
                .WithColumn("LegalCountry").AsString(64).NotNullable()
                .WithColumn("LegalRegion").AsString(64).NotNullable()
                .WithColumn("LegalProvince").AsString(64).NotNullable()
                .WithColumn("LegalCity").AsString(64).NotNullable()
                .WithColumn("LegalStreet").AsString(64).NotNullable()
                .WithColumn("LegalHouse").AsString(64).NotNullable()
                .WithColumn("LegalApartment").AsString(64).NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable();

            Create.UniqueConstraint("UQ_Companies_TaxNumber").OnTable("Companies")
                .Columns("AccountId", "TaxNumber");

            Create.Index(
                    "IX_Companies_AccountId_LeadId_CreateUserId_ResponsibleUserId_Type_IndustryType_FullName_" +
                    "ShortName_Phone_Email_TaxNumber_RegistrationNumber_RegistrationDate_EmployeesCount_" +
                    "YearlyTurnover_IsDeleted_CreateDateTime")
                .OnTable("Companies")
                .OnColumn("AccountId").Descending()
                .OnColumn("LeadId").Descending()
                .OnColumn("CreateUserId").Descending()
                .OnColumn("ResponsibleUserId").Descending()
                .OnColumn("Type").Ascending()
                .OnColumn("IndustryType").Ascending()
                .OnColumn("FullName").Ascending()
                .OnColumn("ShortName").Ascending()
                .OnColumn("Phone").Ascending()
                .OnColumn("Email").Ascending()
                .OnColumn("TaxNumber").Ascending()
                .OnColumn("RegistrationNumber").Ascending()
                .OnColumn("RegistrationDate").Descending()
                .OnColumn("EmployeesCount").Ascending()
                .OnColumn("YearlyTurnover").Ascending()
                .OnColumn("IsDeleted").Ascending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();

            Create.Index(
                    "IX_Companies_AccountId_JuridicalPostcode_JuridicalCountry_JuridicalCountry_JuridicalRegion_" +
                    "JuridicalProvince_JuridicalCity_JuridicalStreet_JuridicalHouse_JuridicalApartment_IsDeleted_CreateDateTime")
                .OnTable("Companies")
                .OnColumn("AccountId").Descending()
                .OnColumn("JuridicalPostcode").Ascending()
                .OnColumn("JuridicalCountry").Ascending()
                .OnColumn("JuridicalRegion").Ascending()
                .OnColumn("JuridicalProvince").Ascending()
                .OnColumn("JuridicalCity").Ascending()
                .OnColumn("JuridicalStreet").Ascending()
                .OnColumn("JuridicalHouse").Ascending()
                .OnColumn("JuridicalApartment").Ascending()
                .OnColumn("IsDeleted").Ascending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();

            Create.Index(
                    "IX_Companies_AccountId_LegalCountry_LegalRegion_LegalProvince_LegalCity_LegalStreet_LegalHouse_" +
                    "LegalApartment_IsDeleted_CreateDateTime")
                .OnTable("Companies")
                .OnColumn("AccountId").Descending()
                .OnColumn("LegalCountry").Ascending()
                .OnColumn("LegalRegion").Ascending()
                .OnColumn("LegalProvince").Ascending()
                .OnColumn("LegalCity").Ascending()
                .OnColumn("LegalStreet").Ascending()
                .OnColumn("LegalHouse").Ascending()
                .OnColumn("LegalApartment").Ascending()
                .OnColumn("IsDeleted").Ascending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index(
                    "IX_Companies_AccountId_LeadId_CreateUserId_ResponsibleUserId_Type_IndustryType_FullName_" +
                    "ShortName_Phone_Email_TaxNumber_RegistrationNumber_RegistrationDate_EmployeesCount_" +
                    "YearlyTurnover_IsDeleted_CreateDateTime")
                .OnTable("Companies");

            Delete.Index(
                    "IX_Companies_AccountId_JuridicalPostcode_JuridicalCountry_JuridicalCountry_JuridicalRegion_" +
                    "JuridicalProvince_JuridicalCity_JuridicalStreet_JuridicalHouse_JuridicalApartment_IsDeleted_CreateDateTime")
                .OnTable("Companies");

            Delete.Index(
                    "IX_Companies_AccountId_LegalCountry_LegalRegion_LegalProvince_LegalCity_LegalStreet_LegalHouse_" +
                    "LegalApartment_IsDeleted_CreateDateTime")
                .OnTable("Companies");

            Delete.UniqueConstraint("UQ_Companies_TaxNumber").FromTable("Companies");
            Delete.Table("Companies");
        }
    }
}