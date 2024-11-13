namespace ShoopingFood.Models
{
    public class ReceiptItem
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal PriceAfterDiscount { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal PriceBeforeDiscount { get; set; }
    }
}
