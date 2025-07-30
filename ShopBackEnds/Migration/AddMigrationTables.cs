//using FluentMigrator;
//
//namespace ShopBackEnd.Migrations
//{
//    // This single migration creates the entire 'shop' schema as defined
//    // by the cumulative effect of migrations:
//    // - 2025022200101 (_4AddLogTable - Initial Schema)
//    // - 202503160101 (_5UpdateCategoryAndUserTables)
//    // - 2025032400101 (_5ModifySaleRecordsTable)
//    // - 2025022200102 (AddCartTable)
//    // - 2025022200103 (AddCartItemsTable)
//    // Use a new timestamp reflecting the combination date or the latest state.
//    [Migration(2025041600001)] // New timestamp for the combined migration
//    public class AddMigrationTables : Migration
//    {
//        private const string SchemaName = "shop";
//
//        public override void Up()
//        {
//            // 1. Create Schema
//            IfDatabase("SqlServer").Execute.Sql(@"
//                 IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = 'shop')
//                 EXEC('CREATE SCHEMA [shop]')
//                ");
//
//            // 2. Create Tables (in an order that respects dependencies or allows FKs later)
//
//            // Categories Table (Reflects changes from _5UpdateCategoryAndUserTables - SalePercentage & LastModifiedBy removed)
//            Create.Table("Categories").InSchema(SchemaName)
//                .WithColumn("Id").AsInt32().PrimaryKey().Identity().NotNullable()
//                .WithColumn("Name").AsString(255).NotNullable()
//                // SalePercentage removed in migration 202503160101
//                // LastModifiedBy removed in migration 202503160101
//                .WithColumn("LastModifiedDate").AsDateTime().NotNullable();
//
//            // Users Table (Reflects changes from _5UpdateCategoryAndUserTables - Salt removed, PhoneNumber added)
//            Create.Table("Users").InSchema(SchemaName)
//                .WithColumn("Id").AsInt32().PrimaryKey().Identity().NotNullable()
//                .WithColumn("IsDeleted").AsBoolean().NotNullable()
//                .WithColumn("Name").AsString(255).NotNullable()
//                .WithColumn("Email").AsString(255).NotNullable().Unique()
//                .WithColumn("Password").AsString(255).NotNullable()
//                // Salt removed in migration 202503160101
//                .WithColumn("UserAccessType").AsInt32().NotNullable()
//                .WithColumn("LastModifyDate").AsDateTime().NotNullable()
//                .WithColumn("PhoneNumber").AsString(20).NotNullable(); // Added in migration 202503160101
//
//            // Products Table (No changes across the migrations provided after initial creation)
//            Create.Table("Products").InSchema(SchemaName)
//                .WithColumn("Id").AsInt32().PrimaryKey().Identity().NotNullable()
//                .WithColumn("CategoryId").AsInt32().NotNullable() // Foreign key added later
//                .WithColumn("ProductType").AsInt32().NotNullable()
//                .WithColumn("Name").AsString(255).NotNullable()
//                .WithColumn("Image").AsString(255).Nullable()
//                .WithColumn("Description").AsString(int.MaxValue).Nullable()
//                .WithColumn("AdditionalValue").AsDecimal(18, 2).NotNullable()
//                .WithColumn("GoldWeightInGrams").AsDecimal(18, 2).NotNullable()
//                .WithColumn("SellingPrice").AsDecimal(18, 2).NotNullable()
//                .WithColumn("ProductState").AsInt32().NotNullable()
//                .WithColumn("StockQuantity").AsInt32().NotNullable()
//                .WithColumn("LastModifiedDate").AsDateTime().NotNullable();
//
//            // Analytics Table (No changes across the migrations provided)
//            Create.Table("Analytics").InSchema(SchemaName)
//                .WithColumn("Id").AsInt32().PrimaryKey().Identity().NotNullable();
//
//            // Orders Table (No changes across the migrations provided)
//            Create.Table("Orders").InSchema(SchemaName)
//                .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
//                .WithColumn("UserId").AsInt32().NotNullable() // Foreign key added later
//                .WithColumn("OrderCreatedDate").AsDateTime().NotNullable()
//                .WithColumn("OrderCompletedDate").AsDateTime().Nullable()
//                .WithColumn("TotalSum").AsDecimal(18, 2).NotNullable()
//                .WithColumn("ShippingFee").AsDecimal(18, 2).NotNullable()
//                .WithColumn("Address").AsString(int.MaxValue).NotNullable()
//                .WithColumn("PaymentMethod").AsInt32().NotNullable()
//                .WithColumn("OrderStatus").AsInt32().NotNullable();
//
//            // OrderItems Table (No changes across the migrations provided)
//            Create.Table("OrderItems").InSchema(SchemaName)
//                .WithColumn("Id").AsInt32().PrimaryKey().Identity().NotNullable()
//                .WithColumn("OrderId").AsGuid().NotNullable() // Foreign key added later
//                .WithColumn("ProductId").AsInt32().NotNullable() // Foreign key added later
//                .WithColumn("Quantity").AsInt32().NotNullable()
//                .WithColumn("Price").AsDecimal(18, 2).NotNullable();
//
//            // SaleRecords Table (Reflects changes from _5ModifySaleRecordsTable)
//            Create.Table("SaleRecords").InSchema(SchemaName)
//                .WithColumn("Id").AsInt32().PrimaryKey().Identity().NotNullable()
//                // ProductId removed in migration 2025032400101
//                // QuantitySold removed in migration 2025032400101
//                .WithColumn("SaleDate").AsDateTime().NotNullable()
//                .WithColumn("OrderItemId").AsInt32().NotNullable() // Added in migration 2025032400101
//                .WithColumn("OrderId").AsGuid().NotNullable();    // Added in migration 2025032400101
//
//            // GoldHistory Table (No changes across the migrations provided)
//            Create.Table("GoldHistory").InSchema(SchemaName)
//                .WithColumn("Id").AsInt32().PrimaryKey().Identity().NotNullable()
//                .WithColumn("Metal").AsString(255).NotNullable()
//                .WithColumn("PriceOunce").AsDecimal(18, 2).NotNullable()
//                .WithColumn("PriceGram").AsDecimal(18, 2).NotNullable()
//                .WithColumn("PercentageChange").AsDouble().NotNullable()
//                .WithColumn("Exchange").AsString(255).NotNullable()
//                .WithColumn("Timestamp").AsString(255).NotNullable() // Consider changing to AsInt64 or AsDateTime if appropriate
//                .WithColumn("Date").AsDateTime().NotNullable();
//
//            // Carts Table (Added in migration 2025022200102)
//            Create.Table("Carts").InSchema(SchemaName)
//                .WithColumn("Id").AsInt32().PrimaryKey().Identity().NotNullable()
//                .WithColumn("UserId").AsInt32().NotNullable(); // Foreign key added later
//
//            // CartItems Table (Added in migration 2025022200103)
//            Create.Table("CartItems").InSchema(SchemaName)
//                .WithColumn("Id").AsInt32().PrimaryKey().Identity().NotNullable()
//                .WithColumn("CartId").AsInt32().NotNullable() // Foreign key added later
//                .WithColumn("ProductId").AsInt32().NotNullable() // Foreign key added later
//                .WithColumn("Quantity").AsInt32().NotNullable()
//                .WithColumn("Price").AsDecimal(18, 2).NotNullable();
//
//            // 3. Create Foreign Keys
//
//            Create.ForeignKey("FK_Products_Categories")
//                .FromTable("Products").InSchema(SchemaName).ForeignColumn("CategoryId")
//                .ToTable("Categories").InSchema(SchemaName).PrimaryColumn("Id");
//
//            Create.ForeignKey("FK_Orders_Users")
//                .FromTable("Orders").InSchema(SchemaName).ForeignColumn("UserId")
//                .ToTable("Users").InSchema(SchemaName).PrimaryColumn("Id");
//
//            Create.ForeignKey("FK_OrderItems_Orders")
//                .FromTable("OrderItems").InSchema(SchemaName).ForeignColumn("OrderId")
//                .ToTable("Orders").InSchema(SchemaName).PrimaryColumn("Id");
//
//            Create.ForeignKey("FK_OrderItems_Products")
//                .FromTable("OrderItems").InSchema(SchemaName).ForeignColumn("ProductId")
//                .ToTable("Products").InSchema(SchemaName).PrimaryColumn("Id");
//
//            // SaleRecords FKs (Reflect changes from _5ModifySaleRecordsTable)
//            Create.ForeignKey("FK_SaleRecords_OrderItems")
//                .FromTable("SaleRecords").InSchema(SchemaName).ForeignColumn("OrderItemId")
//                .ToTable("OrderItems").InSchema(SchemaName).PrimaryColumn("Id");
//
//            Create.ForeignKey("FK_SaleRecords_Orders")
//                .FromTable("SaleRecords").InSchema(SchemaName).ForeignColumn("OrderId")
//                .ToTable("Orders").InSchema(SchemaName).PrimaryColumn("Id");
//
//            // Carts FK (Added in migration 2025022200102)
//            Create.ForeignKey("FK_Carts_Users")
//                .FromTable("Carts").InSchema(SchemaName).ForeignColumn("UserId")
//                .ToTable("Users").InSchema(SchemaName).PrimaryColumn("Id");
//
//            // CartItems FKs (Added in migration 2025022200103)
//            Create.ForeignKey("FK_CartItems_Carts")
//                .FromTable("CartItems").InSchema(SchemaName).ForeignColumn("CartId")
//                .ToTable("Carts").InSchema(SchemaName).PrimaryColumn("Id");
//
//            Create.ForeignKey("FK_CartItems_Products")
//                .FromTable("CartItems").InSchema(SchemaName).ForeignColumn("ProductId")
//                .ToTable("Products").InSchema(SchemaName).PrimaryColumn("Id");
//        }
//
//        public override void Down()
//        {
//            // Drop Foreign Keys first (in reverse order of dependency or creation where possible)
//            Delete.ForeignKey("FK_CartItems_Products").OnTable("CartItems").InSchema(SchemaName);
//            Delete.ForeignKey("FK_CartItems_Carts").OnTable("CartItems").InSchema(SchemaName);
//            Delete.ForeignKey("FK_Carts_Users").OnTable("Carts").InSchema(SchemaName);
//            Delete.ForeignKey("FK_SaleRecords_Orders").OnTable("SaleRecords").InSchema(SchemaName);
//            Delete.ForeignKey("FK_SaleRecords_OrderItems").OnTable("SaleRecords").InSchema(SchemaName);
//            Delete.ForeignKey("FK_OrderItems_Products").OnTable("OrderItems").InSchema(SchemaName);
//            Delete.ForeignKey("FK_OrderItems_Orders").OnTable("OrderItems").InSchema(SchemaName);
//            Delete.ForeignKey("FK_Orders_Users").OnTable("Orders").InSchema(SchemaName);
//            Delete.ForeignKey("FK_Products_Categories").OnTable("Products").InSchema(SchemaName);
//
//            // Drop Tables (in reverse order of creation)
//            Delete.Table("CartItems").InSchema(SchemaName);
//            Delete.Table("Carts").InSchema(SchemaName);
//            Delete.Table("GoldHistory").InSchema(SchemaName);
//            Delete.Table("SaleRecords").InSchema(SchemaName);
//            Delete.Table("OrderItems").InSchema(SchemaName);
//            Delete.Table("Orders").InSchema(SchemaName);
//            Delete.Table("Analytics").InSchema(SchemaName);
//            Delete.Table("Products").InSchema(SchemaName);
//            Delete.Table("Users").InSchema(SchemaName);
//            Delete.Table("Categories").InSchema(SchemaName);
//
//            // Drop Schema
//            Delete.Schema(SchemaName);
//        }
//    }
//}