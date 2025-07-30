using FluentMigrator;

namespace ShopBackEnd.Migrations
{
    [Migration(2025041600002)] 
    public class AddUserEmailFields : Migration
    {
        private const string SchemaName = "shop";
        private const string TableName = "Users";

        public override void Up()
        {
            Alter.Table(TableName).InSchema(SchemaName)
                .AddColumn("EmailConfirmed").AsBoolean().NotNullable().WithDefaultValue(false)
                .AddColumn("EmailConfirmationToken").AsString(512).Nullable()
                .AddColumn("EmailConfirmationTokenExpiry").AsDateTime().Nullable()
                .AddColumn("PasswordResetToken").AsString(512).Nullable()
                .AddColumn("PasswordResetTokenExpiry").AsDateTime().Nullable();
        }

        public override void Down()
        {
            Delete.Column("PasswordResetTokenExpiry").FromTable(TableName).InSchema(SchemaName);
            Delete.Column("PasswordResetToken").FromTable(TableName).InSchema(SchemaName);
            Delete.Column("EmailConfirmationTokenExpiry").FromTable(TableName).InSchema(SchemaName);
            Delete.Column("EmailConfirmationToken").FromTable(TableName).InSchema(SchemaName);
            Delete.Column("EmailConfirmed").FromTable(TableName).InSchema(SchemaName);
        }
    }
}
