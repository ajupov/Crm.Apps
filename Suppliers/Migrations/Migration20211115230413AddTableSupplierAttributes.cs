using FluentMigrator;

namespace Crm.Apps.Suppliers.Migrations
{
    [Migration(20211115230413)]
    public class Migration20211115230413AddTableSupplierAttributes : Migration
    {
        public override void Up()
        {
            Create.Table("SupplierAttributes")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("AccountId").AsGuid().NotNullable()
                .WithColumn("Type").AsByte().NotNullable()
                .WithColumn("Key").AsString().NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("ModifyDateTime").AsDateTime2().Nullable();

            Create.PrimaryKey("PK_SupplierAttributes_Id").OnTable("SupplierAttributes")
                .Column("Id");

            Create.UniqueConstraint("UQ_SupplierAttributes_AccountId_Key").OnTable("SupplierAttributes")
                .Columns("AccountId", "Key");

            Create.Index("IX_SupplierAttributes_AccountId").OnTable("SupplierAttributes")
                .OnColumn("AccountId").Ascending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_SupplierAttributes_AccountId").OnTable("SupplierAttributes");
            Delete.UniqueConstraint("UQ_SupplierAttributes_AccountId_Key").FromTable("SupplierAttributes");
            Delete.PrimaryKey("PK_SupplierAttributes_Id").FromTable("SupplierAttributes");
            Delete.Table("SupplierAttributes");
        }
    }
}
