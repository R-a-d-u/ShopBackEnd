using FluentMigrator;

namespace ShopBackEnd.Migrations
{
    [Migration(2025052800002)]
    public class RemoveValidColumnFromGoldHistory : Migration
    {
        private const string SchemaName = "shop";

        public override void Up()
        {
            Delete.Column("Valid").FromTable("GoldHistory").InSchema(SchemaName);
        }

        public override void Down()
        {
            Alter.Table("GoldHistory").InSchema(SchemaName)
                .AddColumn("Valid").AsBoolean().NotNullable().WithDefaultValue(true);
        }
    }
}