using KA.Domain.Entities;

namespace KA.Domain.Interfaces
{
    public interface IPromotionRepository 
    {
        Task<List<Promotion>?> GetAllPromotionsAsync();

    }
}
