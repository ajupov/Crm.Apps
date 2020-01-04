using FluentMigrator;

namespace Crm.Apps.Products.Migrations
{
    [Migration(20190529000315)]
    public class Migration20190529000315AddTableProducts : Migration
    {
        public override void Up()
        {
            Create.Table("Products")
                .WithColumn("Id").AsGuid().NotNullable()
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
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("ModifyDateTime").AsDateTime2().Nullable();

            Create.PrimaryKey("PK_Products_Id").OnTable("Products")
                .Column("Id");

            Create.ForeignKey("FK_Products_StatusId")
                .FromTable("Products").ForeignColumn("StatusId")
                .ToTable("ProductStatuses").PrimaryColumn("Id");

            Create.UniqueConstraint("UQ_Products_AccountId_Name").OnTable("Products")
                .Columns("AccountId", "Name");

            Create.Index("IX_Products_AccountId_Name_VendorCode_CreateDateTime").OnTable("Products")
                .OnColumn("AccountId").Ascending()
                .OnColumn("Name").Ascending()
                .OnColumn("VendorCode").Ascending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_Products_AccountId_Name_VendorCode_CreateDateTime").OnTable("Products");
            Delete.UniqueConstraint("UQ_Products_AccountId_Name").FromTable("Products");
            Delete.ForeignKey("FK_Products_StatusId").OnTable("Products");
            Delete.PrimaryKey("PK_Products_Id").FromTable("Products");
            Delete.Table("Products");
        }
    }
}