using KA.Domain.Entities;
using KA.Domain.Interfaces;
using KA.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace KA.Infra.Data.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly KADbContext _context;

        public DiscountRepository(KADbContext context)
        {
            _context = context;
        }

        public async Task<List<Discount>?> GetAllDiscountsAsync()
        { 
            var items = await _context.Discounts.ToListAsync();

            if (!items.Any())
            {
                return null;
            }

            return items;
        }



        public async Task<List<Discount>?> GetDiscountsByProductIdAsync(int id)
        {
            var items = await _context.Discounts.Where(x=> x.ItemToApply==id).ToListAsync();

            if (!items.Any())
            {
                return null;
            }

            return items;
        }


        
    }
}
