using System.ComponentModel.DataAnnotations;

namespace KA.Application.DTO
{
    public class ProductDTO
    {
        public int IdProduct { get; set; }

        public string Name { get; set; } = null!;

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public decimal PriceAfterDiscount { get; set; }

        public List<DiscountDTO> Discounts { get; set; }

    }
}
