using FluentMigrator;

namespace Crm.Apps.Suppliers.Migrations
{
    [Migration(20211115230000)]
    public class Migration20211115230000AddTableSuppliers : Migration
    {
        public override void Up()
        {
            Create.Table("Suppliers")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("AccountId").AsGuid().NotNullable()
                .WithColumn("CreateUserId").AsGuid().Nullable()
                .WithColumn("Name").AsString(64).Nullable()
                .WithColumn("Phone").AsString(20).Nullable()
                .WithColumn("Email").AsString(256).Nullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("ModifyDateTime").AsDateTime2().Nullable();

            Create.PrimaryKey("PK_Suppliers_Id").OnTable("Suppliers")
                .Column("Id");

            Create.Index("IX_Suppliers_AccountId_CreateDateTime")
                .OnTable("Suppliers")
                .OnColumn("AccountId").Ascending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_Suppliers_AccountId_CreateDateTime").OnTable("Suppliers");
            Delete.PrimaryKey("PK_Suppliers_Id").FromTable("Suppliers");
            Delete.Table("Suppliers");
        }
    }
}
