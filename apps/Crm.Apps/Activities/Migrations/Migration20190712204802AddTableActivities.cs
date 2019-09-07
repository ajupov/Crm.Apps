using FluentMigrator;

namespace Crm.Apps.Activities.Migrations
{
    [Migration(20190712204802)]
    public class Migration20190712204802AddTableActivities : Migration
    {
        public override void Up()
        {
            Create.Table("Activities")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("AccountId").AsGuid().NotNullable()
                .WithColumn("TypeId").AsGuid().NotNullable()
                .WithColumn("StatusId").AsGuid().NotNullable()
                .WithColumn("LeadId").AsGuid().Nullable()
                .WithColumn("CompanyId").AsGuid().Nullable()
                .WithColumn("ContactId").AsGuid().Nullable()
                .WithColumn("DealId").AsGuid().Nullable()
                .WithColumn("CreateUserId").AsGuid().NotNullable()
                .WithColumn("ResponsibleUserId").AsGuid().Nullable()
                .WithColumn("Name").AsString().NotNullable()
                .WithColumn("Description").AsString().Nullable()
                .WithColumn("Result").AsString().Nullable()
                .WithColumn("Priority").AsByte().NotNullable()
                .WithColumn("StartDateTime").AsDateTime2().Nullable()
                .WithColumn("EndDateTime").AsDateTime2().Nullable()
                .WithColumn("DeadLineDateTime").AsDateTime2().Nullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable();

            Create.PrimaryKey("PK_Activities_Id").OnTable("Activities")
                .Columns("Id");

            Create.ForeignKey("FK_Activities_TypeId")
                .FromTable("Activities").ForeignColumn("TypeId")
                .ToTable("ActivityTypes").PrimaryColumn("Id");

            Create.ForeignKey("FK_Activities_StatusId")
                .FromTable("Activities").ForeignColumn("StatusId")
                .ToTable("ActivityStatuses").PrimaryColumn("Id");

            Create.Index(
                    "IX_Activities_AccountId_TypeId_StatusId_LeadId_CompanyId_ContactId_DealId_CreateUserId_ResponsibleUserId_CreateDateTime")
                .OnTable("Activities")
                .OnColumn("AccountId").Ascending()
                .OnColumn("TypeId").Ascending()
                .OnColumn("StatusId").Ascending()
                .OnColumn("LeadId").Ascending()
                .OnColumn("CompanyId").Ascending()
                .OnColumn("ContactId").Ascending()
                .OnColumn("DealId").Ascending()
                .OnColumn("CreateUserId").Ascending()
                .OnColumn("ResponsibleUserId").Ascending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index(
                    "IX_Activities_AccountId_TypeId_StatusId_LeadId_CompanyId_ContactId_DealId_CreateUserId_ResponsibleUserId_CreateDateTime")
                .OnTable("Activities");

            Delete.ForeignKey("FK_Activities_TypeId").OnTable("Activities");
            Delete.ForeignKey("FK_Activities_StatusId").OnTable("Activities");
            Delete.PrimaryKey("PK_Activities_Id").FromTable("Activities");
            Delete.Table("Activities");
        }
    }
}