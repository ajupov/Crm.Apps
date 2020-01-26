using FluentMigrator;

namespace Crm.Apps.Deals.Migrations
{
    [Migration(20190702224433)]
    public class Migration20190702224433AddTableDeals : Migration
    {
        public override void Up()
        {
            Create.Table("Deals")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("AccountId").AsGuid().NotNullable()
                .WithColumn("TypeId").AsGuid().NotNullable()
                .WithColumn("StatusId").AsGuid().NotNullable()
                .WithColumn("CompanyId").AsGuid().Nullable()
                .WithColumn("ContactId").AsGuid().Nullable()
                .WithColumn("CreateUserId").AsGuid().NotNullable()
                .WithColumn("ResponsibleUserId").AsGuid().Nullable()
                .WithColumn("Name").AsString().NotNullable()
                .WithColumn("StartDateTime").AsDateTime2().NotNullable()
                .WithColumn("EndDateTime").AsDateTime2().Nullable()
                .WithColumn("Sum").AsDecimal().NotNullable()
                .WithColumn("SumWithoutDiscount").AsDecimal().NotNullable()
                .WithColumn("FinishProbability").AsByte().NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("ModifyDateTime").AsDateTime2().Nullable();

            Create.PrimaryKey("PK_Deals_Id").OnTable("Deals")
                .Column("Id");

            Create.ForeignKey("FK_Deals_TypeId")
                .FromTable("Deals").ForeignColumn("TypeId")
                .ToTable("DealTypes").PrimaryColumn("Id");

            Create.ForeignKey("FK_Deals_StatusId")
                .FromTable("Deals").ForeignColumn("StatusId")
                .ToTable("DealStatuses").PrimaryColumn("Id");

            Create.Index("IX_Deals_AccountId_CreateUserId_ResponsibleUserId_CreateDateTime").OnTable("Deals")
                .OnColumn("AccountId").Ascending()
                .OnColumn("CreateUserId").Ascending()
                .OnColumn("ResponsibleUserId").Ascending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_Deals_AccountId_CreateUserId_ResponsibleUserId_CreateDateTime").OnTable("Deals");
            Delete.ForeignKey("FK_Deals_TypeId").OnTable("Deals");
            Delete.ForeignKey("FK_Deals_StatusId").OnTable("Deals");
            Delete.PrimaryKey("PK_Deals_Id").FromTable("Deals");
            Delete.Table("Deals");
        }
    }
}