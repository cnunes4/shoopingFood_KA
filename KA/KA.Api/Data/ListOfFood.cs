using KA.Application.DTO;

namespace KAService.Data
{
    public class ListOfFood
    {
        public List<ProductDTO> FoodAvailable { get; set; }

        public List<PromotionDTO> Promotions { get; set; }
    }
}
