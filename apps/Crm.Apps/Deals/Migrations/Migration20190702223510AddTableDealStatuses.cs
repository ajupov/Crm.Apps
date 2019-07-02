﻿using FluentMigrator;

namespace Crm.Apps.Deals.Migrations
{
    [Migration(20190702223510)]
    public class Migration20190702223510AddTableDealStatuses : Migration
    {
        public override void Up()
        {
            Create.Table("DealStatuses")
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey("PK_DealStatuses_Id")
                .WithColumn("AccountId").AsGuid().NotNullable()
                .WithColumn("Name").AsString(64).NotNullable()
                .WithColumn("IsFinish").AsBoolean().NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable()
                .WithColumn("CreateDateTime").AsDateTime2().NotNullable();

            Create.UniqueConstraint("UQ_DealStatuses_AccountId_Name").OnTable("DealStatuses")
                .Columns("AccountId", "Name");

            Create.Index("IX_DealStatuses_AccountId_Name_IsFinish_IsDeleted_CreateDateTime")
                .OnTable("DealStatuses")
                .OnColumn("AccountId").Descending()
                .OnColumn("Name").Ascending()
                .OnColumn("IsFinish").Ascending()
                .OnColumn("IsDeleted").Ascending()
                .OnColumn("CreateDateTime").Descending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Index("IX_DealStatuses_AccountId_Name_IsFinish_IsDeleted_CreateDateTime").OnTable("DealStatuses");
            Delete.UniqueConstraint("UQ_DealStatuses_AccountId_Name").FromTable("DealStatuses");
            Delete.Table("DealStatuses");
        }
    }
}