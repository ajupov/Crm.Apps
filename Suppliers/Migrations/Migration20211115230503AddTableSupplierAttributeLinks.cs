using FluentMigrator;

namespace Crm.Apps.Suppliers.Migrations
{
    [Migration(20211115230503)]
    public class Migration20211115230503AddTableSupplierAttributeLinks : Migration
    {
        public override void Up()
        {
            Create.Table("SupplierAttributeLinks")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("SupplierId").AsGuid().NotNullable()
                .WithColumn("AttributeId").AsGuid().NotNullable()
                .WithColumn("Value").AsString().NotNullable();

            Create.PrimaryKey("PK_SupplierAttributeLinks_Id").OnTable("SupplierAttributeLinks")
                .Column("Id");

            Create.ForeignKey("FK_SupplierAttributeLinks_SupplierId")
                .FromTable("SupplierAttributeLinks").ForeignColumn("SupplierId")
                .ToTable("Suppliers").PrimaryColumn("Id");

            Create.ForeignKey("FK_SupplierAttributeLinks_AttributeId")
                .FromTable("SupplierAttributeLinks").ForeignColumn("AttributeId")
                .ToTable("SupplierAttributes").PrimaryColumn("Id");

            Create.UniqueConstraint("UQ_SupplierAttributeLinks_SupplierId_AttributeId")
                .OnTable("SupplierAttributeLinks")
                .Columns("SupplierId", "AttributeId");

            Create.Index("IX_SupplierAttributeLinks_SupplierId_AttributeId").OnTable("SupplierAttributeLinks")
                .OnColumn("SupplierId").Ascending()
                .OnColumn("AttributeId").Ascending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_SupplierAttributeLinks_SupplierId_AttributeId").OnTable("SupplierAttributeLinks");
            Delete.UniqueConstraint("UQ_SupplierAttributeLinks_SupplierId_AttributeId")
                .FromTable("SupplierAttributeLinks");
            Delete.ForeignKey("FK_SupplierAttributeLinks_AttributeId").OnTable("SupplierAttributeLinks");
            Delete.ForeignKey("FK_SupplierAttributeLinks_SupplierId").OnTable("SupplierAttributeLinks");
            Delete.PrimaryKey("PK_SupplierAttributeLinks_Id").FromTable("SupplierAttributeLinks");
            Delete.Table("SupplierAttributeLinks");
        }
    }
}
