namespace ShoopingFood.Models
{
    public class Receipt
    {
        public List<ReceiptItem> Items { get; set; } = new List<ReceiptItem>();
        public decimal TotalBeforeDiscount { get; set; }
        public decimal TotalAfterDiscount { get; set; }
        public decimal TotalDiscount { get; set; }
        public DateTime ReceiptDate { get; set; }
    }
}
