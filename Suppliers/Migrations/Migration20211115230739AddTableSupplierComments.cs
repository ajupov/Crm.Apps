using FluentMigrator;

namespace Crm.Apps.Suppliers.Migrations
{
    [Migration(20211115230739)]
    public class Migration20211115230739AddTableSupplierComments : Migration
    {
        public override void Up()
        {
            Create.Table("SupplierComments")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("SupplierId").AsGuid().NotNullable()
                .WithColumn("CommentatorUserId").AsGuid().NotNullable()
                .WithColumn("Value").AsString().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable();

            Create.PrimaryKey("PK_SupplierComments_Id").OnTable("SupplierComments")
                .Column("Id");

            Create.Index("IX_SupplierComments_SupplierId_CreateDateTime").OnTable("SupplierComments")
                .OnColumn("SupplierId").Ascending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_SupplierComments_SupplierId_CreateDateTime").OnTable("SupplierComments");
            Delete.PrimaryKey("PK_SupplierComments_Id").FromTable("SupplierComments");
            Delete.Table("SupplierComments");
        }
    }
}
