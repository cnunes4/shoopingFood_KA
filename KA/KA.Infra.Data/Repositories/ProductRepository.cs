using KA.Domain.Entities;
using KA.Domain.Interfaces;
using KA.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace KA.Infra.Data.Repositories
{
    public class ProductRepository : IProductRepository
    {

        private readonly KADbContext _context;

        public ProductRepository(KADbContext context)
        {
            _context = context;
        }


        public async Task<List<Product>?> GetAllProductsAsync()
        {
             var items = await _context.Products.ToListAsync();

            if ( !items.Any())
            {
                return null;
            }

            return items;
        }

        public async Task<Product> GetItemByIDAsync(int id)
        {
            return await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
        }


     
    }
}
