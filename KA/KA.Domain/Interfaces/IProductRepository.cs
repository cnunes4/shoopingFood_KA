using KA.Domain.Entities;

namespace KA.Domain.Interfaces
{
    public interface IProductRepository
    {
        Task<List<Product>?> GetAllProductsAsync();
        Task<Product> GetProductByIDAsync(int productId);
    }
}
