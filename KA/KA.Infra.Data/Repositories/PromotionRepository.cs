using KA.Domain.Entities;
using KA.Domain.Interfaces;
using KA.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
namespace KA.Infra.Data.Repositories
{
    public class PromotionRepository : IPromotionRepository
    {

        private readonly KADbContext _context;
        private readonly ILogger<PromotionRepository> _logger;
        public PromotionRepository(KADbContext context, ILogger<PromotionRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Get All Promotions 
        /// </summary>
        /// <returns>List of promotions</returns>
        public async Task<List<Promotion>?> GetAllPromotionsAsync()
        {
             var items = await _context.Promotions.Where(x=> x.IsEnabled).ToListAsync();

            if (items == null || !items.Any())
            {
                _logger.LogWarning($"No Promotions found");
                return null;
            }

            return items;
        }



     
    }
}
