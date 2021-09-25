using FluentMigrator;

namespace Crm.Apps.Orders.Migrations
{
    [Migration(20190702232123)]
    public class Migration20190702232123AddTableOrderComments : Migration
    {
        public override void Up()
        {
            Create.Table("OrderComments")
                .WithColumn("Id").AsGuid().NotNullable()
                .WithColumn("OrderId").AsGuid().NotNullable()
                .WithColumn("CommentatorUserId").AsGuid().NotNullable()
                .WithColumn("Value").AsString().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable();

            Create.PrimaryKey("PK_OrderComments_Id").OnTable("OrderComments")
                .Column("Id");

            Create.Index("IX_OrderComments_OrderId_CreateDateTime").OnTable("OrderComments")
                .OnColumn("OrderId").Ascending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_OrderComments_OrderId_CommentatorUserId_Value_CreateDateTime").OnTable("OrderComments");
            Delete.PrimaryKey("PK_OrderComments_Id").FromTable("OrderComments");
            Delete.Table("OrderComments");
        }
    }
}
