using FluentMigrator;

namespace Crm.Apps.Suppliers.Migrations
{
    [Migration(20211115230645)]
    public class Migration20211115230645AddTableSupplierChanges : Migration
    {
        public override void Up()
        {
            Create.Table("SupplierChanges")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("ChangerUserId").AsGuid().NotNullable()
                .WithColumn("SupplierId").AsGuid().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("OldValueJson").AsString().Nullable()
                .WithColumn("NewValueJson").AsString().Nullable();

            Create.PrimaryKey("PK_SupplierChanges_Id").OnTable("SupplierChanges")
                .Column("Id");

            Create.Index("IX_SupplierChanges_SupplierId_CreateDateTime").OnTable("SupplierChanges")
                .OnColumn("SupplierId").Ascending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_SupplierChanges_SupplierId_CreateDateTime").OnTable("SupplierChanges");
            Delete.PrimaryKey("PK_SupplierChanges_Id").FromTable("SupplierChanges");
            Delete.Table("SupplierChanges");
        }
    }
}
