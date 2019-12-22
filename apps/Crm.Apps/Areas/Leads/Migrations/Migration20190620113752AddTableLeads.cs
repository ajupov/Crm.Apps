using FluentMigrator;

namespace Crm.Apps.Areas.Leads.Migrations
{
    [Migration(20190620113752)]
    public class Migration20190620113752AddTableLeads : Migration
    {
        public override void Up()
        {
            Create.Table("Leads")
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey("PK_Leads_Id")
                .WithColumn("AccountId").AsGuid().NotNullable()
                .WithColumn("SourceId").AsGuid().NotNullable()
                .WithColumn("CreateUserId").AsGuid().NotNullable()
                .WithColumn("ResponsibleUserId").AsGuid().NotNullable()
                .WithColumn("Surname").AsString(64).NotNullable()
                .WithColumn("Name").AsString(64).NotNullable()
                .WithColumn("Patronymic").AsString(64).NotNullable()
                .WithColumn("Phone").AsString(10).NotNullable()
                .WithColumn("Email").AsString(256).NotNullable()
                .WithColumn("CompanyName").AsString(256).NotNullable()
                .WithColumn("Post").AsString(256).NotNullable()
                .WithColumn("Postcode").AsString(8).NotNullable()
                .WithColumn("Country").AsString(64).NotNullable()
                .WithColumn("Region").AsString(64).NotNullable()
                .WithColumn("Province").AsString(64).NotNullable()
                .WithColumn("City").AsString(64).NotNullable()
                .WithColumn("Street").AsString(64).NotNullable()
                .WithColumn("House").AsString(64).NotNullable()
                .WithColumn("Apartment").AsString(64).NotNullable()
                .WithColumn("OpportunitySum").AsDecimal(18, 2).NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable();

            Create.ForeignKey("FK_Leads_SourceId")
                .FromTable("Leads").ForeignColumn("SourceId")
                .ToTable("LeadSources").PrimaryColumn("Id");

            Create.Index(
                    "IX_Leads_AccountId_SourceId_CreateUserId_ResponsibleUserId_Surname_Name_Patronymic_Phone_Email_CompanyName_Post_Postcode_Country_Region_Province_City_Street_House_Apartment_OpportunitySum_IsDeleted_CreateDateTime")
                .OnTable("Leads")
                .OnColumn("AccountId").Descending()
                .OnColumn("SourceId").Descending()
                .OnColumn("CreateUserId").Descending()
                .OnColumn("ResponsibleUserId").Descending()
                .OnColumn("Surname").Ascending()
                .OnColumn("Name").Ascending()
                .OnColumn("Patronymic").Ascending()
                .OnColumn("Phone").Ascending()
                .OnColumn("Email").Ascending()
                .OnColumn("CompanyName").Ascending()
                .OnColumn("Post").Ascending()
                .OnColumn("Postcode").Ascending()
                .OnColumn("Country").Ascending()
                .OnColumn("Region").Ascending()
                .OnColumn("Province").Ascending()
                .OnColumn("City").Ascending()
                .OnColumn("Street").Ascending()
                .OnColumn("House").Ascending()
                .OnColumn("Apartment").Ascending()
                .OnColumn("OpportunitySum").Ascending()
                .OnColumn("IsDeleted").Ascending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index(
                    "IX_Leads_AccountId_SourceId_CreateUserId_ResponsibleUserId_Surname_Name_Patronymic_Phone_Email_CompanyName_Post_Postcode_Country_Region_Province_City_Street_House_Apartment_OpportunitySum_IsDeleted_CreateDateTime")
                .OnTable("Leads");

            Delete.UniqueConstraint("UQ_Leads_AccountId_Name").FromTable("Leads");
            Delete.ForeignKey("FK_Leads_SourceId").OnTable("Leads");
            Delete.Table("Leads");
        }
    }
}