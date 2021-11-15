using FluentMigrator;

namespace Crm.Apps.Suppliers.Migrations
{
    [Migration(20211115230611)]
    public class Migration20211115230611AddTableSupplierAttributeChanges : Migration
    {
        public override void Up()
        {
            Create.Table("SupplierAttributeChanges")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("ChangerUserId").AsGuid().NotNullable()
                .WithColumn("AttributeId").AsGuid().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("OldValueJson").AsString().Nullable()
                .WithColumn("NewValueJson").AsString().Nullable();

            Create.PrimaryKey("PK_SupplierAttributeChanges_Id").OnTable("SupplierAttributeChanges")
                .Column("Id");

            Create.Index("IX_SupplierAttributeChanges_AttributeId_CreateDateTime").OnTable("SupplierAttributeChanges")
                .OnColumn("AttributeId").Ascending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_SupplierAttributeChanges_AttributeId_CreateDateTime").OnTable("SupplierAttributeChanges");
            Delete.PrimaryKey("PK_SupplierAttributeChanges_Id").FromTable("SupplierAttributeChanges");
            Delete.Table("SupplierAttributeChanges");
        }
    }
}
