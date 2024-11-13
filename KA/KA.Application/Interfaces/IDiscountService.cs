using KA.Application.DTO;

namespace KA.Application.Interfaces
{
    public interface IDiscountService
    {
        Task<List<DiscountDTO>?> GetAllDiscountsAsync();
        Task<List<DiscountDTO>?> GetDiscountsByProductIdAsync(int id);
    }
}
