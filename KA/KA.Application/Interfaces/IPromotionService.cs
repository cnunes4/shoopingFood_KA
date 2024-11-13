
using KA.Application.DTO;

namespace KA.Application.Interfaces
{
    public interface IPromotionService
    {
        Task<List<PromotionDTO>?> GetAllPromotionsAsync();
    }
}
