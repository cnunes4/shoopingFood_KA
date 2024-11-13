namespace KA.Application.DTO
{
    public class ReceiptsproductDTO
    {
        public int ReceiptId { get; set; }

        public int ProductId { get; set; }

        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public decimal PriceAfterDiscount { get; set; }

        public decimal TotalDiscount { get; set; }
    }
}
