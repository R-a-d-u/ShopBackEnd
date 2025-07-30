using FluentMigrator;

namespace ShopBackEnd.Migrations
{
    [Migration(2025052800001)]
    public class AddValidColumnToGoldHistory : Migration
    {
        private const string SchemaName = "shop";

        public override void Up()
        {
            Alter.Table("GoldHistory").InSchema(SchemaName)
                .AddColumn("Valid").AsBoolean().NotNullable().WithDefaultValue(true);
        }

        public override void Down()
        {
            Delete.Column("Valid").FromTable("GoldHistory").InSchema(SchemaName);
        }
    }
}