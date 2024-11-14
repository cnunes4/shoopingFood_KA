using System.ComponentModel.DataAnnotations;

namespace KA.Application.DTO
{
    public class PromotionDTO
    {
        public int IdPromotion { get; set; }

        public string Description { get; set; } = null!;

        public int ProductIdToApply { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public int Percentage { get; set; }

    }
}
