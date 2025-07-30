using FluentMigrator;
namespace ShopBackEnd.Migrations
{

    [Migration(2025050100001)] 
    public class AddUserCreationDate : Migration
    {
        private const string SchemaName = "shop";
        private const string TableName = "Users";

        public override void Up()
        {
            Alter.Table(TableName).InSchema(SchemaName)
              .AddColumn("CreationDate").AsDateTime().NotNullable()
                .WithDefault(SystemMethods.CurrentDateTime);
        }

        public override void Down()
        {
            Delete.Column("CreationDate").FromTable(TableName).InSchema(SchemaName);
        }
    }
}