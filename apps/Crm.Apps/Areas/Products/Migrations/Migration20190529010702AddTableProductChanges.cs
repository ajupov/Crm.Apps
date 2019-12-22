using FluentMigrator;

namespace Crm.Apps.Areas.Products.Migrations
{
    [Migration(20190529010702)]
    public class Migration20190529010702AddTableProductChanges : Migration
    {
        public override void Up()
        {
            Create.Table("ProductChanges")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("ChangerUserId").AsGuid().NotNullable()
                .WithColumn("ProductId").AsGuid().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("OldValueJson").AsString().Nullable()
                .WithColumn("NewValueJson").AsString().Nullable();

            Create.PrimaryKey("PK_ProductChanges_Id").OnTable("ProductChanges")
                .Column("Id");

            Create.Index("IX_ProductChanges_ProductId_CreateDateTime").OnTable("ProductChanges")
                .OnColumn("ProductId").Ascending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_ProductChanges_ProductId_CreateDateTime").OnTable("ProductChanges");
            Delete.PrimaryKey("PK_ProductChanges_Id").FromTable("ProductChanges");
            Delete.Table("ProductChanges");
        }
    }
}