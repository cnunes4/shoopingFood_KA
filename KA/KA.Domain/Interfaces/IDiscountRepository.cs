using KA.Domain.Entities;

namespace KA.Domain.Interfaces
{ 
    public interface IDiscountRepository
    {
        Task<List<Discount>?> GetAllDiscountsAsync();
        Task<List<Discount>?> GetDiscountsByProductIdAsync(int id);
    }
}
