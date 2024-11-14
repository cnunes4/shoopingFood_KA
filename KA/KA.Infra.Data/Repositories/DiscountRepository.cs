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
                _logger.LogError($"No Discounts found");
                return null;
            }

            return items;
        }


        /// <summary>
        /// Get discounts for on product id
        /// </summary>
        /// <param name="id">product id in DB</param>
        /// <returns>List with all discounts for this prodcut id </returns>
        public async Task<List<Discount>?> GetDiscountsByProductIdAsync(int id)
        {
            var items = await _context.Discounts.Where(x=> x.ItemToApply==id).ToListAsync();

            if (items == null || !items.Any())
            {
                _logger.LogError($"No Discounts for this product {id} found");
                return null;
            }

            return items;
        }


        
    }
}
