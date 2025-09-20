using Microsoft.EntityFrameworkCore;
using ShopBackEnd.Data.Dto;
using ShopBackEnd.Data.Enums;
using ShopBackEnd.Repository.Context;


namespace ShopBackEnd.Repository.EFCoreRepositories
{
    public class SaleRecordRepository : ISaleRecordRepository
    {
        private readonly ShopDbContext _context;

        public SaleRecordRepository(ShopDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProductSalesSummaryDto>> GetProductSalesSummaryBetweenDates(DateTime startDate, DateTime endDate)
        {
            var salesRecords = await _context.SaleRecords
                .Where(sr => sr.SaleDate >= startDate && sr.SaleDate <= endDate)
                .Include(sr => sr.OrderItem)
                .ThenInclude(oi => oi.Product)
                .ToListAsync();

            var totalQuantitySold = salesRecords.Sum(sr => sr.OrderItem.Quantity);

            var productSalesSummary = salesRecords
                .GroupBy(sr => sr.OrderItem.Product.Id)
                .Select(g => new ProductSalesSummaryDto
                {
                    ProductName = g.First().OrderItem.Product.Name,
                    AveragePrice = Math.Round(g.Average(sr => sr.OrderItem.Price), 2),
                    TotalQuantitySold = g.Sum(sr => sr.OrderItem.Quantity),
                    SellingPercentage = Math.Round((decimal)g.Sum(sr => sr.OrderItem.Quantity) / totalQuantitySold * 100, 2)
                })
                .ToList();

            return productSalesSummary;
        }
        public async Task<List<HourlySalesSummaryDto>> GetHourlySalesSummary(DateTime startDate, DateTime endDate)
        {
            var salesRecords = await _context.SaleRecords
                .Where(sr => sr.SaleDate >= startDate && sr.SaleDate <= endDate)
                .ToListAsync();

            var uniqueOrderIds = salesRecords.Select(sr => sr.OrderId).Distinct().ToList();

            var totalUniqueOrders = uniqueOrderIds.Count;

            var hourlySales = salesRecords
                .Where(sr => uniqueOrderIds.Contains(sr.OrderId))
                .GroupBy(sr => sr.SaleDate.Hour)
                .Select(g => new HourlySalesSummaryDto
                {
                    Hour = FormatHour(g.Key),
                    SalesPercentage = Math.Round((decimal)g.Select(x => x.OrderId).Distinct().Count() / totalUniqueOrders * 100, 2)
                })
                .ToList();

            var fullHoursList = Enumerable.Range(0, 24)
                .Select(hour => new HourlySalesSummaryDto
                {
                    Hour = FormatHour(hour),
                    SalesPercentage = hourlySales.FirstOrDefault(h =>
                        h.Hour == FormatHour(hour))?.SalesPercentage ?? 0
                })
                .OrderBy(h => Array.IndexOf(new[] { "12AM", "1AM", "2AM", "3AM", "4AM", "5AM", "6AM", "7AM", "8AM", "9AM", "10AM", "11AM",
                                                 "12PM", "1PM", "2PM", "3PM", "4PM", "5PM", "6PM", "7PM", "8PM", "9PM", "10PM", "11PM" }, h.Hour))
                .ToList();

            return fullHoursList;
        }
        public async Task<RevenueAnalysisDto> GetRevenueAnalysis(DateTime startDate, DateTime endDate)
        {
            var salesRecords = await _context.SaleRecords
                .Include(sr => sr.Order)
                .Where(sr => sr.SaleDate >= startDate && sr.SaleDate <= endDate)
                .ToListAsync();

            var uniqueOrders = salesRecords
                .Where(sr => sr.Order != null)
                .GroupBy(sr => sr.OrderId)
                .Select(g => g.First().Order)
                .ToList();

            var totalRevenue = uniqueOrders.Sum(order => order.TotalSum);
            var daysDifference = (endDate - startDate).TotalDays + 1;

            var totalConfirmedClients = await _context.Users
                .Where(u => u.UserAccessType == UserAccesType.Customer && u.EmailConfirmed)
                .CountAsync();

            var newClientsInTimeframe = await _context.Users
                .Where(u =>
                    u.UserAccessType == UserAccesType.Customer &&
                    u.EmailConfirmed &&
                    u.CreationDate >= startDate &&
                    u.CreationDate <= endDate)
                .CountAsync();

            return new RevenueAnalysisDto
            {
                TotalRevenue = Math.Round(totalRevenue, 2),
                AverageOrderValue = uniqueOrders.Count > 0
                    ? Math.Round(totalRevenue / uniqueOrders.Count, 2)
                    : 0,
                TotalOrderCount = uniqueOrders.Count,
                DailyAverageRevenue = Math.Round(totalRevenue / (decimal)daysDifference, 2),
                TotalConfirmedClients = totalConfirmedClients,
                NewClientsInTimeframe = newClientsInTimeframe
            };
        }
        public async Task<List<CategorySalesDto>> GetCategorySalesPerformance(DateTime startDate, DateTime endDate)
        {
            var salesRecords = await _context.SaleRecords
                .Where(sr => sr.SaleDate >= startDate && sr.SaleDate <= endDate)
                .Include(sr => sr.OrderItem.Product.Category)
                .ToListAsync();

            var totalRevenue = salesRecords.Sum(sr => sr.OrderItem.Quantity * sr.OrderItem.Price);

            return salesRecords
                .GroupBy(sr => sr.OrderItem.Product.Category)
                .Select(g => new CategorySalesDto
                {
                    CategoryName = g.Key.Name,
                    TotalQuantitySold = g.Sum(sr => sr.OrderItem.Quantity),
                    TotalRevenue = g.Sum(sr => sr.OrderItem.Quantity * sr.OrderItem.Price),
                    SalesPercentage = Math.Round(
                        g.Sum(sr => sr.OrderItem.Quantity * sr.OrderItem.Price) / totalRevenue * 100,
                        2)
                })
                .OrderByDescending(x => x.TotalRevenue)
                .ToList();
        }
        public async Task<CustomerPurchasePatternDto> GetCustomerPurchasePattern(int userId)
        {
            var userSales = await _context.SaleRecords
                .Include(sr => sr.Order)
                .Where(sr => sr.Order.UserId == userId)
                .ToListAsync();

            if (!userSales.Any())
            {
                return null;
            }

            var uniqueOrders = userSales
                .Where(sr => sr.Order != null)
                .GroupBy(sr => sr.OrderId)
                .Select(g => g.First().Order)
                .ToList();

            var totalSpent = uniqueOrders.Sum(order => order.TotalSum);

            return new CustomerPurchasePatternDto
            {
                UserId = userId,
                TotalOrderCount = uniqueOrders.Count,
                TotalSpent = Math.Round(totalSpent, 2),
                AverageOrderValue = uniqueOrders.Count > 0
                    ? Math.Round(totalSpent / uniqueOrders.Count, 2)
                    : 0,
                LastPurchaseDate = userSales.Max(sr => sr.SaleDate)
            };
        }


        private string FormatHour(int hour)
        {
            return hour switch
            {
                0 => "12AM",
                < 12 => $"{hour}AM",
                12 => "12PM",
                _ => $"{hour - 12}PM"
            };
        }
    }

}
