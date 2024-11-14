using System.ComponentModel.DataAnnotations;

namespace KA.Application.DTO
{
    public class DiscountDTO
    {
        public int DiscountId { get; set; }

        public int ProductId { get; set; }

        public string Description { get; set; } 

        public int Percentage { get; set; }

        public bool IsEnabled { get; set; }
    }
}
