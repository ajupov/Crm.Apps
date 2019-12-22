using FluentMigrator;

namespace Crm.Apps.Areas.Deals.Migrations
{
    [Migration(20190702232123)]
    public class Migration20190702232123AddTableDealComments : Migration
    {
        public override void Up()
        {
            Create.Table("DealComments")
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey("PK_DealComments_Id")
                .WithColumn("DealId").AsGuid().NotNullable()
                .WithColumn("CommentatorUserId").AsGuid().NotNullable()
                .WithColumn("Value").AsString().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable();

            Create.Index("IX_DealComments_DealId_CommentatorUserId_Value_CreateDateTime").OnTable("DealComments")
                .OnColumn("DealId").Descending()
                .OnColumn("CommentatorUserId").Descending()
                .OnColumn("Value").Ascending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_DealComments_DealId_CommentatorUserId_Value_CreateDateTime").OnTable("DealComments");
            Delete.Table("DealComments");
        }
    }
}