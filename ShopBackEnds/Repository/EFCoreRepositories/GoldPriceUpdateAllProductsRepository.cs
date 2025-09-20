using Microsoft.EntityFrameworkCore;
using ShopBackEnd.Data.Enums;
using ShopBackEnd.Repository.Context;

namespace ShopBackEnd.Repository.EFCoreRepositories
{
    public interface IGoldPriceUpdateAllProductsRepository
    {
        Task UpdateProductPricesBasedOnGoldPrice(decimal goldPriceInGrams);
        Task ClearAllCarts();
    }
    public class GoldPriceUpdateAllProductsRepository : IGoldPriceUpdateAllProductsRepository
    {
        private readonly ShopDbContext _context;
        private readonly ILogger<GoldPriceUpdateAllProductsRepository> _logger;

        public GoldPriceUpdateAllProductsRepository(ShopDbContext context, ILogger<GoldPriceUpdateAllProductsRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task UpdateProductPricesBasedOnGoldPrice(decimal goldPriceInGrams)
        {
            var goldProducts = await _context.Products
                .Where(p => p.ProductType == ProductType.Jewlery ||
                            p.ProductType == ProductType.GoldBars ||
                            p.ProductType == ProductType.GoldCoins)
                .ToListAsync();

            foreach (var product in goldProducts)
            {
                product.SellingPrice = (product.GoldWeightInGrams * goldPriceInGrams + product.AdditionalValue) * 1.4m;
                product.LastModifiedDate = DateTime.Now;
            }

            await _context.SaveChangesAsync();
        }

        public async Task ClearAllCarts()
        {
            var allCarts = await _context.Carts
                .Include(c => c.Items)
                .ToListAsync();

            foreach (var cart in allCarts)
            {
                _context.CartItems.RemoveRange(cart.Items);
                cart.Items.Clear();
            }

            await _context.SaveChangesAsync();
        }
    }
}
