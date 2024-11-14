using KA.Application.Services;
using KA.Domain.Entities;
using KA.Domain.Interfaces;
using KA.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace KA.Infra.Data.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly KADbContext _context;
        private readonly ILogger<DiscountRepository> _logger;
        public DiscountRepository(KADbContext context, ILogger<DiscountRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        /// <summary>
        /// Get All discounts on DB
        /// </summary>
        /// <returns>List of discounts</returns>
        public async Task<List<Discount>?> GetAllDiscountsAsync()
        { 
            var items = await _context.Discounts.ToListAsync();

            if (items == null || !items.Any())
            {
                _logger.LogWarning($"No Discounts found");
                return null;
            }

            return items;
        }


        /// <summary>
        /// Get discounts for on product id
        /// </summary>
        /// <param name="productId">product id in DB</param>
        /// <returns>List with all discounts for this prodcut id </returns>
        public async Task<List<Discount>?> GetDiscountsByProductIdAsync(int productId)
        {
            var discounts = await _context.Discounts
                .Where(d => _context.DiscountsProducts
                    .Any(dp => dp.ProductId == productId && dp.DiscountId == d.DiscountId) && d.IsEnabled)
                .ToListAsync();

            if (discounts == null || !discounts.Any())
            {
                _logger.LogWarning($"No Discounts for product with ID {productId} found");
                return null;
            }

            return discounts;

        }

        /// <summary>
        /// Get all discounts for all products
        /// </summary>
        /// <returns>List with all discounts </returns>
        public async Task<List<DiscountProduct>?> GetDiscountsForEachProductAsync()
        {
            var discounts = await _context.DiscountsProducts.ToListAsync();

            if (discounts == null || !discounts.Any())
            {
                _logger.LogWarning($"No Discounts");
                return null;
            }

            return discounts;

        }

    }
}
