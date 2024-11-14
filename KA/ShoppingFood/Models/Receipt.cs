using System.ComponentModel.DataAnnotations;

namespace ShoopingFood.Models
{
    public class Receipt
    {
        public int IdReceipt { get; set; }
        public int UserId { get; set; }
        public decimal TotalBeforeDiscount { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal TotalAfterDiscount { get; set; }
        public DateTime ReceiptDate { get; set; }
        public List<ReceiptItem> Items { get; set; } = new List<ReceiptItem>();
    }
}
