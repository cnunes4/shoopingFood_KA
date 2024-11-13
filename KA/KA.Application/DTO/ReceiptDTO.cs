namespace KA.Application.DTO
{

    public class ReceiptDTO
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public decimal TotalBeforeDiscount { get; set; }

        public decimal TotalDiscount { get; set; }

        public decimal TotalAfterDiscount { get; set; }

        public DateTime ReceiptDate { get; set; }
    }
}
