using FluentMigrator;

namespace Crm.Apps.Areas.Deals.Migrations
{
    [Migration(20190702231659)]
    public class Migration20190702231659AddTableDealTypeChanges : Migration
    {
        public override void Up()
        {
            Create.Table("DealTypeChanges")
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey("PK_DealTypeChanges_Id")
                .WithColumn("ChangerUserId").AsGuid().NotNullable()
                .WithColumn("TypeId").AsGuid().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("OldValueJson").AsString().NotNullable()
                .WithColumn("NewValueJson").AsString().NotNullable();

            Create.Index("IX_DealTypeChanges_ChangerUserId_TypeId_CreateDateTime")
                .OnTable("DealTypeChanges")
                .OnColumn("ChangerUserId").Descending()
                .OnColumn("TypeId").Descending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_DealTypeChanges_ChangerUserId_TypeId_CreateDateTime")
                .OnTable("DealTypeChanges");
            Delete.Table("DealTypeChanges");
        }
    }
}