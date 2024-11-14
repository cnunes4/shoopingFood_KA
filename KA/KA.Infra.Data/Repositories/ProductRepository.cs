using KA.Domain.Entities;
using KA.Domain.Interfaces;
using KA.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace KA.Infra.Data.Repositories
{
    public class ProductRepository : IProductRepository
    {

        private readonly KADbContext _context;
        private readonly ILogger<ProductRepository> _logger;

        public ProductRepository(KADbContext context, ILogger<ProductRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Get all products in DB
        /// </summary>
        /// <returns>lIST OF PRODUCTS</returns>
        public async Task<List<Product>?> GetAllProductsAsync()
        {
             var items = await _context.Products.ToListAsync();

            if (items == null || !items.Any())
            {
                _logger.LogWarning($"No Products found");
                return null;
            }

            return items;
        }

        /// <summary>
        /// Get on product form DB by product id
        /// </summary>
        /// <param name="productId">product id</param>
        /// <returns>One product if exist </returns>
        public async Task<Product> GetProductByIDAsync(int productId)
        {
            return await _context.Products.FirstOrDefaultAsync(x => x.IdProduct == productId);
        }


     
    }
}
