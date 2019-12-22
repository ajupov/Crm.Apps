using FluentMigrator;

namespace Crm.Apps.Areas.Deals.Migrations
{
    [Migration(20190702231659)]
    public class Migration20190702231659AddTableDealTypeChanges : Migration
    {
        public override void Up()
        {
            Create.Table("DealTypeChanges")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("ChangerUserId").AsGuid().NotNullable()
                .WithColumn("TypeId").AsGuid().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("OldValueJson").AsString().Nullable()
                .WithColumn("NewValueJson").AsString().Nullable();

            Create.PrimaryKey("PK_DealTypeChanges_Id").OnTable("DealTypeChanges")
                .Column("Id");

            Create.Index("IX_DealTypeChanges_TypeId_CreateDateTime").OnTable("DealTypeChanges")
                .OnColumn("TypeId").Ascending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_DealTypeChanges_TypeId_CreateDateTime").OnTable("DealTypeChanges");
            Delete.PrimaryKey("PK_DealTypeChanges_Id").FromTable("DealTypeChanges");
            Delete.Table("DealTypeChanges");
        }
    }
}