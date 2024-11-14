namespace ShoopingFood.Models
{
    public class Discount
    {
        public int DiscountId { get; set; }

        public string Description { get; set; } = null!;

        public int Percentage { get; set; }
    }
}
