
namespace ShoopingFood.Models
{
    public class DetailsOfReceipt
    {
        public Receipt Details { get; set; }
        public List<ReceiptItem> Products { get; set; }
    }
}
