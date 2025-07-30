using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using ShopBackEnd.HelperClass;
using ShopBackEnd.HelperClass.JWT;
using ShopBackEnd.Validation.CartItemValidation;
using ShopBackEnd.Validation.CartValidation;
using ShopBackEnd.Validation.CategoryValidation;
using ShopBackEnd.Validation.GoldHistory;
using ShopBackEnd.Validation.OrderValidation;
using ShopBackEnd.Validation.ProductValidation;
using ShopBackEnd.Validation.UserValidation;
using ShopBackEnd.Validations;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Cors.Infrastructure;

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

MigrationCode.RunMigrations(connectionString);


builder.Services.AddHttpClient();
builder.Configuration.AddUserSecrets<Program>();
//
builder.Services.AddFluentValidationAutoValidation();

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
