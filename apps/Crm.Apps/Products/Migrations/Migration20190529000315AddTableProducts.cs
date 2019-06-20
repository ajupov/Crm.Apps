using FluentMigrator;

namespace Crm.Apps.Products.Migrations
{
    [Migration(20190529000315)]
    public class Migration20190529000315AddTableProducts : Migration
    {
        public override void Up()
        {
            Create.Table("Products")
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey("PK_Products_Id")
                .WithColumn("AccountId").AsGuid().NotNullable()
                .WithColumn("ParentProductId").AsGuid().NotNullable()
                .WithColumn("Type").AsByte().NotNullable()
                .WithColumn("StatusId").AsGuid().NotNullable()
                .WithColumn("Name").AsString(64).NotNullable()
                .WithColumn("VendorCode").AsString(16).NotNullable()
                .WithColumn("Price").AsDecimal(18, 2).NotNullable()
                .WithColumn("Image").AsBinary().Nullable()
                .WithColumn("IsHidden").AsBoolean().NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable();

            Create.ForeignKey("FK_Products_StatusId")
                .FromTable("Products").ForeignColumn("StatusId")
                .ToTable("ProductStatuses").PrimaryColumn("Id");

            Create.UniqueConstraint("UQ_Products_AccountId_Name").OnTable("Products")
                .Columns("AccountId", "Name");

            Create.Index(
                    "IX_Products_AccountId_ParentProductId_Type_StatusId_Name_VendorCode_Price_IsHidden_IsDeleted_CreateDateTime")
                .OnTable("Products")
                .OnColumn("AccountId").Descending()
                .OnColumn("ParentProductId").Descending()
                .OnColumn("Type").Ascending()
                .OnColumn("StatusId").Ascending()
                .OnColumn("Name").Ascending()
                .OnColumn("VendorCode").Ascending()
                .OnColumn("Price").Ascending()
                .OnColumn("IsHidden").Ascending()
                .OnColumn("IsDeleted").Ascending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index(
                    "IX_Products_AccountId_ParentProductId_Type_StatusId_Name_VendorCode_Price_IsHidden_IsDeleted_CreateDateTime")
                .OnTable("Products");
            
            Delete.UniqueConstraint("UQ_Products_AccountId_Name").FromTable("Products");
            Delete.ForeignKey("FK_Products_StatusId").OnTable("Products");
            Delete.Table("Products");
        }
    }
}