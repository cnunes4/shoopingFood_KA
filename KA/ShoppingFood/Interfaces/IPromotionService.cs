using ShoopingFood.Models;

namespace ShoopingFood.Interfaces
{
    public interface IPromotionService
    {
        Task<ListOfPromotions?> GetAllPromotions();
    }

}
