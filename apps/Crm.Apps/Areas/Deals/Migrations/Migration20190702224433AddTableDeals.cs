using FluentMigrator;

namespace Crm.Apps.Areas.Deals.Migrations
{
    [Migration(20190702224433)]
    public class Migration20190702224433AddTableDeals : Migration
    {
        public override void Up()
        {
            Create.Table("Deals")
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey("PK_Deals_Id")
                .WithColumn("AccountId").AsGuid().NotNullable()
                .WithColumn("TypeId").AsGuid().NotNullable()
                .WithColumn("StatusId").AsGuid().NotNullable()
                .WithColumn("CompanyId").AsGuid().NotNullable()
                .WithColumn("ContactId").AsGuid().NotNullable()
                .WithColumn("CreateUserId").AsGuid().NotNullable()
                .WithColumn("ResponsibleUserId").AsGuid().NotNullable()
                .WithColumn("Name").AsString().NotNullable()
                .WithColumn("StartDateTime").AsDateTime2().NotNullable()
                .WithColumn("EndDateTime").AsDateTime2().Nullable()
                .WithColumn("Sum").AsDecimal().NotNullable()
                .WithColumn("SumWithoutDiscount").AsDecimal().NotNullable()
                .WithColumn("FinishProbability").AsByte().NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable();

            Create.ForeignKey("FK_Deals_TypeId")
                .FromTable("Deals").ForeignColumn("TypeId")
                .ToTable("DealTypes").PrimaryColumn("Id");

            Create.ForeignKey("FK_Deals_StatusId")
                .FromTable("Deals").ForeignColumn("StatusId")
                .ToTable("DealStatuses").PrimaryColumn("Id");

            Create.Index(
                    "IX_Deals_AccountId_TypeId_TypeId_StatusId_CompanyId_CreateUserId_ResponsibleUserId_Name_" +
                    "StartDateTime_EndDateTime_Sum_SumWithoutDiscount_FinishProbability_IsDeleted_CreateDateTime")
                .OnTable("Deals")
                .OnColumn("AccountId").Descending()
                .OnColumn("TypeId").Descending()
                .OnColumn("StatusId").Descending()
                .OnColumn("CompanyId").Descending()
                .OnColumn("ContactId").Descending()
                .OnColumn("CreateUserId").Descending()
                .OnColumn("ResponsibleUserId").Descending()
                .OnColumn("Name").Ascending()
                .OnColumn("StartDateTime").Descending()
                .OnColumn("EndDateTime").Descending()
                .OnColumn("Sum").Ascending()
                .OnColumn("SumWithoutDiscount").Ascending()
                .OnColumn("FinishProbability").Ascending()
                .OnColumn("IsDeleted").Ascending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index(
                    "IX_Deals_AccountId_TypeId_TypeId_StatusId_CompanyId_CreateUserId_ResponsibleUserId_Name_" +
                    "StartDateTime_EndDateTime_Sum_SumWithoutDiscount_FinishProbability_IsDeleted_CreateDateTime")
                .OnTable("Deals");

            Delete.ForeignKey("FK_Deals_TypeId").OnTable("Deals");
            Delete.ForeignKey("FK_Deals_StatusId").OnTable("Deals");
            Delete.Table("Deals");
        }
    }
}