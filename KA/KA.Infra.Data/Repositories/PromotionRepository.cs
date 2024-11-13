using KA.Domain.Entities;
using KA.Domain.Interfaces;
using KA.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
namespace KA.Infra.Data.Repositories
{
    public class PromotionRepository : IPromotionRepository
    {

        private readonly KADbContext _context;

        public PromotionRepository(KADbContext context)
        {
            _context = context;
        }


        public async Task<List<Promotion>?> GetAllPromotionsAsync()
        {
             var items = await _context.Promotions.ToListAsync();

            if ( !items.Any())
            {
                return null;
            }

            return items;
        }



     
    }
}
