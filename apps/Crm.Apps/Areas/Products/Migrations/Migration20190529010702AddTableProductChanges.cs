using FluentMigrator;

namespace Crm.Apps.Areas.Products.Migrations
{
    [Migration(20190529010702)]
    public class Migration20190529010702AddTableProductChanges : Migration
    {
        public override void Up()
        {
            Create.Table("ProductChanges")
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey("PK_ProductChanges_Id")
                .WithColumn("ChangerUserId").AsGuid().NotNullable()
                .WithColumn("ProductId").AsGuid().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("OldValueJson").AsString().NotNullable()
                .WithColumn("NewValueJson").AsString().NotNullable();

            Create.Index("IX_ProductChanges_ChangerUserId_ProductId_CreateDateTime").OnTable("ProductChanges")
                .OnColumn("ChangerUserId").Descending()
                .OnColumn("ProductId").Descending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_ProductChanges_ChangerUserId_ProductId_CreateDateTime").OnTable("ProductChanges");
            Delete.Table("ProductChanges");
        }
    }
}