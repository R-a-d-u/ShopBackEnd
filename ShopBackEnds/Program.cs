using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using ShopBackEnd.HelperClass;
using ShopBackEnd.HelperClass.JWT;
using ShopBackEnd.Repository;
using ShopBackEnd.Repository.Context;
using ShopBackEnd.Repository.EFCoreRepositories;
using ShopBackEnd.Service;
using ShopBackEnd.Services;
using ShopBackEnd.Validation.CartItemValidation;
using ShopBackEnd.Validation.CartValidation;
using ShopBackEnd.Validation.CategoryValidation;
using ShopBackEnd.Validation.GoldHistory;
using ShopBackEnd.Validation.OrderValidation;
using ShopBackEnd.Validation.ProductValidation;
using ShopBackEnd.Validation.UserValidation;
using ShopBackEnd.Validations;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200")
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});

string? connectionString = builder.Configuration.GetConnectionString("SqlServer");
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Connection string 'SqlServer' not found.");
}

//MigrationCode.RunMigrations(connectionString);

builder.Services.AddDbContext<ShopDbContext>(options =>
    options.UseSqlServer(connectionString));
//
builder.Services.AddHttpClient();
builder.Configuration.AddUserSecrets<Program>();
builder.Services.AddSingleton<GoldApiFetch>();
//
builder.Services.AddFluentValidationAutoValidation();
//
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.AddScoped<IJwtService, JwtService>();
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
var key = Encoding.UTF8.GetBytes(jwtSettings.SecretKey);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidateAudience = true,
        ValidAudience = jwtSettings.Audience,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});
builder.Services.AddAuthorization();

//repositories
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IGoldHistoryRepository, GoldHistoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<ICartItemRepository, CartItemRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderItemRepository, OrderItemRepository>();
builder.Services.AddScoped<IGoldPriceUpdateAllProductsRepository, GoldPriceUpdateAllProductsRepository>();
builder.Services.AddScoped<ISaleRecordRepository, SaleRecordRepository>();

//services
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IGoldHistoryService, GoldHistoryService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<ICartItemService, CartItemService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderItemService, OrderItemService>();
builder.Services.AddScoped<ISaleRecordService, SaleRecordService>();
builder.Services.AddScoped<IEmailService, EmailService>();

//validation 
builder.Services.AddScoped<CategoryIdValidation>();
builder.Services.AddScoped<CategoryAddValidation>();
builder.Services.AddScoped<CategoryEditValidation>();
builder.Services.AddScoped<CategoryDeleteValidation>();

builder.Services.AddScoped<UserIdValidation>();
builder.Services.AddScoped<UserAddValidation>();
builder.Services.AddScoped<UserEditPasswordValidation>();
builder.Services.AddScoped<UserEditValidation>();
builder.Services.AddScoped<UserDeleteValidation>();
builder.Services.AddScoped<UserIsAdminValidaton>();
builder.Services.AddScoped<UserIsEmployeeValidaton>();

builder.Services.AddScoped<GoldHistoryIdValidation>();
builder.Services.AddScoped<GoldHistoryAddValidation>();

builder.Services.AddScoped<ProductDtoAddValidation>();
builder.Services.AddScoped<ProductEditInformationValidation>();
builder.Services.AddScoped<ProductEditSellingPriceValidation>();
builder.Services.AddScoped<ProductEditStateValidation>();
builder.Services.AddScoped<ProductEditStockValidation>();
builder.Services.AddScoped<ProductFilterValidation>();
builder.Services.AddScoped<ProductIdValidation>();
builder.Services.AddScoped<ProductValidation>();

builder.Services.AddScoped<CartIdValidation>();

builder.Services.AddScoped<CartItemIdValidation>();
builder.Services.AddScoped<CartItemAddValidation>();
builder.Services.AddScoped<CartItemEditQuantityValidator>();

builder.Services.AddScoped<OrderDtoAddValidation>();
builder.Services.AddScoped<OrderIdValidation>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowAngular");
app.UseRouting();
app.UseHttpsRedirection();


app.UseAuthorization();

app.MapControllers();

app.Run();
