using FluentMigrator;

namespace Crm.Apps.Activities.Migrations
{
    [Migration(20190712204802)]
    public class Migration20190712204802AddTableActivities : Migration
    {
        public override void Up()
        {
            Create.Table("Activities")
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey("PK_Activities_Id")
                .WithColumn("AccountId").AsGuid().NotNullable()
                .WithColumn("TypeId").AsGuid().NotNullable()
                .WithColumn("StatusId").AsGuid().NotNullable()
                .WithColumn("LeadId").AsGuid().NotNullable()
                .WithColumn("CompanyId").AsGuid().NotNullable()
                .WithColumn("ContactId").AsGuid().NotNullable()
                .WithColumn("DealId").AsGuid().NotNullable()
                .WithColumn("CreateUserId").AsGuid().NotNullable()
                .WithColumn("ResponsibleUserId").AsGuid().NotNullable()
                .WithColumn("Name").AsString().NotNullable()
                .WithColumn("Description").AsString().NotNullable()
                .WithColumn("Result").AsString().NotNullable()
                .WithColumn("Priority").AsByte().NotNullable()
                .WithColumn("StartDateTime").AsDateTime2().NotNullable()
                .WithColumn("EndDateTime").AsDateTime2().Nullable()
                .WithColumn("DeadLineDateTime").AsDateTime2().Nullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable();

            Create.ForeignKey("FK_Activities_TypeId")
                .FromTable("Activities").ForeignColumn("TypeId")
                .ToTable("ActivityTypes").PrimaryColumn("Id");

            Create.ForeignKey("FK_Activities_StatusId")
                .FromTable("Activities").ForeignColumn("StatusId")
                .ToTable("ActivityStatuses").PrimaryColumn("Id");

            Create.Index(
                    "IX_Activities_AccountId_TypeId_StatusId_LeadId_CompanyId_ContactId_DealId_CreateUserId_" +
                    "ResponsibleUserId_Name_Description_Result_Priority_StartDateTime_EndDateTime_DeadLineDateTime_" +
                    "IsDeleted_CreateDateTime")
                .OnTable("Activities")
                .OnColumn("AccountId").Descending()
                .OnColumn("TypeId").Descending()
                .OnColumn("StatusId").Descending()
                .OnColumn("LeadId").Descending()
                .OnColumn("CompanyId").Descending()
                .OnColumn("ContactId").Descending()
                .OnColumn("DealId").Descending()
                .OnColumn("CreateUserId").Descending()
                .OnColumn("ResponsibleUserId").Descending()
                .OnColumn("Name").Ascending()
                .OnColumn("Description").Ascending()
                .OnColumn("Result").Ascending()
                .OnColumn("Priority").Ascending()
                .OnColumn("StartDateTime").Descending()
                .OnColumn("EndDateTime").Descending()
                .OnColumn("DeadLineDateTime").Descending()
                .OnColumn("IsDeleted").Ascending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index(
                    "IX_Activities_AccountId_TypeId_StatusId_LeadId_CompanyId_ContactId_DealId_CreateUserId_" +
                    "ResponsibleUserId_Name_Description_Result_Priority_StartDateTime_EndDateTime_DeadLineDateTime_" +
                    "IsDeleted_CreateDateTime")
                .OnTable("Activities");

            Delete.ForeignKey("FK_Activities_TypeId").OnTable("Activities");
            Delete.ForeignKey("FK_Activities_StatusId").OnTable("Activities");
            Delete.Table("Activities");
        }
    }
}