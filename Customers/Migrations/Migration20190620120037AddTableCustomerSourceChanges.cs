using FluentMigrator;

namespace Crm.Apps.Customers.Migrations
{
    [Migration(20190620120037)]
    public class Migration20190620120037AddTableCustomerSourceChanges : Migration
    {
        public override void Up()
        {
            Create.Table("CustomerSourceChanges")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("ChangerUserId").AsGuid().NotNullable()
                .WithColumn("SourceId").AsGuid().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable()
                .WithColumn("OldValueJson").AsString().Nullable()
                .WithColumn("NewValueJson").AsString().Nullable();

            Create.PrimaryKey("PK_CustomerSourceChanges_Id").OnTable("CustomerSourceChanges")
                .Column("Id");

            Create.Index("IX_CustomerSourceChanges_SourceId_CreateDateTime").OnTable("CustomerSourceChanges")
                .OnColumn("SourceId").Ascending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_CustomerSourceChanges_SourceId_CreateDateTime").OnTable("CustomerSourceChanges");
            Delete.PrimaryKey("PK_CustomerSourceChanges_Id").FromTable("CustomerSourceChanges");
            Delete.Table("CustomerSourceChanges");
        }
    }
}
